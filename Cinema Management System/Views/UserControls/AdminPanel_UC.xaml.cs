using Cinema_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace Cinema_Management_System.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AdminPanel_UC.xaml
    /// </summary>
    public partial class AdminPanel_UC : UserControl
    {
        public AdminPanel_UC()
        {
            InitializeComponent();
            this.DataContext = new AdminPanel_UCViewModel() { AdminPanel_UCs = this };

          




        }


      

    private void Username_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Username.Text == "Username")
            {
                Username.Text = null;


                Color color1 = new Color();
                color1 = Color.FromArgb(255, 37, 191, 191);

                Username.Foreground = new SolidColorBrush(color1);
            }
        }

        private void Username_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Username.Text == "")
            {

                Color color2 = new Color();
                color2 = Color.FromArgb(255, 110, 127, 128);

                Username.Text = "Username";
                Username.Foreground = new SolidColorBrush(color2);
            }
        }



        private void Name_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Name.Text == "Name")
            {
                Name.Text = null;



                Color color1 = new Color();
                color1 = Color.FromArgb(255, 37, 191, 191);

                Name.Foreground = new SolidColorBrush(color1);


            }
        }

        private void Name_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Name.Text == "")
            {

                Color color2 = new Color();
                color2 = Color.FromArgb(255, 110, 127, 128);

                Name.Text = "Name";
                Name.Foreground = new SolidColorBrush(color2);
            }
        }

        private void Surname_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Surname.Text == "Surname")
            {
                Surname.Text = null;



                Color color1 = new Color();
                color1 = Color.FromArgb(255, 37, 191, 191);

                Surname.Foreground = new SolidColorBrush(color1);


            }
        }

        private void Surname_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Surname.Text == "")
            {

                Color color2 = new Color();
                color2 = Color.FromArgb(255, 110, 127, 128);

                Surname.Text = "Surname";
                Surname.Foreground = new SolidColorBrush(color2);
            }
        }

        private void Username2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Username2.Text == "Username")
            {
                Username2.Text = null;



                Color color1 = new Color();
                color1 = Color.FromArgb(255, 37, 191, 191);

                Username2.Foreground = new SolidColorBrush(color1);


            }
        }

        private void Username2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Username2.Text == "")
            {

                Color color2 = new Color();
                color2 = Color.FromArgb(255, 110, 127, 128);

                Username2.Text = "Username";
                Username2.Foreground = new SolidColorBrush(color2);
            }
        }



        private void Email_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Email.Text == "Email")
            {
                Email.Text = null;



                Color color1 = new Color();
                color1 = Color.FromArgb(255, 37, 191, 191);

                Email.Foreground = new SolidColorBrush(color1);


            }
        }

        private void Email_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Email.Text == "")
            {

                Color color2 = new Color();
                color2 = Color.FromArgb(255, 110, 127, 128);

                Email.Text = "Email";
                Email.Foreground = new SolidColorBrush(color2);
            }
        }
    }
}
