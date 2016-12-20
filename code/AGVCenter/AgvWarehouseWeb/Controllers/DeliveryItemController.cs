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
    public class DeliveryItemController : Controller
    {
        // GET: DeliveryItem
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            DeliveryItemService ps = new DeliveryItemService(Settings.Default.db);
            var q = new DeliveryItemSearchModel();
            IPagedList<DeliveryItemStorageView> items =
                ps.SearchDetail(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View(items);
        }


        public ActionResult Search([Bind(Include = "Nr,KNr,PositionNr,DeliveryNr,TrayNr")]  DeliveryItemSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            DeliveryItemService ps = new DeliveryItemService(Settings.Default.db);
             
            IPagedList<DeliveryItemStorageView> items =
                ps.SearchDetail(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;


            return View("Index", items);
        }

        // GET: DeliveryItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeliveryItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryItem/Create
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

        // GET: DeliveryItem/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeliveryItem/Edit/5
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

        // GET: DeliveryItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeliveryItem/Delete/5
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
