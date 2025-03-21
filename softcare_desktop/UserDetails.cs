using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softcare_desktop
{
    public class UserDetails
    {
        private static string _userid;
        private static string _username;
        private static string _first_name;
        private static string _last_name;

        public static string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        public static string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public static string FirstName
        {
            get { return _first_name; }
            set { _first_name = value; }
        }
        public static string LastName
        {
            get { return _last_name; }
            set { _last_name = value; }
        }
        
    }
}
