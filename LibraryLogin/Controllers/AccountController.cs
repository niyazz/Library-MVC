using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryLogin.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibraryLogin.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            if(HttpContext.Session["user"] != null)
            {
                Book filterBook = null;
                ViewBag.User = HttpContext.Session["user"] as User;

                if (HttpContext.Session["findBook"] != null)
                {
                    filterBook = HttpContext.Session["findBook"] as Book;
                }       
                if (HttpContext.Session["books"] == null)
                {
                    DataBase db = new DataBase();
                    ViewBag.Books = db.RequestToBooks(filterBook);
                }
                else
                {
                    ViewBag.Books = HttpContext.Session["books"];
                }                   
                DefinePageNumber();
                ViewBag.page = HttpContext.Session["page"];

                return View("AIndex");
            }
            return RedirectToAction("Index", "Home");
        }
        public void DefinePageNumber()
        {
            if (HttpContext.Session["page"] == null)
            {
                Session["page"] = 0;
            }
        }
        public ActionResult PageUp()
        {
            if (Session["page"] != null )
            {
                Session["page"] = Convert.ToInt32(Session["page"]) + 10;
            }         
            return RedirectToAction("Index", "Account");
        }
        public ActionResult PageDown()
        {   if(Session["page"] != null && Convert.ToInt32(Session["page"]) > 0)
            {
                Session["page"] = Convert.ToInt32(Session["page"]) - 10;
            }
            return RedirectToAction("Index", "Account");
        }    
        public ActionResult OrderBook(string book_id)
        {
                DataBase db = new DataBase();
                if (HttpContext.Session["user"] != null)
                {
                    User curUser = HttpContext.Session["user"] as User;
                    if (db.RequestToOrderBook(book_id, curUser.id))
                    {
                        if (db.RequestToDecreaseBook(book_id))
                        {
                            ViewBag.OrderedBook = db.RequestToBookDetails(book_id);
                            return View("OrderBook", curUser);
                        }
                    }
            }
            return View("AIndex");
        }
       
        [HttpPost]
        public ActionResult Filter(Book book)
        {
            if(book.Name == null && book.Author == null)
            {
                HttpContext.Session["findBook"] = null;
            }
            else
            {
                HttpContext.Session["findBook"] = book;
            }
           
            return RedirectToAction("Index", "Account");
        }
    }
}