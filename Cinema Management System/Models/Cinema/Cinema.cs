using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_Management_System.Models.Cinema
{
    public class Cinema : INotifyPropertyChanged
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
        public string _Hall { get; set; }

        public string Hall { get { return _Hall; } set { _Hall = value; OnpropertyChanged(); } }
        public double _Price { get; set; }


        public double Price { get { return _Price; } set { _Price = value; OnpropertyChanged(); } }



        public Cinema()
        {

        }
    }
}
