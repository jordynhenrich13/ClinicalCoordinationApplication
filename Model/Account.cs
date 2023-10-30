using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalCoordinationApplication.Model
{
    internal class Account
    {
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string role;

        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Role { get { return role; } set { role = value; } }
    }
}
