using Cinema_Management_System.Command;
using Cinema_Management_System.Views;
using Cinema_Management_System.Views.UserControls;
using Cinema_Management_System.Views.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Cinema_Management_System.ViewModels
{
   public class ControlPanelViewModel : INotifyPropertyChanged
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

        public StartWindow StartWindows { get; set; }
        public StartViewModel StartViewModels { get; set; }
        public ControlPanelWindow ControlPanelWindows { get; set; }

        public AdminPanel_UCViewModel _AdminPanel_UCViewModels { get; set; }

     //  public AdminPanel_UCViewModel AdminPanel_UCViewModels { get; set; }

        public AdminPanel_UCViewModel AdminPanel_UCViewModels { get { return _AdminPanel_UCViewModels; } set { _AdminPanel_UCViewModels = value; OnpropertyChanged(); } }
        public AdminPanel_UC _AdminPanel_UCs { get; set; }

    //  public AdminPanel_UC AdminPanel_UCs { get; set; }
        public AdminPanel_UC AdminPanel_UCs { get { return _AdminPanel_UCs; } set { _AdminPanel_UCs = value; OnpropertyChanged(); } }


        public RelayCommand APanelCommand { get; set; }

        public RelayCommand UPanelCommand { get; set; }

 


                 public string Text { get; set; }

        public ICommand GotoAdminPanel { get; set; }

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

        private void NavigateToAdminPanel_UC(object obj)
        {
            SelectedViewModel = new AdminPanel_UC();
        }


        public ICommand GotoUserPanel { get; set; }



        private void NavigateToUserPanel_UC(object obj)
        {
            SelectedViewModel = new UserPanel_UC();
        }
        public ControlPanelViewModel()
        {
      


            GotoAdminPanel = new RelayCommand(NavigateToAdminPanel_UC);

            GotoUserPanel = new RelayCommand(NavigateToUserPanel_UC);




        }
            


    }
}
