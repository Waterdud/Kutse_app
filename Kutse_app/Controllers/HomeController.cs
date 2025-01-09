﻿using Kutse_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Kutse_app.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Ootan sind minu poele! Palun tule!!!";
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 10 ? "Tere hommikust!" : "Tere päevast!";
            return View();
        }
        [HttpGet]
        public ViewResult Ankeet()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Ankeet(Guest guest)
        {
            E_Mail(guest);
            if (ModelState.IsValid)
            {
                return View("Thanks", guest);
            }
            else
            {
                return View();
            }
        }
        public void E_Mail(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "glebsotjov@gmail.com";
                WebMail.Password = "dikf aipr rebq ytsk";
                WebMail.From = "glebsotjov@gmail.com";
                WebMail.Send("glebsotjov@gmail.com", "Vastus kutsele", guest.Name + " Vastus " + ((guest.WillAttend ?? false) ?
                    "tuleb poele " : "ei tule poele"));
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Mul on kuhju! Ei saa kirja saada!!!";
            }
        }
    }
}