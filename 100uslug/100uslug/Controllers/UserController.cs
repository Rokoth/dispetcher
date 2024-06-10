using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoUslug.Contract.Interfaces;
using StoUslug.Contract.Models;

namespace _100uslug.Controllers
{
    public class UserController : Controller
    {
        private IDataService<User> _dataService;
        private ILogger<UserController> _logger;

        public UserController(IDataService<User> dataService, ILogger<UserController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        // GET: Simple
        public async Task<ActionResult> IndexAsync(IFilter<User> filter)
        {
            try
            {
                var result = await _dataService.GetList(filter);
                if (result == null)
                    return RedirectToAction("Error", "Home", new { Error = "Ошибка получения пользователей" });

                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка получения пользователей", ex.Message);
                return RedirectToAction("Error", "Home", new { Error = ex.Message });
            }
        }

        // GET: Simple/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var result = await _dataService.GetItem(id);
                if(result == null)
                    return RedirectToAction("Error", "Home", new { Error = "Пользователь не найден" });

                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home", new { Error = ex.Message });
            }
            
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

                return RedirectToAction(nameof(IndexAsync));
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

                return RedirectToAction(nameof(IndexAsync));
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

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}