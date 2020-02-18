using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryLogin.Models;

namespace LibraryLogin.Controllers
{
    public class DescriptionController : Controller
    {
        string localhost = "localhost";
        string dbName = "library";
        string dbPassword = "root";
        string userName = "root";

        [ChildActionOnly]
        public ActionResult Index(Book defBook)
        {
            Book selectedBook = HttpContext.Session["book"] as Book;
            if(selectedBook == null)
            {
                if(defBook != null)
                    selectedBook = defBook;
                else
                    selectedBook = null;
            }
                HttpContext.Session["book"] = null;
                return PartialView("DIndex", selectedBook);         
        }
        public ActionResult ShowBookDetails(string book_id)
        {
            DataBase db = new DataBase();          
            HttpContext.Session["book"] = db.RequestToBookDetails(book_id);

            
            return RedirectToAction("Index", "Account");
        }
        public ActionResult ShowBookDetailsUser(string book_id)
        {
            DataBase db = new DataBase();
            HttpContext.Session["book"] = db.RequestToBookDetails(book_id);


            return RedirectToAction("Index", "User");
        }
    }
}