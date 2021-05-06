using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _100uslug.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _100uslug.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        // POST: Auth/Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(IFormCollection collection)
        {
            try
            {
                if (collection["Login"] == "admin" && collection["Password"] == "admin")
                {
                    Session.Sessions.Add("admin", "admin");
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}