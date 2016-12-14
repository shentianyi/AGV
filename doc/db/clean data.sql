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