using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Brilliantech.Framwork.Models;

namespace Brilliantech.Framwork.Utils.EnumUtil
{
   public class EnumUtil
    {
        public static string GetDescription(Enum enumValue)
        {
            string description = string.Empty;
            FieldInfo info = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] attributes = null;
            try
            {
                attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            }
            catch { }
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();
        }

        public static string GetDescriptionByFiledName(string name, Type type)
        {
            string desc = string.Empty;
            var values = Enum.GetValues(type);
            foreach (Enum v in values)
            {
                if (v.ToString().Equals(name))
                {
                    desc = GetDescription(v);
                    break;
                }
            }
            return desc;
        }

        public static Array GetAllValue(Type type)
        {
            return Enum.GetValues(type);
        }


        public static List<EnumItem> GetList(Type type)
        {
            List<EnumItem> arraylist = new List<EnumItem>();

            var values = Enum.GetValues(type);

            foreach (Enum v in values)
            {
                EnumItem item = new EnumItem();
                item.Text = GetDescription(v);
                item.Value = Convert.ToInt32(v).ToString();
                arraylist.Add(item);
            }

            return arraylist;
        }

    }
}
