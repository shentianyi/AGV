using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGVCenterLib.Data;
using AGVCenterLib.Model.SearchModel;
using AGVCenterLib.Service;
using AgvWarehouseWeb.Helpers;
using AgvWarehouseWeb.Properties;
using MvcPaging;

namespace AgvWarehouseWeb.Controllers
{
    public class StockTaskController : Controller
    {
        // GET: StockTask
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);
            StockTaskService ps = new StockTaskService(Settings.Default.db);
            var q = new StockTaskSearchModel();

            IPagedList<StockTask> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;
            return View(items);
        }


        public ActionResult Search([Bind(Include = "UniqItemNr,PositionNr,CreatedAtStart,CreatedAtEnd")]  StockTaskSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            StockTaskService ps = new StockTaskService(Settings.Default.db);

            IPagedList<StockTask> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", items);
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
