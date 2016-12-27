using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgvWarehouseWeb.Controllers
{
    public class StockTaskController : Controller
    {
        // GET: StockTask
        public ActionResult Index()
        {
            return View();
        }

        // GET: StockTask/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StockTask/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockTask/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StockTask/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StockTask/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StockTask/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StockTask/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
