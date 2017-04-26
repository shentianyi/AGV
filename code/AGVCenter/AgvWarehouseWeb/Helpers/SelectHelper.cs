using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AgvWarehouseWeb.Helpers
{
    public static class SelectHelper
    {
        public static MvcHtmlString StockMovementTypeDropdownList(this HtmlHelper htmlHelper,
         int? type,
         string id = "StockMovementType")
        {
            var item = EnumUtil.GetList(typeof(StockMovementType));

            var list = string.Empty;
            List<SelectListItem> select = new List<SelectListItem>();

            select.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var it in item)
            {
                if (type.HasValue && it.Value == type.Value.ToString())
                {
                    select.Add(new SelectListItem { Text = it.Text, Value = it.Value.ToString(), Selected = true });
                }
                else
                {
                    select.Add(new SelectListItem { Text = it.Text, Value = it.Value.ToString(), Selected = false });
                }
            }

            list += htmlHelper.DropDownList(id, select).ToHtmlString();

            return new MvcHtmlString(list);
        }
    }
}