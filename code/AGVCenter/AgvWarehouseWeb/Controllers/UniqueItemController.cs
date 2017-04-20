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
using AGVCenterLib.Enum;

namespace AgvWarehouseWeb.Controllers
{
    public class UniqueItemController : Controller
    {
        // GET: UniqueItem
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            UniqueItemService ps = new UniqueItemService(Settings.Default.db);
            var q = new UniqueItemSearchModel();

            IPagedList<UniqueItem> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View(items);
        }

        public ActionResult Search([Bind(Include = "Nr,KNr,PartNrAct,BoxTypeId,CreatedAtStart,CreatedAtEnd")]  UniqueItemSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            UniqueItemService ps = new UniqueItemService(Settings.Default.db);

            IPagedList<UniqueItem> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", items);
        }

        // GET: UniqueItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UniqueItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UniqueItem/Create
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

        // GET: UniqueItem/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UniqueItem/Edit/5
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

        // GET: UniqueItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UniqueItem/Delete/5
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

        [HttpGet]
        public ActionResult MisOutStocked(string id)
        {
            var item = new UniqueItemService(Settings.Default.db).FindByNr(id);
            return View(item);
        }


        [HttpPost]
        public ActionResult MisOutStocked(string id, FormCollection collection)
        {
            UniqueItem item = null;//new UniqueItemService(Settings.Default.db).FindByNr(id);
            var msg = "";
            try
            {
                new UniqueItemService(Settings.Default.db).UpdateUniqItemState(id, UniqueItemState.MisOutStocked);
                item =  new UniqueItemService(Settings.Default.db).FindByNr(id);
                msg = "【设置成功】，此成品可重新入库操作！";
            }
            catch (Exception ex)
            {
                msg = "【设置失败】" + ex.Message;
            }
            ViewBag.message = msg;
            if (item != null)
            {
                return View(item);
            }
            else
            {
                return RedirectToAction("Index");

            }
        }
    }
}
