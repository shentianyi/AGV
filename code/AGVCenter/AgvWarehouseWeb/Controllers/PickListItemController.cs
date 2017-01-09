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
    public class PickListItemController : Controller
    {
        // GET: PickListItem
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            PickListItemService ps = new PickListItemService(Settings.Default.db);
            var q = new PickListItemSearchModel();
            IPagedList<PickListItemStorageView> items =
                ps.SearchDetail(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View(items);
        }


        public ActionResult Search([Bind(Include = "Nr,KNr,PositionNr,PickListNr,PickListNrAct,CreatedAtStart,CreatedAtEnd")]  PickListItemSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            PickListItemService ps = new PickListItemService(Settings.Default.db);
     
            IPagedList<PickListItemStorageView> items =
                ps.SearchDetail(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);


            ViewBag.Query = q;


            return View("Index", items);
        }

        // GET: PickListItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PickListItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PickListItem/Create
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

        // GET: PickListItem/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PickListItem/Edit/5
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

        // GET: PickListItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PickListItem/Delete/5
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
