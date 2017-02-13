using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public void Export([Bind(Include = "Nr,KNr,PartNrAct,BoxTypeId,RoadMachineIndex,PositionNr,FIFOStart,FIFOEnd,CreatedAtStart,CreatedAtEnd")]  StorageSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            StorageService ps = new StorageService(Settings.Default.db);

            List<StorageUniqueItemView> items =
                ps.SearchDetail(q)
              .ToList();

            ViewBag.Query = q;


            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                List<string> head = new List<string> { "配置号", "大众K号", "莱尼内部K号", "验证码", "库位号", "FIFO", "箱型", "巷道机号" };
                sw.WriteLine(string.Join(",", head));

                for (var i = 0; i < items.Count; i++)
                {
                    List<string> ii = new List<string>();
                   // ii.Add((i + 1).ToString());
                    ii.Add(items[i].UniqueItemPartNr);
                    ii.Add(items[i].UniqueItemKNr);
                    ii.Add(items[i].UniqueItemKskNr);
                    ii.Add(items[i].UniqueItemCheckCode);
                    ii.Add(items[i].PositionNr);
                    ii.Add(items[i].FIFOStr);
                    ii.Add(items[i].BoxTypeStr);
                    ii.Add(items[i].PositionRoadMachineIndex.ToString());
                    sw.WriteLine(string.Join(",", ii.ToArray()));
                }
                //sw.WriteLine(max);
            }
            var filename = "Storage_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            var contenttype = "text/csv";
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = contenttype;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(ms.ToArray());
            Response.End();
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
