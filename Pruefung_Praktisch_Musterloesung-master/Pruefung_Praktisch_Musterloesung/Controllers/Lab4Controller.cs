using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab4Controller : Controller
    {

        /**
        * 
        * Das Ziel ist es hier filter zu setzen für den Input welcher vom User gemacht wird
        * 
        * */

        public ActionResult Index() {

            Lab4IntrusionLog model = new Lab4IntrusionLog();
            return View(model.getAllData());   
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];//input filter setzen damit username auf email adresse mit kleinbuchstaben geprüft wird
            var password = Request["password"];//input filter setzen damit passwort auf min 10 max 20 zeichen geprüft wird

            bool intrusion_detected = false;
        
            // Hints
            // Request.Browser.Platform;
            // Request.UserHostAddress;

            Lab4IntrusionLog model = new Lab4IntrusionLog();

            // Hint:
            //model.logIntrusion();

            if (intrusion_detected)
            {
                return RedirectToAction("Index", "Lab4");
            }
            else
            {
                // check username and password
                // this does not have to be implemented!
                return RedirectToAction("Index", "Lab4");
            }
        }
    }
}