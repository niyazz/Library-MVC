using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryLogin.Models;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace LibraryLogin.Controllers
{
    public class HomeController : Controller {
        string localhost = "localhost";
        string dbName = "library";
        string dbPassword = "root";
        string userName = "root";
        public ActionResult Index()
        {
            if(HttpContext.Session["user"] == null)
                 return View("HIndex");
            else
                return RedirectToAction("Index", "Account");
        }
        public ActionResult Login(User enterUser)
        {
            ViewBag.NotValidData = "";
            
            DataBase db = new DataBase();
            User logUser = db.RequestToUser(enterUser);

            if(logUser != null)
            {
                HttpContext.Session["user"] = logUser;
                return RedirectToAction("Index","Account");
            }
            else
            {
                ViewBag.NotValidData = "Неправильные имя пользователя или пароль.";
                return View("HIndex");
            }
            
        }     
    }
}