using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data.Repository.Implement
{
  public  class Incoming : RepositoryBase<Movement>,IIncoming
    {
        private AgvWareHouseDataContext context;

        public Incoming(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }


        /// <summary>
        /// 基础表中唯一吗是否存在
        /// </summary>
        /// <param name="UniqNr"></param>
        /// <returns></returns>

        public UniqueItem SearchByUniqNr(string UniqNr)
        {
            return this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.UniqNr.Equals(UniqNr));
        }
        public bool UnqiNrExist(string UniqNr)
        {
            return SearchByUniqNr(UniqNr) != null;
        }



        /// <summary>
        /// 在Position表中查看对应位置是否存在
        /// </summary>
        /// <param name="PositionNr"></param>
        /// <returns></returns>

        public Position SearchByPositionNr(string PositionNr)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.PositionNr.Equals(PositionNr));
        }
        public bool PositionNrExist(string PositionNr)
        {
            return SearchByPositionNr(PositionNr) != null;
        }


        /// <summary>
        /// 唯一吗是否在Storage表
        /// </summary>
        /// <param name="UniqueItemNr"></param>
        /// <returns></returns>
        public Storage SearchByUniqueNr(string UniqueItemNr)
        {
            return this.context.GetTable<Storage>().FirstOrDefault(c => c.UniqItemNr.Equals(UniqueItemNr));
        }

        public bool UniqueItemNrExist(string UniqueItemNr)
        {
            return SearchByUniqueNr(UniqueItemNr) != null;
        }


        /// <summary>
        /// 搜索仓库基础表
        /// </summary>
        /// <param name="WHNr"></param>
        /// <returns></returns>
        public Warehouse SearchByWHNr(string WHNr)
        {
            return this.context.GetTable<Warehouse>().FirstOrDefault(c => c.WHNr.Equals(WHNr));
        }
        public bool WHNrExist(string WHNr)
        {
            return SearchByWHNr(WHNr) != null;
        }

        /// <summary>
        /// 根据仓库层排列查询位置 判断是否存在
        /// </summary>
        /// <param name="WHNr"></param>
        /// <param name="Floor"></param>
        /// <param name="Column"></param>
        /// <param name="Row"></param>
        /// <returns></returns>
        public Position SearchByPosition(string WHNr, int Floor, int Column, int Row)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.WHNr.Equals(WHNr) && c.Floor.Equals(Floor) && c.Column.Equals(Column) && c.Row.Equals(Row));
        }




        public bool PositionExist(string WHNr, int Floor, int Column, int Row)
        {
            return SearchByPosition(WHNr, Floor, Column, Row) != null;
        }



        /// <summary>
        /// 创建Storage
        /// </summary>
        /// <param name="sg"></param>
        /// <returns></returns>
        public bool Create(Storage sg)
        {
            try
            {
                this.context.GetTable<Storage>().InsertOnSubmit(sg);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


       
       

        


       


       

       

       
    }
}
