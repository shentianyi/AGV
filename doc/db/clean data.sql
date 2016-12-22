delete from StockTask;
delete from StockMovement;
delete from Storage;
delete from TrayItem;
delete from DeliveryTray;
delete from Tray;
delete from DeliveryItem;
delete from Delivery;
delete from UniqueItem   where createdat <'2016-12-21 09:00';

delete from Position;



  update [AgvWarehouseDb].[dbo].[UniqueItem] set State=100;
  
/**
  delete from StockTask;
 delete from [AgvWarehouseDb].[dbo].[Storage];
  update [AgvWarehouseDb].[dbo].UniqueItem set State=100;
  
  **/
  
 -- delete from StockTask where [type]=2;
  
  
 -- update StockTask set State=7 where [type]=1;