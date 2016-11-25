using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Services.Implement;
using AgvLibrary.Model;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data;


namespace TestCon
{
    class Program
    {
        static void Main(string[] args)
        {
            PartSearchModel p = new PartSearchModel();
            IPartServices q = new PartServices("Data Source = 42.121.111.38; Initial Catalog = BlueCarGps; Persist Security Info = True; User ID = bluecargps; Password = bluecargps; Connect Timeout = 150;");
            string a =q.SearchByNr("PN001").ToString();
        }
    }
}
