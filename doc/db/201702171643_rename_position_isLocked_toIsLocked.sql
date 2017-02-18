use AgvWarehouseDb
go

exec sp_RENAME  'dbo.Position.isLocked','IsLocked','COLUMN';

go


USE [AgvWarehouseDb]
GO

/****** Object:  View [dbo].[PositionStorageView]    Script Date: 02/17/2017 16:41:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[PositionStorageView]
AS
SELECT     dbo.Position.Nr, dbo.Position.WarehouseNr, dbo.Position.Floor, dbo.Position.[Column], dbo.Position.Row, dbo.Position.State, dbo.Position.RoadMachineIndex, 
                      dbo.Position.IsLocked, dbo.Storage.Id AS StorageId, dbo.Storage.PositionNr AS StoragePositionNr, dbo.Storage.PartNr AS StoragePartNr, 
                      dbo.Storage.FIFO AS StorageFIFO, dbo.Storage.UniqItemNr AS StorageUniqItemNr, dbo.Storage.CreatedAt AS StorageCreatedAt
FROM         dbo.Position LEFT OUTER JOIN
                      dbo.Storage ON dbo.Position.Nr = dbo.Storage.PositionNr

GO