using Kutse_app.Models;
using System;
using System.Web.Mvc;
using System.Web.Helpers;

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
                // Замените "your-public-url.com" на ваш публичный адрес
                string baseUrl = "https://your-public-url.com";
                string yesLink = $"{baseUrl}/Home/Answer?guestName={guest.Name}&answer=true";
                string noLink = $"{baseUrl}/Home/Answer?guestName={guest.Name}&answer=false";

                string body = $"<html><body>" +
                              $"<p>Здравствуйте, {guest.Name}!</p>" +
                              $"<p>Приглашаем вас {ViewBag.Message}</p>" +
                              $"<p><a href='{yesLink}'><button>Да</button></a> " +
                              $"<a href='{noLink}'><button>Нет</button></a></p>" +
                              $"</body></html>";

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "glebsotjov@gmail.com";
                WebMail.Password = "dikf aipr rebq ytsk"; // Замените на ваш пароль
                WebMail.From = "glebsotjov@gmail.com";
                WebMail.Send("marina.oleinik@tthk.ee", "Vastus kutsele", body, isBodyHtml: true);

                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Mul on kuhju! Ei saa kirja saada!!!";
            }
        }

        public ActionResult Answer(string guestName, bool answer)
        {
            try
            {
                string answerText = answer ? "tuleb poele" : "ei tule peole";
                WebMail.Send("glebsotjov@gmail.com", "Ответ на приглашение", $"{guestName} {answerText}");
            }
            catch (Exception ex)
            {
                // Обработка ошибки отправки, например, запись в лог
                return View("Error"); //  Можно создать представление для отображения ошибки
            }

            return RedirectToAction("Thanks"); // Перенаправление на страницу Thanks
        }
    }
}