using Cinema_Management_System.Models;
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
using System.Xml;
using System.Xml.Linq;

namespace Cinema_Management_System.ViewModels
{
    public class Sell_Activities_UCViewModel : INotifyPropertyChanged
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


        private BuyMovies _BuyMovies;
        public BuyMovies BuyMovies { get { return _BuyMovies; } set { _BuyMovies = value; OnpropertyChanged(); } }
        public List<BuyMovies> _BuyMoviesList { get; set; }
        public List<BuyMovies> BuyMoviesList { get { return _BuyMoviesList; } set { _BuyMoviesList = value; OnpropertyChanged(); } }

        public Sell_Activities_UC Sell_Activities_UCs { get; set; }


        DateTime d = DateTime.Now;

        public Sell_Activities_UCViewModel()
        {

            try
            {

                string path = $@"../../DataBase/CinemaData/TicketList/TicketList {d.ToString("MM dd yyyy")}.xml";


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


                BuyMoviesList = new List<BuyMovies>();

                    BuyMoviesList = doc.Root
                    .Descendants("BuyMovies")
                   .Select(node => new BuyMovies
                   {

                       Price = double.Parse(node.Element("Price").Value),
                       Hall = node.Element("Hall").Value,
                       Title = node.Element("Title").Value,
                       Poster = node.Element("Poster").Value,
                       Chair = node.Element("Chair").Value,
                       StarRaiting = node.Element("StarRaiting").Value,



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
        }
    }
}
