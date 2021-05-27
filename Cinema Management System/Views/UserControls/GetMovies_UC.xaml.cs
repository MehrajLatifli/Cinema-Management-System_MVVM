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
    /// Interaction logic for GetMovies_UC.xaml
    /// </summary>
    public partial class GetMovies_UC : UserControl
    {
        public GetMovies_UC()
        {

            this.DataContext = new GetMovies_UCViewModel() { GetMovies_UCs = this };
            InitializeComponent();
        }
    }
}
