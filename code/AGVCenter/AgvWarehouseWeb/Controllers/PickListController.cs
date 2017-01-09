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
    public class PickListController : Controller
    {
        // GET: PickList
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            PickListService ps = new PickListService(Settings.Default.db);
            var q = new PickListSearchModel();
            IPagedList<PickList> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;
            return View(items);
        }


        public ActionResult Search([Bind(Include = "Nr,CreatedAtStart,CreatedAtEnd")]  PickListSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            PickListService ps = new PickListService(Settings.Default.db);

            IPagedList<PickList> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", items);
        }

        // GET: PickList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PickList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PickList/Create
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

        // GET: PickList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PickList/Edit/5
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

        // GET: PickList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PickList/Delete/5
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
