using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Pruefung_Praktisch_Musterloesung.Models;
using System.Web.SessionState;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab2Controller : Controller
    {

        /**
        * Session ID fehler
        * 1) Die Session ID wird garnicht überprüft. Es kann irgenein login eingegeben werden und man kann sich so einloggen
        * 2) Funktionsweise(Url: http://localhost:50374/Lab2/login?sid=fakeID)
        * 3) Es wird irgendein login eingegeben und mann kan einfach so ins backend gelangen
        * 
        * Browser wird nicht überprüft
        * 1) Es wird nicht überprüft welcher browser verwendet wird
        * 2) http://localhost:50374/Lab2   (beim login wird der broswer nicht überprüft
        * 3) Es können keine Browser Spezifische sicherheitsmassnahmen getroffen werden
        * */

        public ActionResult Index() {

            var sessionid = Request.QueryString["sid"];

            if (string.IsNullOrEmpty(sessionid))
            {
                var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                sessionid = string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
            }
            //erstellen einer neuen ID
            /* Auskommentiert weil es nicht korrekt funktioniert!
            SessionIDManager manager = new SessionIDManager();
            string newID = manager.CreateSessionID(Context);
            bool redirected = false;
            bool isAdded = false;
            manager.SaveSessionID(Context, newID, out redirected, out isAdded);*/

            //replace old Session ID with new Session ID
            sessionid = newID;
            //Zuerst muss eine neue Session ID generiert werden
            ViewBag.sessionid = sessionid;

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];
            var sessionid = Request.QueryString["sid"];

            // hints:
            //var used_browser = Request.Browser.Platform;
            //var ip = Request.UserHostAddress;

            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkCredentials(username, password))
            {
                model.storeSessionInfos(username, password, sessionid);

                HttpCookie c = new HttpCookie("sid");
                c.Expires = DateTime.Now.AddMonths(2);
                c.Value = sessionid;
                Response.Cookies.Add(c);

                return RedirectToAction("Backend", "Lab2");
            }
            else
            {
                ViewBag.message = "Wrong Credentials";
                return View();
            }
        }

        public ActionResult Backend()
        {
            var sessionid = "";

            if (Request.Cookies.AllKeys.Contains("sid"))
            {
                sessionid = Request.Cookies["sid"].Value.ToString();
            }           

            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                sessionid = Request.QueryString["sid"];
            }
            
            // hints:
            //var used_browser = Request.Browser.Platform;
            //var ip = Request.UserHostAddress;

            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkSessionInfos(sessionid))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Lab2");
            }              
        }
    }
}