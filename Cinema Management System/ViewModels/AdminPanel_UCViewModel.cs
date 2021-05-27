using Cinema_Management_System.Command;
using Cinema_Management_System.Models.Accounts;
using Cinema_Management_System.Views;
using Cinema_Management_System.Views.UserControls;
using Cinema_Management_System.Views.Windows;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cinema_Management_System.ViewModels
{
    public class AdminPanel_UCViewModel : INotifyPropertyChanged
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

        public ObservableCollection<AdminAccount> _AdminAccount_List { get; set; }
        public ObservableCollection<AdminAccount> AdminAccount_List { get { return _AdminAccount_List; } set { _AdminAccount_List = value; OnpropertyChanged(); } }

        private AdminAccount _AdminAccounts;
        public AdminAccount AdminAccounts { get { return _AdminAccounts; } set { _AdminAccounts = value; OnpropertyChanged(); } }


        public RelayCommand AdminSingInCommand { get; set; }

        public RelayCommand AdminSingUpCommand { get; set; }


        public StartWindow StartWindows { get; set; }
        public StartViewModel StartViewModels { get; set; }

        public ControlPanelWindow ControlPanelWindows { get; set; }

        public ControlPanelViewModel ControlPanelViewModels { get; set; }

        public Sell_Activities_UCViewModel Sell_Activities_UCs { get; set; }


        // public StartViewModel StartViewModels { get { _StartViewModels = new StartViewModel(); return _StartViewModels; } }

        public AdminPanel_UC AdminPanel_UCs { get; set; }
        public UserPanel_UC UserPanel_UCs { get; set; }

        public UserPanel_UCViewModel UserPanel_UCViewModels { get; set; }

        DateTime d = DateTime.Now;
        StringBuilder stringBuilder = new StringBuilder();
        string a = "";

        public ICommand GotoMovies { get; set; }
        private object selectedViewModel;
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnpropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand GotoMovieList { get; set; }

        public ICommand GotoSellActivities { get; set; }

        private object selectedViewModel2;
        public object SelectedViewModel2
        {
            get { return selectedViewModel2; }
            set
            {
                selectedViewModel2 = value;
                OnpropertyChanged(nameof(SelectedViewModel2));
            }
        }

        public AdminPanel_UCViewModel()
        {

    


            AdminSingUpCommand = new RelayCommand((e) =>
            {



                AdminAccount_List = new ObservableCollection<AdminAccount>();

                AdminAccount_List.Add(new AdminAccount
                {

                    _Name = AdminPanel_UCs.Name.Text,
                    Surname = AdminPanel_UCs.Surname.Text,
                    Username = AdminPanel_UCs.Username2.Text,
                    Email = AdminPanel_UCs.Email.Text,
                    Password = AdminPanel_UCs.Password2.Password,
                    ConfirmPassword = AdminPanel_UCs.ConfirmPassword.Password,

                });


                foreach (var item in AdminAccount_List)
                {


                    if (item.Password == item.ConfirmPassword && item.Email.Contains("@"))
                    {
                        SaveAdminAccount();

                        notifier.ShowSuccess("The registration process was completed successfully. ");
                       
                    }

                    if (item.Password != item.ConfirmPassword || !item.Email.Contains("@"))
                    {
             

                        notifier.ShowWarning($"Email or password is incorrect.");
                    }

                    if (string.IsNullOrEmpty(item._Name) || string.IsNullOrEmpty(item.Surname) || string.IsNullOrEmpty(item.Username) || string.IsNullOrEmpty(item.Email) || string.IsNullOrEmpty(item.Password) || string.IsNullOrEmpty(item.ConfirmPassword))
                    {
                        if (string.IsNullOrEmpty(item._Name) || item._Name == "Name")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            AdminPanel_UCs.Name.BorderThickness = new Thickness(4, 4, 4, 4);

                            AdminPanel_UCs.Name.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Surname) || item.Surname == "Surname")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            AdminPanel_UCs.Surname.BorderThickness = new Thickness(4, 4, 4, 4);

                            AdminPanel_UCs.Surname.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Username) || item.Username == "Username")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            AdminPanel_UCs.Username2.BorderThickness = new Thickness(4, 4, 4, 4);

                            AdminPanel_UCs.Username2.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Email) || item.Email == "Email")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            AdminPanel_UCs.Email.BorderThickness = new Thickness(4, 4, 4, 4);

                            AdminPanel_UCs.Email.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Password) || item.Password == "Password")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            AdminPanel_UCs.Password2.BorderThickness = new Thickness(4, 4, 4, 4);

                            AdminPanel_UCs.Password2.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.ConfirmPassword) || item.Password == "ConfirmPassword")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            AdminPanel_UCs.ConfirmPassword.BorderThickness = new Thickness(4, 4, 4, 4);

                            AdminPanel_UCs.ConfirmPassword.BorderBrush = new SolidColorBrush(color1);

                        }


                        notifier.ShowError("One of Fields or More field are empty");
               

                    }

                    if (!string.IsNullOrEmpty(item._Name) || item._Name != "Name")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        AdminPanel_UCs.Name.BorderThickness = new Thickness(2, 2, 2, 2);

                        AdminPanel_UCs.Name.BorderBrush = new SolidColorBrush(color1);
                    }

                    if (!string.IsNullOrEmpty(item.Surname) || item.Surname != "Surname")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        AdminPanel_UCs.Surname.BorderThickness = new Thickness(2, 2, 2, 2);

                        AdminPanel_UCs.Surname.BorderBrush = new SolidColorBrush(color1);
                    }

                    if (!string.IsNullOrEmpty(item.Username) || item.Username != "Username")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        AdminPanel_UCs.Username2.BorderThickness = new Thickness(2, 2, 2, 2);

                        AdminPanel_UCs.Username2.BorderBrush = new SolidColorBrush(color1);
                    }


                    if (!string.IsNullOrEmpty(item.Email) || item.Email != "Email")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        AdminPanel_UCs.Email.BorderThickness = new Thickness(2, 2, 2, 2);

                        AdminPanel_UCs.Email.BorderBrush = new SolidColorBrush(color1);
                    }


                    if (!string.IsNullOrEmpty(item.Password) || item.Password != "Password")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        AdminPanel_UCs.Password2.BorderThickness = new Thickness(2, 2, 2, 2);

                        AdminPanel_UCs.Password2.BorderBrush = new SolidColorBrush(color1);
                    }

                    if (!string.IsNullOrEmpty(item.ConfirmPassword) || item._Name != "ConfirmPassword")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        AdminPanel_UCs.ConfirmPassword.BorderThickness = new Thickness(2, 2, 2, 2);

                        AdminPanel_UCs.ConfirmPassword.BorderBrush = new SolidColorBrush(color1);
                    }
                }



             


            });



            AdminSingInCommand = new RelayCommand((e) =>
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load($@"../../DataBase/AccountData/RegisterAdmin/RegisterAdmin {d.ToString("MM dd yyyy")}.xml");
                    XmlNodeList xmlNodeList1 = doc.GetElementsByTagName("Username");
                    XmlNodeList xmlNodeList2 = doc.GetElementsByTagName("Password");

                    for (int i = 0; i < xmlNodeList1.Count; i++)
                    {


                        for (int j = 0; j < xmlNodeList2.Count; j++)
                        {


                            string user = AdminPanel_UCs.Username.Text;

                            string pasw = AdminPanel_UCs.Password.Password;


                            if (user == xmlNodeList1[i].InnerText.ToString() && pasw == xmlNodeList2[i].InnerText.ToString())
                            {
                                

                                notifier.ShowSuccess($"Welcome {AdminPanel_UCs.Username.Text} ");

                                AdminPanel_UCs.AddMoviesButton.Visibility = Visibility.Visible;
                                AdminPanel_UCs.MovieList.Visibility = Visibility.Visible;
                                AdminPanel_UCs.SellActivitiesButton.Visibility = Visibility.Visible;
                                //  AdminPanel_UCs.BuyList.Visibility = Visibility.Visible;


                                AdminPanel_UCs.Grid_SingUp.Visibility = Visibility.Hidden;





                            }
                        }


                    }
                }
                catch (Exception)
                {
                    notifier.ShowError("Incorrect Sing in or your account has expired ");
           
                  
                }
  
            });


            GotoMovies = new RelayCommand(NavigateToMovies);

            GotoMovieList = new RelayCommand(NavigateToMovieList);

            GotoSellActivities = new RelayCommand(NavigateSellActivities);
        }

            private void NavigateToMovies(object obj)
            {
            AdminPanel_UCs.ProfileImage.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Username.Visibility = Visibility.Hidden;
         //   AdminPanel_UCs.PasswordLabel.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Password.Visibility = Visibility.Hidden;
            AdminPanel_UCs.SingIn.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Name.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Surname.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Username2.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Password2.Visibility = Visibility.Hidden;
            AdminPanel_UCs.ConfirmPassword.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Email.Visibility = Visibility.Hidden;
            AdminPanel_UCs.SingUp.Visibility = Visibility.Hidden;
        //    AdminPanel_UCs.Password2Label.Visibility = Visibility.Hidden;
        //    AdminPanel_UCs.ConfirmPassword2Label.Visibility = Visibility.Hidden;

            AdminPanel_UCs.AddMoviesButton.Visibility = Visibility.Hidden;
            AdminPanel_UCs.MovieList.Visibility = Visibility.Hidden;
          //  AdminPanel_UCs.BuyList.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Grid_SingIn.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Grid_SingUp.Visibility = Visibility.Hidden;

            AdminPanel_UCs.Ccontrol1.Visibility = Visibility.Visible;
            AdminPanel_UCs.SellActivitiesButton.Visibility = Visibility.Hidden;

            SelectedViewModel = new GetMovies_UCViewModel();
            }

        private void NavigateToMovieList(object obj)
        {
            AdminPanel_UCs.ProfileImage.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Username.Visibility = Visibility.Hidden;
            //   AdminPanel_UCs.PasswordLabel.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Password.Visibility = Visibility.Hidden;
            AdminPanel_UCs.SingIn.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Name.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Surname.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Username2.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Password2.Visibility = Visibility.Hidden;
            AdminPanel_UCs.ConfirmPassword.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Email.Visibility = Visibility.Hidden;
            AdminPanel_UCs.SingUp.Visibility = Visibility.Hidden;
            //    AdminPanel_UCs.Password2Label.Visibility = Visibility.Hidden;
            //    AdminPanel_UCs.ConfirmPassword2Label.Visibility = Visibility.Hidden;

            AdminPanel_UCs.AddMoviesButton.Visibility = Visibility.Hidden;
            AdminPanel_UCs.MovieList.Visibility = Visibility.Hidden;
          //  AdminPanel_UCs.BuyList.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Grid_SingIn.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Grid_SingUp.Visibility = Visibility.Hidden;

            AdminPanel_UCs.Ccontrol1.Visibility = Visibility.Visible;
            AdminPanel_UCs.SellActivitiesButton.Visibility = Visibility.Hidden;

            SelectedViewModel = new CreateCinema_UCViewModel();
        }

        private void NavigateSellActivities(object obj)
        {

            AdminPanel_UCs.Grid_Admin.Visibility = Visibility.Hidden;
            AdminPanel_UCs.Ccontrol2.Visibility = Visibility.Visible;

            SelectedViewModel2 = new Sell_Activities_UC();
        }

        public void SaveAdminAccount()
        {


            //  MessageBox.Show($"{AdminPanel_UCs.Name.Text}");

            try
            {
                if (!Directory.Exists(".../../DataBase/AccountData"))
                {
                    Directory.CreateDirectory("../../DataBase/AccountData");
                }
                if (!Directory.Exists("../../DataBase/AccountData/RegisterAdmin"))
                {
                    Directory.CreateDirectory("../../DataBase/AccountData/RegisterAdmin");
                }


                if (File.Exists($@"../../DataBase/AccountData/RegisterAdmin/RegisterAdmin {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/AccountData/RegisterAdmin/RegisterAdmin {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(ObservableCollection<AdminAccount>));
                using (var fs = new FileStream($@"../../DataBase/AccountData/RegisterAdmin/RegisterAdmin {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, AdminAccount_List);
                }


                AdminAccount adminAccount = null;

                var xml2 = new XmlSerializer(typeof(ObservableCollection<AdminAccount>));

                using (var fs2 = new FileStream($@"../../DataBase/AccountData/RegisterAdmin/RegisterAdmin {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    adminAccount = xml2.Deserialize(fs2) as AdminAccount;

                    foreach (var item in AdminAccount_List)
                    {
                        a = $"{item._Name} \n {item.Surname} \n {item.Password} \n {item.Email} \n {item.Username} \n {item.ConfirmPassword}";

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
