using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Service
{
    public class PositionService : ServiceBase
    {

        public PositionService(string dbString) : base(dbString)
        {

        }


        /// <summary>
        /// 是否有可用库位,目前写死了巷道机1和2的判断
        /// </summary>
        /// <param name="dispatchedPositions">已分配的库位</param>
        /// <returns></returns>
        public bool HasAvaliablePosition(List<string> dispatchedPositions)
        {
            bool r1 = this.FindInStockPosition(1, dispatchedPositions) != null;
            bool r2 = this.FindInStockPosition(2, dispatchedPositions) != null;

            return r1 && r2;
        }

        /// <summary>
        /// 查询可用的位置，可以传递前一个库位做动态平衡
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <param name="dispatchedPositions"></param>
        /// <param name="lockPosition">是否锁定返回的库位，默认为false</param>
        /// <returns></returns>
        public Position FindInStockPosition(int roadMachineIndex,
            List<string> dispatchedPositions = null,
            bool lockPosition = false)
        {
            if (dispatchedPositions == null)
            {
                dispatchedPositions = new List<string>();
            }
            IPositionRepository posiRep = new PositionRepository(this.Context);
            return posiRep.FindByRoadMachineAndSort(roadMachineIndex, dispatchedPositions, lockPosition);
        }

        /// <summary>
        /// 快速创建仓库库位，nr将以 P-roadIndex-row-column-floor 的格式
        /// </summary>
        /// <param name="warehouseNr"></param>
        /// <param name="roadMachineIndex"></param>
        /// <param name="rowNum"></param>
        /// <param name="columnNum"></param>
        /// <param name="floorNum"></param>
        public void FastCreatePositions(string warehouseNr,
            int roadMachineIndex, 
            int rowNum,
            int columnNum ,
            int floorNum, int startColumn=1)
        {
            List<Position> positions = new List<Position>();
            for(int i = 1; i <= rowNum; i++)
            {
                for(int j = startColumn; j <= columnNum; j++)
                {
                    for(int k = 1; k <= floorNum; k++)
                    {
                        positions.Add(new Position()
                        {
                            Nr = string.Format("P {0} {1} {2} {3}",roadMachineIndex.ToString().PadLeft(2,'0'), i.ToString().PadLeft(2, '0'), j.ToString().PadLeft(2, '0'), k.ToString().PadLeft(2, '0')),
                            WarehouseNr=warehouseNr,
                            Row=i,
                            Column=j,
                            Floor=k,
                            RoadMachineIndex=roadMachineIndex,
                        });
                    }
                }
            }
            IPositionRepository posiRep = new PositionRepository(this.Context);
            posiRep.Creates(positions);
            this.Context.SaveAll();
        }

        public IQueryable<Position> Search(PositionSearchModel searchModel)
        {
            return new PositionRepository(this.Context).Search(searchModel);
        }


        public void CreatePosition( Position position)
        {
            IPositionRepository posiRep = new PositionRepository(this.Context);
            posiRep.Create(position);
            this.Context.SaveAll();
        }

        public void LockUnlockPosotion(string positionNr)
        {
            Position p = new PositionRepository(this.Context).FindByNr(positionNr);
            p.IsLocked = ! p.IsLocked;
            this.Context.SaveAll();
        }
    }


}
