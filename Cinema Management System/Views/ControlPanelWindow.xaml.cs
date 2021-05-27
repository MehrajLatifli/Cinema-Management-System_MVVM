using Cinema_Management_System.Command;
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
using System.Windows.Shapes;

namespace Cinema_Management_System.Views
{
    /// <summary>
    /// Interaction logic for ControlPanelWindow.xaml
    /// </summary>
    public partial class ControlPanelWindow : Window
    {
        public RelayCommand APanelCommand { get; set; }






        public ControlPanelWindow()
        {
            ControlPanelViewModel c = new ControlPanelViewModel();
            this.DataContext = new ControlPanelViewModel() { ControlPanelWindows = this };

          //  APanelCommand = c.APanelCommand;
            InitializeComponent();



        }
    }
}
