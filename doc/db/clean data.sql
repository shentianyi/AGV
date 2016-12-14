delete from StockTask;
delete from StockMovement;
delete from Storage;
delete from TrayItem;
delete from DeliveryTray;
delete from Tray;
delete from DeliveryItem;
delete from Delivery;
delete from UniqueItem;




delete from Position;
  



  delete from StockTask;
  delete from [AgvWarehoueDb].[dbo].[Storage];
  update [AgvWarehoueDb].[dbo].UniqueItem set State=100;
  
  
  delete from StockTask where [type]=2;
  
  
  update StockTask set State=7 where [type]=1;