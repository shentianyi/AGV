using AgvLibrary.Data;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Implement;
namespace AgvLibrary.Services.Implement

{
    public class PartServices :ServiceBase,IPartServices
    {

        private IPartRepository part;

        public PartServices(string dbString) : base(dbString) {
            part = new PartRepository(this.Context);
        }


        public List<Part> All()
        {

            return this.Context.Context.GetTable<Part>().ToList();
        }

        public IQueryable<Data.Part> Search(PartSearchModel partSearchModel)
        {
            return part.Search(partSearchModel);

        }
    }
}
