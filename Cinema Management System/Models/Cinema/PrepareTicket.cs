using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_Management_System.Models.Cinema
{

   public class PrepareTicket
    {
        public string Hall { get; set; }
        public double Price { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public string imdbRating { get; set; }

        public string Actors { get; set; }

        public string Poster { get; set; }

        public string Plot { get; set; }

        public string Awards { get; set; }


        public string Genre { get; set; }

        public string Director { get; set; }

        public string Writer { get; set; }

        public PrepareTicket()
        {
                            
        }
    }
}
