using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_Management_System.Models
{
   public  class Entity: INotifyPropertyChanged
    {
        public int _Id;


        public int Id
        {
            get { return _Id; }
            set { _Id = value; OnpropertyChanged(); }
        }

        public Entity()
        {
                
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnpropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
