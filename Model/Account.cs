using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalCoordinationApplication.Model
{
    // The Account class represents a basic user account with email, password, and role information.
    public class Account
    {
        // Private fields to store email, password, and role information.
        private string email;
        private string password;
        private string role;

        // Constructor to initialize an Account object with email, password, and role.
        public Account(string email, string password, string role)
        {
            this.email = email;
            this.password = password;
            this.role = role;
        }

        // Public property for accessing and modifying the email field.
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        // Public property for accessing and modifying the password field.
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        // Public property for accessing and modifying the role field.
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
    }

}
