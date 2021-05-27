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


    public class UserPanel_UCViewModel : INotifyPropertyChanged
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

        public ObservableCollection<UserAccount> _UserAccount_List { get; set; }
        public ObservableCollection<UserAccount> UserAccount_List { get { return _UserAccount_List; } set { _UserAccount_List = value; OnpropertyChanged(); } }

        private UserAccount _UserAccounts;
        public UserAccount UserAccounts { get { return _UserAccounts; } set { _UserAccounts = value; OnpropertyChanged(); } }


        public RelayCommand UserSingInCommand { get; set; }

        public RelayCommand UserSingUpCommand { get; set; }


        public StartWindow StartWindows { get; set; }
        public StartViewModel StartViewModels { get; set; }

        public ControlPanelWindow ControlPanelWindows { get; set; }

        public ControlPanelViewModel ControlPanelViewModels { get; set; }


        // public StartViewModel StartViewModels { get { _StartViewModels = new StartViewModel(); return _StartViewModels; } }

        public UserPanel_UC UserPanel_UCs { get; set; }

        DateTime d = DateTime.Now;
        StringBuilder stringBuilder = new StringBuilder();
        string a = "";

        public ICommand GotoBuyMovies { get; set; }
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
        public UserPanel_UCViewModel()
        {




            UserSingUpCommand = new RelayCommand((e) =>
            {



                UserAccount_List = new ObservableCollection<UserAccount>();

                UserAccount_List.Add(new UserAccount
                {

                    _Name = UserPanel_UCs.Name.Text,
                    Surname = UserPanel_UCs.Surname.Text,
                    Username = UserPanel_UCs.Username2.Text,
                    Email = UserPanel_UCs.Email.Text,
                    Password = UserPanel_UCs.Password2.Password,
                    ConfirmPassword = UserPanel_UCs.ConfirmPassword.Password,

                });


                foreach (var item in UserAccount_List)
                {


                    if (item.Password == item.ConfirmPassword && item.Email.Contains("@"))
                    {
                        SaveUserAccount();
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

                            UserPanel_UCs.Name.BorderThickness = new Thickness(4, 4, 4, 4);

                            UserPanel_UCs.Name.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Surname) || item.Surname == "Surname")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            UserPanel_UCs.Surname.BorderThickness = new Thickness(4, 4, 4, 4);

                            UserPanel_UCs.Surname.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Username) || item.Username == "Username")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            UserPanel_UCs.Username2.BorderThickness = new Thickness(4, 4, 4, 4);

                            UserPanel_UCs.Username2.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Email) || item.Email == "Email")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            UserPanel_UCs.Email.BorderThickness = new Thickness(4, 4, 4, 4);

                            UserPanel_UCs.Email.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.Password) || item.Password == "Password")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            UserPanel_UCs.Password2.BorderThickness = new Thickness(4, 4, 4, 4);

                            UserPanel_UCs.Password2.BorderBrush = new SolidColorBrush(color1);

                        }

                        if (string.IsNullOrEmpty(item.ConfirmPassword) || item.Password == "ConfirmPassword")
                        {
                            Color color1 = new Color();
                            color1 = Color.FromArgb(255, 235, 76, 66);

                            UserPanel_UCs.ConfirmPassword.BorderThickness = new Thickness(4, 4, 4, 4);

                            UserPanel_UCs.ConfirmPassword.BorderBrush = new SolidColorBrush(color1);

                        }



                        notifier.ShowError("One of Fields or More field are empty");

                    }

                    if (!string.IsNullOrEmpty(item._Name) || item._Name != "Name")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        UserPanel_UCs.Name.BorderThickness = new Thickness(2, 2, 2, 2);

                        UserPanel_UCs.Name.BorderBrush = new SolidColorBrush(color1);
                    }

                    if (!string.IsNullOrEmpty(item.Surname) || item.Surname != "Surname")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        UserPanel_UCs.Surname.BorderThickness = new Thickness(2, 2, 2, 2);

                        UserPanel_UCs.Surname.BorderBrush = new SolidColorBrush(color1);
                    }

                    if (!string.IsNullOrEmpty(item.Username) || item.Username != "Username")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        UserPanel_UCs.Username2.BorderThickness = new Thickness(2, 2, 2, 2);

                        UserPanel_UCs.Username2.BorderBrush = new SolidColorBrush(color1);
                    }


                    if (!string.IsNullOrEmpty(item.Email) || item.Email != "Email")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        UserPanel_UCs.Email.BorderThickness = new Thickness(2, 2, 2, 2);

                        UserPanel_UCs.Email.BorderBrush = new SolidColorBrush(color1);
                    }


                    if (!string.IsNullOrEmpty(item.Password) || item.Password != "Password")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        UserPanel_UCs.Password2.BorderThickness = new Thickness(2, 2, 2, 2);

                        UserPanel_UCs.Password2.BorderBrush = new SolidColorBrush(color1);
                    }

                    if (!string.IsNullOrEmpty(item.ConfirmPassword) || item._Name != "ConfirmPassword")
                    {
                        Color color1 = new Color();
                        color1 = Color.FromArgb(255, 37, 191, 191);

                        UserPanel_UCs.ConfirmPassword.BorderThickness = new Thickness(2, 2, 2, 2);

                        UserPanel_UCs.ConfirmPassword.BorderBrush = new SolidColorBrush(color1);
                    }
                }





            });



            UserSingInCommand = new RelayCommand((e) =>
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load($@"../../DataBase/AccountData/RegisterUser/RegisterUser {d.ToString("MM dd yyyy")}.xml");
                    XmlNodeList xmlNodeList1 = doc.GetElementsByTagName("Username");
                    XmlNodeList xmlNodeList2 = doc.GetElementsByTagName("Password");

                    for (int i = 0; i < xmlNodeList1.Count; i++)
                    {


                        for (int j = 0; j < xmlNodeList2.Count; j++)
                        {


                            string user = UserPanel_UCs.Username.Text;

                            string pasw = UserPanel_UCs.Password.Password;


                            if (user == xmlNodeList1[i].InnerText.ToString() && pasw == xmlNodeList2[i].InnerText.ToString())
                            {
                                notifier.ShowSuccess($"Welcome {UserPanel_UCs.Username.Text}");

                                UserPanel_UCs.BuyTicketButton.Visibility = Visibility.Visible;
          


                                UserPanel_UCs.Grid_SingUp.Visibility = Visibility.Hidden;

                            }
                        }


                    }
                }
                catch (Exception)
                {
                    notifier.ShowError("Incorrect Sing in or your account has expired ");

                }

            });




            GotoBuyMovies = new RelayCommand(NavigatetoBuyMovies);
        }


        public void NavigatetoBuyMovies(object obj)
        {
            UserPanel_UCs.ProfileImage.Visibility = Visibility.Hidden;
            UserPanel_UCs.Username.Visibility = Visibility.Hidden;

            UserPanel_UCs.Password.Visibility = Visibility.Hidden;
            UserPanel_UCs.SingIn.Visibility = Visibility.Hidden;
            UserPanel_UCs.Name.Visibility = Visibility.Hidden;
            UserPanel_UCs.Surname.Visibility = Visibility.Hidden;
            UserPanel_UCs.Username2.Visibility = Visibility.Hidden;
            UserPanel_UCs.Password2.Visibility = Visibility.Hidden;
            UserPanel_UCs.ConfirmPassword.Visibility = Visibility.Hidden;
            UserPanel_UCs.Email.Visibility = Visibility.Hidden;
            UserPanel_UCs.SingUp.Visibility = Visibility.Hidden;

            UserPanel_UCs.Grid_SingIn.Visibility = Visibility.Hidden;
            UserPanel_UCs.Grid_SingUp.Visibility = Visibility.Hidden;

            UserPanel_UCs.Ccontrol1.Visibility = Visibility.Visible;



            SelectedViewModel = new BuyTicket_UCViewModel();
        }

        public void SaveUserAccount()
        {


            //  MessageBox.Show($"{UserPanel_UCs.Name.Text}");

            try
            {
                if (!Directory.Exists("../../DataBase/AccountData"))
                {
                    Directory.CreateDirectory("../../DataBase/AccountData");
                }
                if (!Directory.Exists("../../DataBase/AccountData/RegisterUser"))
                {
                    Directory.CreateDirectory("../../DataBase/AccountData/RegisterUser");
                }


                if (File.Exists($@"../../DataBase/RegisterUser/RegisterUser {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/RegisterUser/RegisterUser {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(ObservableCollection<UserAccount>));
                using (var fs = new FileStream($@"../../DataBase/AccountData/RegisterUser/RegisterUser {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, UserAccount_List);
                }


                UserAccount UserAccount = null;

                var xml2 = new XmlSerializer(typeof(ObservableCollection<UserAccount>));

                using (var fs2 = new FileStream($@"../../DataBase/AccountData/RegisterUser/RegisterUser {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    UserAccount = xml2.Deserialize(fs2) as UserAccount;

                    foreach (var item in UserAccount_List)
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
