using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _100uslug.Models;
using _100uslug.Common;

namespace _100uslug.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (Session.Sessions.ContainsKey("admin"))
            {
                ViewData["Message"] = "Main page.";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Auth");
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
