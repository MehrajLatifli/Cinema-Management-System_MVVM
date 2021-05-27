using Cinema_Management_System.Command;
using Cinema_Management_System.Models;
using Cinema_Management_System.Repo;
using Cinema_Management_System.Views;
using Cinema_Management_System.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Cinema_Management_System.ViewModels
{
    public class StartViewModel : INotifyPropertyChanged
    {



        //public ObservableCollection<MoviesfromOMDbAPI> _MoviesfromOMDbAPI_List { get; set; }
        //public ObservableCollection<MoviesfromOMDbAPI> MoviesfromOMDbAPI_List { get { return _MoviesfromOMDbAPI_List; } set { _MoviesfromOMDbAPI_List = value; OnpropertyChanged(); } }

        //private MoviesfromOMDbAPI _moviesfromOMDbAPI;
        //public MoviesfromOMDbAPI MoviesfromOMDbAPI { get { return _moviesfromOMDbAPI; } set { _moviesfromOMDbAPI = value; OnpropertyChanged(); } }


        public ObservableCollection<ImagesforCover> _ImagesforCover_List { get; set; }
        public ObservableCollection <ImagesforCover> ImagesforCover_List { get { return _ImagesforCover_List; } set { _ImagesforCover_List = value; OnpropertyChanged(); } }

        private ImagesforCover _ImagesforCovers { get; set; }

        public ImagesforCover ImagesforCovers { get { return _ImagesforCovers; } set { _ImagesforCovers = value; OnpropertyChanged(); } }


        public Repo_CovertPhoto CovertPhotoRepository { get; set; }

        public RelayCommand CloseStartWindowCommand { get; set; }

        public StartWindow StartWindows { get; set; }
        public ControlPanelWindow ControlPanelWindows { get; set; }

        public ControlPanelViewModel ControlPanelViewModels { get; set; }

        //public StartViewModel _StartViewModels { get; set; }

        //public StartViewModel StartViewModels { get { _StartViewModels = new StartViewModel(); return _StartViewModels; } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnpropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }



        public StartViewModel()
        {


            CovertPhotoRepository = new Repo_CovertPhoto();
            ImagesforCover_List = new ObservableCollection<ImagesforCover>(CovertPhotoRepository.GetAll());

            ImagesforCovers = new ImagesforCover
            {
                FilePath1 = "../../Images/Cover Images/gif.gif",
                FilePath2= "../../Images/CMS.png",

            };



            CloseStartWindowCommand = new RelayCommand((e) =>
            {

                if (StartWindows.StartProgressBar.Value == 100)
                {
                    StartWindows.Hide();


                    var ControlPanelWindows = new ControlPanelWindow();

                    ControlPanelViewModels = new ControlPanelViewModel();

                    ControlPanelViewModels.ControlPanelWindows = ControlPanelWindows;

                    
                    ControlPanelWindows.ShowDialog();
                }
 
                
            });

        }


    
         

        








    }
}
