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
    public class StorageController : Controller
    {
        // GET: Storage
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);
            
            StorageService ps = new StorageService(Settings.Default.db);
            var q = new StorageSearchModel();
            IPagedList<StorageUniqueItemView> items = 
                ps.SearchDetail(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q; 

            return View(items);
        }

        public ActionResult Search([Bind(Include = "Nr,KNr,PartNrAct,BoxTypeId,RoadMachineIndex,PositionNr,FIFOStart,FIFOEnd,CreatedAtStart,CreatedAtEnd")]  StorageSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            StorageService ps = new StorageService(Settings.Default.db);
             
            IPagedList<StorageUniqueItemView> items =
                ps.SearchDetail(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", items);
        }
        // GET: Storage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Storage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Storage/Create
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

        // GET: Storage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Storage/Edit/5
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

        // GET: Storage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Storage/Delete/5
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
