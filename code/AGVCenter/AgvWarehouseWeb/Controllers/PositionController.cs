using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.SearchModel;
using AGVCenterLib.Service;
using AgvWarehouseWeb.Helpers;
using AgvWarehouseWeb.Properties;
using MvcPaging;
using System.IO;
using AgvWarehouseWeb.Models;
using CsvHelper.Configuration;
using System.Text;
using CsvHelper;

namespace AgvWarehouseWeb.Controllers
{
    public class PositionController : Controller
    {
        // GET: Position
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            PositionService ps = new PositionService(Settings.Default.db);
            var q = new PositionSearchModel();

            IPagedList<Position> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;
            return View(items);
        }


        public ActionResult Search([Bind(Include = "Nr,NrAct,IsLocked")]  PositionSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            PositionService ps = new PositionService(Settings.Default.db);

            IPagedList<Position> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", items);
        }

        // GET: Position/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Position/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Position/Create
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

        public void Export([Bind(Include = "Nr,InStorePriority")]  PositionSearchModel q)
        {

            PositionService ps = new PositionService(Settings.Default.db);

            List<Position> items =
                ps.Search(q)
              .ToList();

            ViewBag.Query = q;


            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                List<string> head = new List<string> { "PositionNr", "InStorePriority" };
                sw.WriteLine(string.Join(",", head));

                for (var i = 0; i < items.Count; i++)
                {
                    List<string> ii = new List<string>();
                    // ii.Add((i + 1).ToString());
                    ii.Add(items[i].Nr);
                    ii.Add(items[i].InStorePriority.ToString());
                    sw.WriteLine(string.Join(",", ii.ToArray()));
                }
                //sw.WriteLine(max);
            }
            var filename = "PositionPriority_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            var contenttype = "text/csv";
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = contenttype;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        //public ActionResult Row()
        //{
        //    return View();
        //}


        public ActionResult SearchByRow()
        {
            return View();
        }

        //[HttpGet]
        //public JsonResult AjaxSearchByRow(string Row)
        //{
        //    PositionService ps = new PositionService(Settings.Default.db);
        //    StorageService ss = new StorageService(Settings.Default.db);
        //    var q = new PositionSearchModel();
        //    var t = new StorageSearchModel();
        //    var qt = new PositionStorageSearchModel();

        //    List<PositionStorageSearchModel> realback = new List<PositionStorageSearchModel>();
        //    if (string.IsNullOrEmpty(Row))
        //    {
        //        return Json(realback, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        q.Nr = Row;
        //    }

        //    IQueryable<Position> items = ps.Search(q);

        //    foreach (var item in items)
        //    {
        //        qt = new PositionStorageSearchModel();
        //        t.PositionNr = item.Nr;
        //        var tempsst = ss.SearchDetail(t);

        //        string partNr = (tempsst.Count() != 0) ? tempsst.FirstOrDefault().PartNr : null;

        //        qt.Column = item.Column;
        //        qt.Row = item.Row;
        //        qt.Floor = item.Floor;
        //        qt.Nr = item.Nr;
        //        qt.IsLocked = item.isLocked;
        //        qt.InStorePriority = item.InStorePriority;
        //        qt.NrAct = item.Nr;
        //        qt.PartNrAct = partNr;
        //        realback.Add(qt);

        //    }
        //    ViewBag.Query = qt;

        //    return Json(realback.OrderBy(c => c.Floor), JsonRequestBehavior.AllowGet);
        //}


        public ActionResult ImportPriority()
        {
            return View();
        }

        //示例CSV文件在AGV/doc/PositionImportSetPriority.csv
        [HttpPost]
        public ActionResult DoImportPriority(HttpPostedFileBase partFile)
        {

            partFile = Request.Files[0];

            if (partFile == null)
            {
                //TODO: Parts 上传， 如果没有路径，在此处进行友好处理
                throw new Exception("No file is uploaded to system");
            }

            var appData = Server.MapPath("~/TmpFile/");
            var filename = Path.Combine(appData,
                DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(partFile.FileName));

            partFile.SaveAs(filename);
            string ex = Path.GetExtension(filename);

            List<PositionImportModel> records = new List<PositionImportModel>();

            ResultMessage rmsg = new ResultMessage();
            if (ex.Equals(".csv"))
            {
                CsvConfiguration configuration = new CsvConfiguration();
                configuration.Delimiter = ",";
                configuration.HasHeaderRecord = true;
                configuration.SkipEmptyRecords = true;
                configuration.RegisterClassMap<PositionImportModelCsvMap>();
                configuration.TrimHeaders = true;
                configuration.TrimFields = true;

                try
                {
                    using (TextReader treader = System.IO.File.OpenText(filename))
                    {
                        CsvReader csvReader = new CsvReader(treader, configuration);
                        records = csvReader.GetRecords<PositionImportModel>().ToList();
                    }
                }
                catch (Exception e)
                {
                    //ViewBag.TextExpMsg = "<-------------Read Csv File Exception!,Please Check.------------->" + e;
                    // ViewBag.TextExpMsg = "<-------------读取CSV文件异常，请查看原因：------------->" + e;
                    rmsg.Content = "<-------------读取CSV文件异常，请查看原因：------------->," + e.Message;
                }


                try
                {
                    if (records.Count > 0)
                    {

                        foreach (var record in records)
                        {
                            PositionService ps = new PositionService(Settings.Default.db);
                            ps.SetPriority(new string[] { record.PositionNr }, record.InStorePriority, null);

                        }
                        rmsg.Success = true;
                        rmsg.Content = "导入成功";
                    }
                    else
                    {
                        //ViewBag.NotCheckedData = "No Data Checked. Please Check Delimiter or Column Name.";
                        rmsg.Content = "没有检测到数据。请检查分隔符和列名。";
                    }
                }
                catch (Exception exx)
                {
                    rmsg.Content = "导入失败，" + exx.Message;
                }
            }
            else
            {
                //ViewBag.NotCsv = "Your File is not .Csv File, Please Check FileName.";
                rmsg.Content = "你上传的文件不是.CSV格式。请检查文件名。";
            }



            return Json(rmsg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LockUnLockPosition(string positionNr)
        {
            ResultMessage msg = new ResultMessage();
            try
            {
                // TODO: Add insert logic here
                new PositionService(Settings.Default.db).LockUnlockPosotion(positionNr);
                msg.Success = true;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                msg.Content = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Position/Edit/5
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

        // GET: Position/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Position/Delete/5
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
