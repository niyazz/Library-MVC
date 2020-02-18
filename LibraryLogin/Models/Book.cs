using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryLogin.Models
{
    public class Book
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publiching { get; set; }
        public int Amount { get; set; }

        public Book()
        {

        }
        public Book(string id, string name, string author)
        {
            this.id = id;
            this.Name = name;
            this.Author = author;
        }
        public Book(string id, string name,  string author, string publishing, string amount)
        {
            this.id = id;
            this.Name = name;
            this.Author = author;
            this.Publiching = publishing;
            this.Amount = Convert.ToInt32(amount);
        }
    }
}