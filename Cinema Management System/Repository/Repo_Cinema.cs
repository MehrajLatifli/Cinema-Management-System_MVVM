using Cinema_Management_System.Models.Cinema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_Management_System.Repo
{
    public class Repo_Cinema
    {
        public Repo_Cinema()
        {
                            
        }

        public ObservableCollection<Cinema> GetCinemas()
        {
            return new ObservableCollection<Cinema>
            {



                new Cinema
                {
          
                    Hall="Hall 10:00",
                    Price=10,
                },

                new Cinema
                {
                 
                    Hall="Hall 13:00",
                    Price=15,
                },

                new Cinema
                {
                
                    Hall=" Hall 16:00",
                    Price=10,
                },

                new Cinema
                {
                  
                    Hall=" Hall 20:00",
                    Price=20,
                },

                new Cinema
                {
                  
                    Hall=" Hall 22:00",
                    Price=10,
                },
            };
        }
    }

   
}
