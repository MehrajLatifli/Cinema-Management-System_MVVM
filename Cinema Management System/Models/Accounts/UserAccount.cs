using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_Management_System.Models.Accounts
{
    public class UserAccount
    {
        public UserAccount()
        {
                        
        }

        public UserAccount(string name, string surname, string email, string password, string confirmPassword, string username)
        {
            _Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Username = username;
        }

        public string _Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Username { get; set; }
    }
}
