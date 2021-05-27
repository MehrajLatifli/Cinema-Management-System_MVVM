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
    /// Interaction logic for CreateCinema_UC.xaml
    /// </summary>
    public partial class CreateCinema_UC : UserControl
    {
        public CreateCinema_UC()
        {
            this.DataContext = new CreateCinema_UCViewModel() { CreateCinema_UCs = this };
            InitializeComponent();
        }

      
    }
}
