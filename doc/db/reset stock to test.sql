
use AgvWarehoueDb
go

delete from DeliveryItem;
delete from Delivery;

  delete from StockTask;
  delete from [AgvWarehoueDb].[dbo].[Storage];
  update [AgvWarehoueDb].[dbo].UniqueItem set State=100;
  
  update [AgvWarehoueDb].[dbo].Delivery set State=100;
  update [AgvWarehoueDb].[dbo].DeliveryItem set State=100;
  
  update [UniqueItem] set BoxTypeId=1;
  
   
  
  
  delete from StockTask where [type]=2;
  
  
  update StockTask set State=7 where [type]=1;