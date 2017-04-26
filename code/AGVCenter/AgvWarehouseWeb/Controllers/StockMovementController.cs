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
using System.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Brilliantech.Framwork.Utils.EnumUtil;
using AGVCenterLib.Enum;

namespace AgvWarehouseWeb.Controllers
{
    public class StockMovementController : Controller
    {
        // GET: Storage
        public ActionResult Index(int? page)
        {
            int pageIndex = PagingHelper.GetPageIndex(page);

            StockMovementService ps = new StockMovementService(Settings.Default.db);
            var q = new StockMovementSearchModel();
            IPagedList<StockMovement> items =
                ps.Search(q)
                .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View(items);
        }

        public ActionResult Search([Bind(Include = "UniqItemNr,PositionNr,CreatedAtStart,CreatedAtEnd,StockMovementType")]  StockMovementSearchModel q)
        {
            int pageIndex = 0;
            int.TryParse(Request.QueryString.Get("page"), out pageIndex);
            pageIndex = PagingHelper.GetPageIndex(pageIndex);

            StockMovementService ps = new StockMovementService(Settings.Default.db);
            IPagedList<StockMovement> items =
               ps.Search(q)
               .ToPagedList(pageIndex, Settings.Default.pageSize);

            ViewBag.Query = q;

            return View("Index", items);
        }


        public void Export([Bind(Include = "UniqItemNr,PositionNr,CreatedAtStart,CreatedAtEnd,StockMovementType")]  StockMovementSearchModel q)
        {
            StockMovementService ps = new StockMovementService(Settings.Default.db);
            List<StockMovement> items =
               ps.Search(q).ToList();

            ViewBag.Query = q;

            #region CSV
            //MemoryStream ms = new MemoryStream();
            //using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            //{
            //    List<string> head = new List<string> { "KSK号", "来源库位", "目的库位", "记录类型","操作时间" };
            //    sw.WriteLine(string.Join(",", head));

            //    for (var i = 0; i < items.Count; i++)
            //    {
            //        List<string> ii = new List<string>();
            //        ii.Add(items[i].UniqItemNr);
            //        ii.Add(items[i].SourcePosition);
            //        ii.Add(items[i].AimedPosition);
            //        ii.Add(items[i].TypeStr);
            //        ii.Add(items[i].CreatedAt.ToString());
            //        sw.WriteLine(string.Join(",", ii.ToArray()));
            //    }
            //}
            //var filename = "库存记录_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            //var contenttype = "text/csv";
            //Response.Clear();
            //Response.ContentEncoding = Encoding.UTF8;
            //Response.ContentType = contenttype;
            //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.BinaryWrite(ms.ToArray());
            //Response.End();
            #endregion

            #region EXCEL
            var appData = Server.MapPath("~/TmpFile/");
            var tmpFile = Path.Combine(appData,
                "库存记录报表_" +
                DateTime.Now.ToString("yyyyMMddHHmmss")+".xlsx");


            FileInfo fileInfo = new FileInfo(tmpFile);

            using (ExcelPackage ep = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet totalSheet = ep.Workbook.Worksheets.Add("统计");
                if (items.Count > 0)
                {
                    // 写入统计
                  //  ExcelWorksheet totalSheet = ep.Workbook.Worksheets.Add("统计");
                    // 写入title
                    string startDate = items.OrderBy(s => s.CreatedAt).FirstOrDefault().CreatedAtStr;
                    string endDate = items.OrderBy(s => s.CreatedAt).LastOrDefault().CreatedAtStr;
                    var range = totalSheet.Cells["A1:B1"];
                    range.Merge = true;

                    totalSheet.Cells[1, 1].Value = string.Format("{0}~{1}的报表", startDate, endDate);

                    totalSheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    totalSheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    totalSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var types = EnumUtil.GetList(typeof(StockMovementType));
                    int typeCount = 1;
                    foreach (var type in types)
                    {
                        totalSheet.Cells[1 + typeCount, 1].Value = type.Text;
                        totalSheet.Cells[1 + typeCount, 2].Value = items.Count(s => s.Type == int.Parse(type.Value));
                        typeCount += 1;
                    }

                    totalSheet.Cells[1 + typeCount, 1].Value = "全部";
                    totalSheet.Cells[1 + typeCount, 2].Value = items.Count;


                    // 写入详细 
                    Dictionary<string, List<StockMovement>> detailItemsDic = new Dictionary<string, List<StockMovement>>();

                    detailItemsDic.Add("【入库】记录详细", items.Where(s => s.Type == (int)StockMovementType.In).ToList());
                    detailItemsDic.Add("【出库】记录详细", items.Where(s => s.Type == (int)StockMovementType.Out).ToList());
                    detailItemsDic.Add("【移库】记录详细", items.Where(s => s.Type == (int)StockMovementType.Move).ToList());
                    detailItemsDic.Add("记录详细", items);

                    foreach (var d in detailItemsDic)
                    {
                        ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(d.Key);
                        sheet.Cells[1, 1].Value = "KSK号";
                        sheet.Cells[1, 2].Value = "来源库位";
                        sheet.Cells[1, 3].Value = "目的库位";
                        sheet.Cells[1, 4].Value = "记录类型";
                        sheet.Cells[1, 5].Value = "操作时间";
                        sheet.Cells["A1:E1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        sheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        sheet.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        

                        int i = 1;
                        foreach (var item in d.Value)
                        {
                            sheet.Cells[1 + i, 1].Value = item.UniqItemNr;
                            sheet.Cells[1 + i, 2].Value = item.SourcePosition;
                            sheet.Cells[1 + i, 3].Value = item.AimedPosition;
                            sheet.Cells[1 + i, 4].Value = item.TypeStr;
                            sheet.Cells[1 + i, 5].Value = item.CreatedAtStr;
                            i += 1;
                        }
                    }

                }
                ep.Save();
            }



             
            FileStream fs = new FileStream(tmpFile, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/vnd.ms-excel";


            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(Path.GetFileName( tmpFile)));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

            #endregion
        }
        // GET: StockMovement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StockMovement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockMovement/Create
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

        // GET: StockMovement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StockMovement/Edit/5
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

        // GET: StockMovement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StockMovement/Delete/5
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
