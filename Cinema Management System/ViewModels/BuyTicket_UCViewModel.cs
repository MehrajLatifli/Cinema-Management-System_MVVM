using CefSharp;
using Cinema_Management_System.Command;
using Cinema_Management_System.Models;
using Cinema_Management_System.Models.Cinema;
using Cinema_Management_System.Views;
using Cinema_Management_System.Views.UserControls;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cinema_Management_System.ViewModels
{
    public class BuyTicket_UCViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnpropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


        public BuyTicket_UC BuyTicket_UCs { get; set; }

        public ControlPanelWindow ControlPanelWindows { get; set; }



        private MoviesfromOMDbAPI _movie;
        public MoviesfromOMDbAPI Movie { get { return _movie; } set { _movie = value; OnpropertyChanged(); } }


        public List<MoviesfromOMDbAPI> _MovieListInGetMovies { get; set; }
        public List<MoviesfromOMDbAPI> MovieListInGetMovies { get { return _MovieListInGetMovies; } set { _MovieListInGetMovies = value; OnpropertyChanged(); } }

        private PrepareTicket _PrepareTicket;
        public PrepareTicket PrepareTicket { get { return _PrepareTicket; } set { _PrepareTicket = value; OnpropertyChanged(); } }
        public List<PrepareTicket> _PrepareTicket_List { get; set; }
        public List<PrepareTicket> PrepareTicket_List { get { return _PrepareTicket_List; } set { _PrepareTicket_List = value; OnpropertyChanged(); } }

        private BuyMovies _BuyMovies;
        public BuyMovies BuyMovies { get { return _BuyMovies; } set { _BuyMovies = value; OnpropertyChanged(); } }
        public ObservableCollection<BuyMovies> _BuyMoviesList { get; set; }
        public ObservableCollection<BuyMovies> BuyMoviesList { get { return _BuyMoviesList; } set { _BuyMoviesList = value; OnpropertyChanged(); } }


        private Chair _Chair;
        public Chair Chair { get { return _Chair; } set { _Chair = value; OnpropertyChanged(); } }
        public ObservableCollection<Chair> _Chair_List { get; set; }
        public ObservableCollection<Chair> Chair_List { get { return _Chair_List; } set { _Chair_List = value; OnpropertyChanged(); } }

        public ObservableCollection<Chair> _Chair_List2 { get; set; }
        public ObservableCollection<Chair> Chair_List2 { get { return _Chair_List2; } set { _Chair_List2 = value; OnpropertyChanged(); } }


        public RelayCommand SearchTextChangedCommand { get; set; }

        public RelayCommand SelectedItemChangedCommand { get; set; }

        public RelayCommand BuyTicketCommand { get; set; }

        public RelayCommand UnCheckedChairCommand { get; set; }

        public RelayCommand CheckedChairCommand { get; set; }

        DateTime d = DateTime.Now;

        public BuyTicket_UCViewModel()
        {



            try
            {
                PrepareTicket_List = new List<PrepareTicket>();
                MovieListInGetMovies = new List<MoviesfromOMDbAPI>();
                string path = $@"../../DataBase/CinemaData/PrepareTicketList/PrepareTicketList {d.ToString("MM dd yyyy")}.xml";


                if (File.Exists(path))
                {
                    if (new System.IO.FileInfo(path).Length <= 0)
                    {
                        File.Delete(path);
                    }
                }

                if (File.Exists(path))
                {

                    var doc = XDocument.Load(path);

                    XNamespace df = doc.Root.Name.Namespace;

                    //   MessageBox.Show($"{a}");



                    PrepareTicket_List = doc.Root
                    .Descendants("PrepareTicket")
                   .Select(node => new PrepareTicket
                   {

                       Price = double.Parse(node.Element("Price").Value),
                       Hall = node.Element("Hall").Value,
                       Title = node.Element("Title").Value,
                       Year = node.Element("Year").Value,
                       imdbRating = node.Element("imdbRating").Value,
                       Actors = node.Element("Actors").Value,
                       Poster = node.Element("Poster").Value,
                       Genre = node.Element("Genre").Value,
                       Director = node.Element("Director").Value,
                       Writer = node.Element("Writer").Value,
                       Plot = node.Element("Plot").Value,
                       Awards = node.Element("Awards").Value,

                   }).ToList();






                }

                //CreateCinema_UCs.listbox1.ItemsSource = null;

                //  CreateCinema_UCs.listbox1.Items.Clear();

                //   MessageBox.Show($"{MovieListInGetMovies2[0].Title} {MovieListInGetMovies2[1].Title}");

                // CreateCinema_UCs.listbox1.ItemsSource = MovieListInGetMovies2;

            }
            catch (XmlException ex)
            {

                // MessageBox.Show($"{ex.Message}");
            }

            SearchTextChangedCommand = new RelayCommand((e) =>
            {
                BuyTicket_UCs.listbox1.ItemsSource = null;

                if (string.IsNullOrEmpty(BuyTicket_UCs.SearchTextbox.Text) == false)
                {
                    BuyTicket_UCs.listbox1.Items.Clear();

                    foreach (var item in PrepareTicket_List)
                    {


                        if (item.Title.Contains(BuyTicket_UCs.SearchTextbox.Text))
                        {
                            BuyTicket_UCs.listbox1.Items.Add(item);
                        }
                        BuyTicket_UCs.listbox1.ItemsSource = null;
                    }
                }

                else
                {
                    BuyTicket_UCs.listbox1.Items.Clear();

                    foreach (var item in PrepareTicket_List)
                    {

                        BuyTicket_UCs.listbox1.Items.Add(item);


                    }
                }

            });



            SelectedItemChangedCommand = new RelayCommand((e) =>
            {


                try
                {
                    if (BuyTicket_UCs.listbox1.SelectedIndex < 0)
                    {
                        SearchResultVideos("upcoming movies", $"{d}").GetAwaiter();
                    }

                    else
                    {
                        //MessageBox.Show($"{addMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Title}");
                        SearchResultVideos(PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title, PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Year).GetAwaiter();
                    }
                }
                catch (Exception)
                {


                }


            });



            CheckedChairCommand = new RelayCommand((e) =>
            {
                Chair_List = new ObservableCollection<Chair>();
                if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 1",
                    });

                    SaveChair();

                    notifier.ShowInformation($"You is selecting Chair_No 1");
                }

                if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 2",
                    });


                    SaveChair();

                    notifier.ShowInformation($"You is selecting Chair_No 2");
                }

                if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 3",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 3");
                }


                if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 4",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 4");
                }

                if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 5",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 5");
                }


                if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 6",
                    });

                    SaveChair();



                    notifier.ShowInformation($"You is selecting Chair_No 6");
                }


                if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 7",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 7");
                }


                if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 8",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 8");
                }


                if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 9",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 9");
                }


                if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 10",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 10");
                }


                if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 11",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 11");
                }


                if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 12",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 12");
                }


                if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 13",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 13");
                }


                if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 14",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 14");
                }


                if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 15",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 15");
                }


                if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 16",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 16");
                }


                if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 17",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 17");
                }


                if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 18",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 18");
                }


                if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 19",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 19");
                }

                if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                {

                    Chair_List.Add(new Chair
                    {
                        Chair_No = "Chair_No: 20",
                    });

                    SaveChair();


                    notifier.ShowInformation($"You is selecting Chair_No 20");
                }
            });




            UnCheckedChairCommand = new RelayCommand((e) =>
            {

                Chair_List = new ObservableCollection<Chair>();
                Chair_List2 = new ObservableCollection<Chair>();


                if (BuyTicket_UCs.ChairButton1.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("1"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton2.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("2"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton3.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("3"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton4.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("4"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton5.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("5"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton6.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("6"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton7.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("7"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton8.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("8"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton9.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("9"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton10.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("10"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton11.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("11"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton12.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("12"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton13.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("13"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton14.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("14"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton15.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("15"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton16.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("16"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }


                if (BuyTicket_UCs.ChairButton17.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("17"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton18.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("18"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton19.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("19"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }

                if (BuyTicket_UCs.ChairButton20.IsChecked == false)
                {

                    foreach (var item in Chair_List)
                    {
                        if (item.Chair_No.Equals("20"))
                        {
                            Chair_List.Remove(item);
                            break;
                        }

                    }





                    if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "1") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });

                        }


                    }

                    if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "2") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "2",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "3") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "3",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "4") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "4",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "5") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "5",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "6") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "6",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "7") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "7",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "8") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "8",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "9") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "9",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "10") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "10",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "11") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "11",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "12") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "12",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "13") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "13",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "14") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "14",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "15") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "15",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "16") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "16",
                            });

                        }
                    }


                    if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "17") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "1",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "18") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "18",
                            });


                        }
                    }


                    if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "19") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "19",
                            });


                        }
                    }

                    if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                    {

                        if (Chair_List.Any(p => p.Chair_No == "20") == false)
                        {

                            Chair_List.Add(new Chair
                            {
                                Chair_No = "20",
                            });

                        }
                    }




                    SaveChair();

                }
            });

            BuyTicketCommand = new RelayCommand((e) =>
            {
                BuyMoviesList = new ObservableCollection<BuyMovies>();
             

                if (BuyTicket_UCs.listbox1.SelectedIndex != -1)
                {

                    if (BuyTicket_UCs.ChairButton1.IsChecked == false && BuyTicket_UCs.ChairButton2.IsChecked == false && BuyTicket_UCs.ChairButton3.IsChecked == false && BuyTicket_UCs.ChairButton4.IsChecked == false && BuyTicket_UCs.ChairButton5.IsChecked == false
                    && BuyTicket_UCs.ChairButton6.IsChecked == false && BuyTicket_UCs.ChairButton7.IsChecked == false && BuyTicket_UCs.ChairButton8.IsChecked == false && BuyTicket_UCs.ChairButton9.IsChecked == false && BuyTicket_UCs.ChairButton10.IsChecked == false
                    && BuyTicket_UCs.ChairButton11.IsChecked == false && BuyTicket_UCs.ChairButton12.IsChecked == false && BuyTicket_UCs.ChairButton13.IsChecked == false && BuyTicket_UCs.ChairButton14.IsChecked == false && BuyTicket_UCs.ChairButton15.IsChecked == false
                    && BuyTicket_UCs.ChairButton16.IsChecked == false && BuyTicket_UCs.ChairButton17.IsChecked == false && BuyTicket_UCs.ChairButton18.IsChecked == false && BuyTicket_UCs.ChairButton19.IsChecked == false && BuyTicket_UCs.ChairButton20.IsChecked == false)
                    {
             

                        notifier.ShowWarning($"Seat selection are forgotten. ");
                    }

                    else if (BuyTicket_UCs.starRaiting == 0)
                    {
             

                        notifier.ShowWarning($"Star Raiting is forgotten.");
                    }
                    else
                    {


                        if (BuyTicket_UCs.ChairButton1.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 1",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();

                          
                        }

                        if (BuyTicket_UCs.ChairButton2.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 2",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                   
                            SaveTicket();
                 
                        }

                        if (BuyTicket_UCs.ChairButton3.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 3",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                       
                            SaveTicket();
                        }

                        if (BuyTicket_UCs.ChairButton4.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 4",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });


                            SaveTicket();
                           
                        }

                        if (BuyTicket_UCs.ChairButton5.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 5",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                       
                            SaveTicket();
                         
                        }

                        if (BuyTicket_UCs.ChairButton6.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 6",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

         
                            SaveTicket();
                     
                        }

                        if (BuyTicket_UCs.ChairButton7.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 7",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                     
                        }

                        if (BuyTicket_UCs.ChairButton8.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 8",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                         
                        }

                        if (BuyTicket_UCs.ChairButton9.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 9",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
        
                        }

                        if (BuyTicket_UCs.ChairButton10.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 10",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
            
                        }


                        if (BuyTicket_UCs.ChairButton11.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 11",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                     
                        }


                        if (BuyTicket_UCs.ChairButton12.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 12",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
     
                        }

                        if (BuyTicket_UCs.ChairButton13.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 13",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
   
                        }

                        if (BuyTicket_UCs.ChairButton14.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 14",
                                StarRaiting = $"{$"{BuyTicket_UCs.starRaiting}"}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                   
                        }

                        if (BuyTicket_UCs.ChairButton15.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 15",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                  
                        }

                        if (BuyTicket_UCs.ChairButton16.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 16",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                    
                        }

                        if (BuyTicket_UCs.ChairButton17.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 17",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                   
                        }

                        if (BuyTicket_UCs.ChairButton18.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 18",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                      
                        }

                        if (BuyTicket_UCs.ChairButton19.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 19",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                        
                        }

                        if (BuyTicket_UCs.ChairButton20.IsChecked == true)
                        {
                            BuyMoviesList.Add(new BuyMovies
                            {
                                Chair = "Chair_No: 20",
                                StarRaiting =$"User's vote: {BuyTicket_UCs.starRaiting}",
                                Hall = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Hall,
                                Poster = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Poster,
                                Price = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Price,
                                Title = PrepareTicket_List[BuyTicket_UCs.listbox1.SelectedIndex].Title,
                            });

                            SaveTicket();
                           ;
                        }
                        SavePDF();
                    }

     





                }

                else
                {
             

                    notifier.ShowWarning($"Movie selection is forgotten. ");
                }

            });


        }


        void SavePDF ()
        {


            ControlPanelWindows = new ControlPanelWindow();
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(BuyTicket_UCs, "Ticket");
            }



        }








        StringBuilder stringBuilder = new StringBuilder();

        string a = "";
        public void SaveChair() 
        {
            try
            {
                if (!Directory.Exists("../../DataBase/CinemaData"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData");
                }
                if (!Directory.Exists("../../DataBase/CinemaData/ChairList"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData/ChairList");
                }


                if (File.Exists($@"../../DataBase/CinemaData/ChairList/ChairList {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/CinemaData/ChairList/ChairList {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(ObservableCollection<Chair>));
                using (var fs = new FileStream($@"../../DataBase/CinemaData/ChairList/ChairList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, Chair_List);
                }


                Chair adminAccount = null;

                var xml2 = new XmlSerializer(typeof(ObservableCollection<Chair>));

                using (var fs2 = new FileStream($@"../../DataBase/CinemaData/ChairList/ChairList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    adminAccount = xml2.Deserialize(fs2) as Chair;

                    foreach (var item in Chair_List)
                    {
                        a = $"{item.Chair_No} ";

                        stringBuilder.Append(a);

                        a = stringBuilder.ToString();
                    }


                }
            }
            catch (Exception)
            {

            }
        }

        public void SaveTicket()
        {


            try
            {
                if (!Directory.Exists("../../DataBase/CinemaData"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData");
                }
                if (!Directory.Exists("../../DataBase/CinemaData/TicketList"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData/TicketList");
                }


                if (File.Exists($@"../../DataBase/CinemaData/TicketList/TicketList {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/CinemaData/TicketList/TicketList {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(ObservableCollection<BuyMovies>));
                using (var fs = new FileStream($@"../../DataBase/CinemaData/TicketList/TicketList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, BuyMoviesList);
                }


                BuyMovies adminAccount = null;

                var xml2 = new XmlSerializer(typeof(ObservableCollection<BuyMovies>));

                using (var fs2 = new FileStream($@"../../DataBase/CinemaData/TicketList/TicketList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    adminAccount = xml2.Deserialize(fs2) as BuyMovies;

                    foreach (var item in BuyMoviesList)
                    {
                        a = $"{item.Chair} \n {item.Title} \n {item.Poster} \n {item.StarRaiting} \n {item.Hall} \n {item.Price}";

                        stringBuilder.Append(a);

                        a = stringBuilder.ToString();
                    }


                }
            }
            catch (Exception)
            {

            }
        }

        private async Task SearchResultVideos(string movieName, string year)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = ConfigurationManager.AppSettings["YoutubeAPI_Key"],
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = $"{movieName} {year} Official Trailer"; // Replace with your search term.
            searchListRequest.MaxResults = 1;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            string v = "";

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //   videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        videos.Add(searchResult.Id.VideoId);
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            v = videos[0];


            // MessageBox.Show($" \n www./youtu.be/{v}");



            if (BuyTicket_UCs.ChromiumBrowser.Address == null)
            {

                BuyTicket_UCs.Stackyoutubewb.Visibility = Visibility.Visible;
                BuyTicket_UCs.ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{v}";
            }


            if (BuyTicket_UCs.ChromiumBrowser.Address != null)
            {

                BuyTicket_UCs.ChromiumBrowser.Address = string.Empty;
                BuyTicket_UCs.ChromiumBrowser.Reload();
                BuyTicket_UCs.ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{v}";






            }







        }

    }
}
