using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalCoordinationApplication.Model
{
    public class Account
    {
        private string email;
        private string password;
        private string role;

        public Account (string email, string password, string role) 
        {
            this.email = email;
            this.password = password;
            this.role = role;
        }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Role { get { return role; } set { role = value; } }
    }
}
