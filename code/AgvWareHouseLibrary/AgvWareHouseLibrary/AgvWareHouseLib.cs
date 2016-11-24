using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvWareHouseLibrary
{
    public class AgvWareHouseLib
    {

        static string SearchPosationNr = string.Empty; 

        /// <summary>
        /// 
        /// </summary>
        /// <param 零件编号="PartNr"></param>
        /// <param 唯一码="ItemUniqueNr"></param>
        /// <returns></returns>
        public static string Offline(string PartNr, int ItemUniqueNr)
        {
            string part = PartNr;
            int partUniqueNr = ItemUniqueNr;
            string returnMean = string.Empty;
            AgvWareHouseDataContext ctx = new AgvWareHouseDataContext();
            if(string.IsNullOrEmpty(part))
            {
                returnMean = "零件编号有误";
            }
            else if(string.IsNullOrEmpty(partUniqueNr.ToString()))
            {
                returnMean = "零件唯一码有误";
            }
            else
            { 
            var PartQuery = from p in ctx.Part
                            where p.PartNr == part
                            select p;

                if (PartQuery.Count() ==0)//?

                {
                    returnMean = (part + "未在Part表中维护\n");

                }
                else
                {
                    var PartUniqueQuery = from PU in ctx.UniqueItem
                                          where PU.ItemUnique == partUniqueNr
                                          select PU;
                    if (PartUniqueQuery.Count() == 0)//?
                    {
                        string createTime = DateTime.Now.ToString();
                        UniqueItem array = new UniqueItem
                        {
                            PartNr = part,
                            ItemUnique = partUniqueNr,
                            CreateTime = createTime,
                            Status = "已下线"

                        };
                        ctx.UniqueItem.InsertOnSubmit(array);
                        ctx.SubmitChanges();
                        returnMean = ("下线成功");
                    }
                    else
                    {
                        returnMean = (partUniqueNr + "唯一码已经生成过\n");
                    }

                }
            }
           return returnMean;
        }


        /// <summary>
        /// 根绝零件唯一码和位置编号 入库
        /// </summary>
        /// <param name="ItemUnique"></param>
        /// <param name="PosationNr"></param>
        /// <returns></returns>
        public static string Incoming(int ItemUnique,string PosationNr)
        {
            string mean = "";
            string posationNr = PosationNr;
            int itemUnique = ItemUnique;

            AgvWareHouseDataContext dtx = new AgvWareHouseDataContext("server=localhost;database=WareHouseTest;uid=sa;pwd=123456@");
           
            //判断位置编码是否存在Posation基础表中
            var PosationNrQuery = from PN in dtx.Posation
                                  where PN.PosationNr == posationNr
                                  select PN;

            if (PosationNrQuery.Count() == 0)
            {
                mean = "位置编码" + posationNr + "有误";
            }

            //判断零件唯一码是否存在于Item基础表中
            var ItemQuery = from IQ in dtx.UniqueItem
                            where IQ.ItemUnique == itemUnique
                            select IQ;
            if (ItemQuery.Count() == 0)
            {
                mean = "唯一码" + itemUnique + "有误";
            }


            //判断库位是否被占用
            var SearchQuery = from SQ in dtx.Storage
                              where SQ.PosationNr == posationNr
                              select SQ.ItemNr;
            if(SearchQuery.Count()>=0)
            {
                mean = "库位：" + posationNr + "已被占用";
            }
            else
            {
                Storage sa = new Storage()
                {
                    PosationNr = posationNr,
                    ItemNr = itemUnique,
                    FIFO = DateTime.Now.ToString()
                };
                dtx.Storage.InsertOnSubmit(sa);
                dtx.SubmitChanges();

            }
            
           
            return mean;
        }



        /// <summary>
        /// 根据零件唯一码和仓库层排列 入库
        /// </summary>
        /// <param name="ItemUnique"></param>
        /// <param name="WHNr"></param>
        /// <param name="Floor"></param>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        public static string Incoming(int ItemUnique, string WHNr, int Floor, int Row, int Column)
        {
           
            string mean = string.Empty;
            int  itemUnique = ItemUnique;
            if (string.IsNullOrEmpty(ItemUnique.ToString()))

            {
                mean = "零件唯一码有误";
            }
            if (string.IsNullOrEmpty(WHNr) || string.IsNullOrEmpty(Floor.ToString()) || string.IsNullOrEmpty(Row.ToString()) || string.IsNullOrEmpty(Column.ToString()))
            {
                mean = "库位信息有误";

            }
            else
            {

                AgvWareHouseDataContext dtx = new AgvWareHouseDataContext("server=localhost;database=WareHouseTest;uid=sa;pwd=123456@");//?
                //查询唯一码是否在基础数据表中存在
                var ItemQuery = from IQ in dtx.UniqueItem
                                where IQ.ItemUnique == itemUnique
                                select IQ;
                if (ItemQuery.Count() == 0)
                {
                    mean = "唯一码" + itemUnique + "不存在";
                }
                //查询库位否存在
                var PosationQuery = from PN in dtx.Posation
                                    where PN.WHNr == WHNr && PN.Floor == Floor && PN.Row == Row && PN.Coloumn == Column
                                    select PN.PosationNr;

                if (PosationQuery.Count() >=0)
                {
                    mean = "库位:" + WHNr + "仓库" + Floor + "层" + Row + "排" + Column + "列不存在";
                }
                foreach (string Posation in PosationQuery)
                {
                    SearchPosationNr = Posation;
                }
                //查询库位是否被占用
                var PosationNrQuery = from PNQ in dtx.Storage
                                      where PNQ.PosationNr == SearchPosationNr
                                      select PNQ.ItemNr;
                if (PosationNrQuery.Count()>=0)
                {
                    mean = "库位:" + WHNr + "仓库" + Floor + "层" + Row + "排" + Column + "列"+"已被"+SearchPosationNr+"占用";
                }
                
                else
                {
                    Storage sa = new Storage()
                    {
                        PosationNr =SearchPosationNr,
                        ItemNr = itemUnique,
                        FIFO = DateTime.Now.ToString()
                    };
                    dtx.Storage.InsertOnSubmit(sa);
                    dtx.SubmitChanges();
                    mean = "入库成功";
                    //更改Item表中对应零件的状态
                    var ItemNrQuery = from INQ in dtx.UniqueItem
                                      where INQ.ItemUnique == itemUnique
                                      select INQ;
                    foreach (var p in ItemNrQuery)
                    {
                        p.Status = "已出库";
                    }

                    dtx.SubmitChanges();
                }
            }
            return mean;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemUnique"></param>
        /// <returns></returns>
        public static string SearchPosation(int ItemUnique)
        {
            int itemUnique = ItemUnique;
            string PosationNr = string.Empty;
            if(string.IsNullOrEmpty(itemUnique.ToString()))
            {
                PosationNr = string.Empty;
            }
            AgvWareHouseDataContext stx = new AgvWareHouseDataContext("server=localhost;database=WareHouseTest;uid=sa;pwd=123456@");

            var SearchPosationNrQuery = from SPQ in stx.Storage
                                  where SPQ.ItemNr == itemUnique
                                  select SPQ.PosationNr;
            if (SearchPosationNrQuery.Count() == 0)
            {
                PosationNr = string.Empty;
            }
            else
            {
                foreach (string posationNr in SearchPosationNrQuery)
                {
                  PosationNr = posationNr;
                }
            }
            return PosationNr;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemUnique"></param>
        /// <returns></returns>
        public static string Outcoming(int ItemUnique)
        {
            int itemUnique = ItemUnique;
            string mean = string.Empty;
            string PosationNr = string.Empty;
            if(string.IsNullOrEmpty(ItemUnique.ToString()))
            {
                mean = "零件唯一码有误";
            }

            AgvWareHouseDataContext otx = new AgvWareHouseDataContext("server=localhost;database=WareHouseTest;uid=sa;pwd=123456@");

            string partPosation = SearchPosation(itemUnique);

            //查询零件所在位置
            var PosationNrQuery = from PQ in otx.Storage
                                  where PQ.ItemNr == itemUnique && PQ.PosationNr==partPosation
                                  select PQ;
            
            if (PosationNrQuery.Count() == 0)
                {
                mean = "无法找到零件：" + itemUnique + "所在位置";
               
                }
                else
                {
                    foreach (var posationNr in PosationNrQuery)
                    {
                    otx.Storage.DeleteOnSubmit(posationNr);
                    }
                otx.SubmitChanges();
                mean = "出库成功";

                //更改Item表中对应零件的状态
                var ItemNrQuery = from INQ in otx.UniqueItem
                                  where INQ.ItemUnique == itemUnique
                                  select INQ;
                foreach(var p in ItemNrQuery)
                {
                    p.Status = "已出库";
                }

                otx.SubmitChanges();
               
            }
            return mean;
        }
           
          
            

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DealNr"></param>
        /// <returns></returns>
        public static int[] SearchItemOnDeal (string DealNr)
        {
            AgvWareHouseDataContext stx = new AgvWareHouseDataContext("server=localhost;database=WareHouseTest;uid=sa;pwd=123456@");
            int[] itemUniques = new int[1024];
            if(!string.IsNullOrEmpty(DealNr))
            {
                var ItemUniquesQuery = from IQS in stx.ItemOnDeal
                                       where IQS.DealNr == DealNr
                                       select IQS.ItemNr;
                itemUniques = ItemUniquesQuery.ToArray();
            }

            return itemUniques;
        }

        public static string Dlivery(string DealNr)
        {
            string dealNr = DealNr;
            string mean = string.Empty;
            
            AgvWareHouseDataContext stx = new AgvWareHouseDataContext("server=localhost;database=WareHouseTest;uid=sa;pwd=123456@");
            int[] itemUniques = new int[1024];
            itemUniques = SearchItemOnDeal(dealNr);
            for(int i=0;i<=itemUniques.Length-1;i++)
            {
                //更改Item表中对应零件的状态
                var ItemNrQuery = from INQ in stx.UniqueItem
                                  where INQ.ItemUnique == itemUniques[i]
                                  select INQ;
                foreach (var p in ItemNrQuery)
                {
                    p.Status = "已出库";
                }

                stx.SubmitChanges();
                mean = "发运完成";
            }
            return mean;

        }

   
        
    }
   
}
