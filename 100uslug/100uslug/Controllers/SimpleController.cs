using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _100uslug.Controllers
{
    public class SimpleController<T> : Controller
    {
        public SimpleController(IServiceProvider serviceProvider, string route)
        {

        }

        // GET: Simple
        public ActionResult Index()
        {
            return View();
        }

        // GET: Simple/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Simple/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Simple/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Simple/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Simple/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Simple/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Simple/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}