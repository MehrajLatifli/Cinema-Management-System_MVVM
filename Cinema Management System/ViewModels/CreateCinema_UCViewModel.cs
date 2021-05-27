using Cinema_Management_System.Command;
using Cinema_Management_System.Models;
using Cinema_Management_System.Models.Cinema;
using Cinema_Management_System.Repo;
using Cinema_Management_System.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cinema_Management_System.ViewModels
{
    public class CreateCinema_UCViewModel : INotifyPropertyChanged
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

        public CreateCinema_UC CreateCinema_UCs { get; set; }

        public List<MoviesfromOMDbAPI> _MovieListInGetMovies { get; set; }
        public List<MoviesfromOMDbAPI> MovieListInGetMovies { get { return _MovieListInGetMovies; } set { _MovieListInGetMovies = value; OnpropertyChanged(); } }

        private MoviesfromOMDbAPI _movie;
        public MoviesfromOMDbAPI Movie { get { return _movie; } set { _movie = value; OnpropertyChanged(); } }


        public List<MovieList> _MovieListInGetMovies2 { get; set; }
        public List<MovieList> MovieListInGetMovies2 { get { return _MovieListInGetMovies2; } set { _MovieListInGetMovies2 = value; OnpropertyChanged(); } }



        public ObservableCollection<Cinema> _CinemaList { get; set; }
        public ObservableCollection<Cinema> CinemaList { get { return _CinemaList; } set { _CinemaList = value; OnpropertyChanged(); } }


        public ObservableCollection<Cinema> _CinemaList2 { get; set; }
        public ObservableCollection<Cinema> CinemaList2 { get { return _CinemaList2; } set { _CinemaList2 = value; OnpropertyChanged(); } }

        private Cinema _Cinema;
        public Cinema Cinema { get { return _Cinema; } set { _Cinema = value; OnpropertyChanged(); } }


        public ObservableCollection<PrepareTicket> _PrepareTicketList { get; set; }
        public ObservableCollection<PrepareTicket> PrepareTicketList { get { return _PrepareTicketList; } set { _PrepareTicketList = value; OnpropertyChanged(); } }

        private PrepareTicket _PrepareTicket;
        public PrepareTicket PrepareTicket { get { return _PrepareTicket; } set { _PrepareTicket = value; OnpropertyChanged(); } }

        public Repo_Cinema _Repo_Cinema { get; set; }

        public Repo_Cinema Repo_Cinema { get { return _Repo_Cinema; } set { _Repo_Cinema = value; OnpropertyChanged(); } }

        public RelayCommand SearchTextChangedCommand { get; set; }
        public RelayCommand AddToVisionCommand { get; set; }

        public RelayCommand RemoveListCommand { get; set; }

        DateTime d = DateTime.Now;


        public CreateCinema_UCViewModel()
        {

            MovieListInGetMovies = new List<MoviesfromOMDbAPI>();
            PrepareTicketList = new ObservableCollection<PrepareTicket>();

            Repo_Cinema = new Repo_Cinema();
            CinemaList = new ObservableCollection<Cinema>(Repo_Cinema.GetCinemas());


            Cinema = new Cinema
            {

                Hall = "Select Hall",
                Price = 0,
            };


            try
            {

                

                if (File.Exists($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml"))
                {
                    if (new System.IO.FileInfo($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml").Length <= 0)
                    {
                        File.Delete($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml");
                    }
                }

                if (File.Exists($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml"))
                {

                    var doc = XDocument.Load($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml");




                    MovieListInGetMovies = doc.Root
                     .Descendants("MoviesfromOMDbAPI")
                     .Select(node => new MoviesfromOMDbAPI
                     {
                         ID = int.Parse(node.Element("ID").Value),
                         Title = node.Element("Title").Value,
                         Year = node.Element("Year").Value,
                         Poster = node.Element("Poster").Value,
                         imdbRating = node.Element("imdbRating").Value,
                         Genre = node.Element("Genre").Value,
                         Plot = node.Element("Plot").Value,
                         Rated = node.Element("Rated").Value,
                         Released = node.Element("Released").Value,
                         Runtime = node.Element("Runtime").Value,
                         Director = node.Element("Director").Value,
                         Writer = node.Element("Writer").Value,
                         Actors = node.Element("Actors").Value,
                         Language = node.Element("Language").Value,
                         Country = node.Element("Country").Value,
                         Awards = node.Element("Awards").Value,
                         Metascore = node.Element("Metascore").Value,
                         imdbVotes = node.Element("imdbVotes").Value,
                         Type = node.Element("Type").Value,
                         Response = node.Element("Response").Value,
                     })
                     .ToList();




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



            AddToVisionCommand = new RelayCommand((e) =>
            {


                //  MessageBox.Show($"{CinemaList[CreateCinema_UCs.HallCombobox.SelectedIndex].Hall}");

                if (CreateCinema_UCs.listbox1.SelectedIndex >= 0)
                {
                    if (CreateCinema_UCs.HallCombobox.SelectedIndex >= 0 && !string.IsNullOrEmpty(CreateCinema_UCs.PriceTxtblock.Text))
                    {
                        PrepareTicketList.Add(new PrepareTicket
                        {
                            Hall = CinemaList[CreateCinema_UCs.HallCombobox.SelectedIndex].Hall,
                            Price = Convert.ToDouble(CreateCinema_UCs.PriceTxtblock.Text),


                            Title = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Title,
                            Year = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Year,
                            Poster = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Poster,
                            imdbRating = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].imdbRating,
                            Genre = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Genre,
                            Plot = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Plot,

                            Director = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Director,
                            Writer = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Writer,
                            Actors = MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Actors,
                            Awards= MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Awards,

                        });


                        SavePrepareTicketList();

                      
                            notifier.ShowSuccess($"You is adding {MovieListInGetMovies[CreateCinema_UCs.listbox1.SelectedIndex].Title} to vision.");
                        


                    }
                    else
                    {
                       
                        notifier.ShowWarning($"Select Hall");
                    }
                }

                else
                {
                    notifier.ShowWarning($"First select movies");
                    
                }

            });


            RemoveListCommand = new RelayCommand((e) =>
            {
                try
                {
                    if (CreateCinema_UCs.listbox1.SelectedIndex !=-1)
                    {


                        int index = 0;

                        MovieListInGetMovies.RemoveAt(CreateCinema_UCs.listbox1.SelectedIndex);

                        // CreateCinema_UCs.listbox1.Items.RemoveAt(CreateCinema_UCs.listbox1.SelectedIndex);

             

                        CreateCinema_UCs.listbox1.ItemsSource = null;
                        CreateCinema_UCs.listbox1.Items.Clear();



                        foreach (var item in MovieListInGetMovies)
                        {

                            CreateCinema_UCs.listbox1.Items.Add(item);


                        }
                    
                            ChangeAddCinemaList();

                    



                    }
                    else
                    {
                        notifier.ShowWarning($"First select movies");
                    }

                }
                catch (Exception)
                {


                }



            });

            SearchTextChangedCommand = new RelayCommand((e) =>
            {

                CreateCinema_UCs.listbox1.ItemsSource = null;

                if (string.IsNullOrEmpty(CreateCinema_UCs.SearchTextbox.Text) == false)
                {
                    CreateCinema_UCs.listbox1.Items.Clear();

                    foreach (var item in MovieListInGetMovies)
                    {


                        if (item.Title.Contains(CreateCinema_UCs.SearchTextbox.Text))
                        {
                            CreateCinema_UCs.listbox1.Items.Add(item);
                        }
                        CreateCinema_UCs.listbox1.ItemsSource = null;
                    }
                }

                else
                {
                    CreateCinema_UCs.listbox1.Items.Clear();

                    foreach (var item in MovieListInGetMovies)
                    {

                        CreateCinema_UCs.listbox1.Items.Add(item);


                    }
                }


                //if (CreateCinema_UCs.SearchTextbox.Text != string.Empty)
                //{
                //    CreateCinema_UCs.listbox1.ItemsSource = MovieListInGetMovies2.Where(t => t.Title.Contains(CreateCinema_UCs.SearchTextbox.Text));
                //}

            });




        }


        StringBuilder stringBuilder = new StringBuilder();

        string a = "";
        public void SavePrepareTicketList()
        {
            try
            {
                if (!Directory.Exists("../../DataBase/CinemaData"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData");
                }
                if (!Directory.Exists("../../DataBase/CinemaData/PrepareTicketList"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData/PrepareTicketList");
                }


                if (File.Exists($@"../../DataBase/CinemaData/PrepareTicketList/PrepareTicketList {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/CinemaData/PrepareTicketList/PrepareTicketList {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(ObservableCollection<PrepareTicket>));
                using (var fs = new FileStream($@"../../DataBase/CinemaData/PrepareTicketList/PrepareTicketList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, PrepareTicketList);
                }


                PrepareTicket adminAccount = null;

                var xml2 = new XmlSerializer(typeof(ObservableCollection<PrepareTicket>));

                using (var fs2 = new FileStream($@"../../DataBase/CinemaData/PrepareTicketList/PrepareTicketList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    adminAccount = xml2.Deserialize(fs2) as PrepareTicket;

                    foreach (var item in PrepareTicketList)
                    {
                        a = $"{item.Year} \n {item.Title} \n {item.Poster} \n {item.imdbRating} \n {item.Genre} \n {item.Plot}";

                        stringBuilder.Append(a);

                        a = stringBuilder.ToString();
                    }


                }
            }
            catch (Exception)
            {

            }
        }

        public void ChangeAddCinemaList()
        {


            //  MessageBox.Show($"{AdminPanel_UCs.Name.Text}");



            try
            {
                if (!Directory.Exists("../../DataBase/CinemaData"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData");
                }
                if (!Directory.Exists("../../DataBase/CinemaData/AddCinemaList"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData/AddCinemaList");
                }


                if (File.Exists($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(List<MoviesfromOMDbAPI>));
                using (var fs = new FileStream($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, MovieListInGetMovies);
                }


                MoviesfromOMDbAPI adminAccount = null;

                var xml2 = new XmlSerializer(typeof(List<MoviesfromOMDbAPI>));

                using (var fs2 = new FileStream($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    adminAccount = xml2.Deserialize(fs2) as MoviesfromOMDbAPI;

                    foreach (var item in MovieListInGetMovies)
                    {
                        a = $"{item.Year} \n {item.Title} \n {item.Poster} \n {item.imdbRating} \n {item.Genre} \n {item.Plot}";

                        stringBuilder.Append(a);

                        a = stringBuilder.ToString();
                    }


                }
            }
            catch (Exception)
            {

            }


        }
    }
}
