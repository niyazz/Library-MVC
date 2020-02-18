using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryLogin.Models;

namespace LibraryLogin.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.Session["user"] != null)
            {
                ViewBag.User = HttpContext.Session["user"] as User;

                DataBase db = new DataBase();
                List<Book> user_books = db.RequestToUserBooks(ViewBag.User.id);

                ViewBag.UserBooks = user_books;

                return View("UIndex");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ReturnBook(string book_id)
        {
            DataBase db = new DataBase();
            if (HttpContext.Session["user"] != null)
            {
                User curUser = HttpContext.Session["user"] as User;
                if (db.RequestToReturnBook(book_id, curUser.id))
                {
                    if (db.RequestToIncreaseBook(book_id))
                    {
                        return View("ReturnBook", curUser);
                    }
                }
            }
            return View("UIndex");
        }
    }
   
}