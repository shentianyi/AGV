using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class TrayRepository:RepositoryBase<Tray>, ITrayRepository
    {
        private AgvWarehouseDataContext context;


        public TrayRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(Tray entity)
        {
            this.context.Tray.InsertOnSubmit(entity);
        }

        public Tray FindByNr(string nr)
        {
            return this.context.Tray.FirstOrDefault(s => s.Nr==nr);
        }
    }
}
