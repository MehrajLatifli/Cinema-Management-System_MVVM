using Cinema_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cinema_Management_System.Views.UserControls
{
    /// <summary>
    /// Interaction logic for BuyTicket_UC.xaml
    /// </summary>
    public partial class BuyTicket_UC : UserControl
    {
        public BuyTicket_UC()
        {

            this.DataContext = new BuyTicket_UCViewModel() { BuyTicket_UCs = this };
            InitializeComponent();



        }

        string s = "../../Images/Logo/Star.png";
        string s2 = "../../Images/Logo/Empty Star.png";

       public int starRaiting = 0;

        private void Image_S1_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Image_S1.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            starRaiting=1;
        }

        private void Image_S2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image_S1.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S2.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            starRaiting =2;
        }

        private void Image_S3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image_S1.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S2.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S3.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            starRaiting = 3;
        }

        private void Image_S4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image_S1.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S2.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S3.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S4.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            starRaiting = 4;
        }

        private void Image_S5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image_S1.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S2.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S3.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S4.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            Image_S5.Source = new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            starRaiting = 5;

        }

        private void Image_R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image_S1.Source = new BitmapImage(new Uri(s2, UriKind.RelativeOrAbsolute));
            Image_S2.Source = new BitmapImage(new Uri(s2, UriKind.RelativeOrAbsolute));
            Image_S3.Source = new BitmapImage(new Uri(s2, UriKind.RelativeOrAbsolute));
            Image_S4.Source = new BitmapImage(new Uri(s2, UriKind.RelativeOrAbsolute));
            Image_S5.Source = new BitmapImage(new Uri(s2, UriKind.RelativeOrAbsolute));

            starRaiting = 0;

        }
    }
}
