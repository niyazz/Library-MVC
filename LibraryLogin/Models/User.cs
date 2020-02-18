using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryLogin.Models
{
    public class User
    {
        public string Name { get; set; }

        public string id { get; set; }

        public string Password { get; set; }

        public User(string id, string name, string pass)
        {
            this.Name = name;
            this.id = id;
            this.Password = pass;
        }
        public User()
        {

        }

    }
    
}