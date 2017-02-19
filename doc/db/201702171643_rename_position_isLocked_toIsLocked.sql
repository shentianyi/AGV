----- 创建表
USE [AgvWarehouseDb]
GO

/****** Object:  Table [dbo].[WarehouseArea]    Script Date: 02/19/2017 13:16:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[WarehouseArea](
	[Nr] [varchar](50) NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[Remark] [varchar](50) NULL,
	[Color] [varchar](50) NULL,
	[InStorePriority] [int] NOT NULL,
	[WarehouseNr] [varchar](50) NULL,
 CONSTRAINT [PK_WarehouseArea] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[WarehouseArea]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseArea_Warehouse] FOREIGN KEY([WarehouseNr])
REFERENCES [dbo].[Warehouse] ([Nr])
GO

ALTER TABLE [dbo].[WarehouseArea] CHECK CONSTRAINT [FK_WarehouseArea_Warehouse]
GO

ALTER TABLE [dbo].[WarehouseArea] ADD  CONSTRAINT [DF_WarehouseArea_isLocked]  DEFAULT ((0)) FOR [IsLocked]
GO

ALTER TABLE [dbo].[WarehouseArea] ADD  CONSTRAINT [DF_WarehouseArea_InStorePriority]  DEFAULT ((1)) FOR [InStorePriority]
GO




---- 修改列名称
use AgvWarehouseDb
go

exec sp_RENAME  'dbo.Position.isLocked','IsLocked','COLUMN';

go

-- 添加列名
use AgvWarehouseDb
go

alter table Position Add WarehouseAreaNr varchar(50);

go
-- 添加外键

USE [AgvWarehouseDb]
GO

ALTER TABLE [dbo].[Position]  WITH CHECK ADD  CONSTRAINT [FK_Position_WarehouseArea] FOREIGN KEY([WarehouseAreaNr])
REFERENCES [dbo].[WarehouseArea] ([Nr])
GO

ALTER TABLE [dbo].[Position] CHECK CONSTRAINT [FK_Position_WarehouseArea]
GO





--修改视图-----------------------------------------------------------------------------------------------------------

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
                      dbo.Position.IsLocked,dbo.Position.WarehouseAreaNr, dbo.Storage.Id AS StorageId, dbo.Storage.PositionNr AS StoragePositionNr, dbo.Storage.PartNr AS StoragePartNr, 
                      dbo.Storage.FIFO AS StorageFIFO, dbo.Storage.UniqItemNr AS StorageUniqItemNr, dbo.Storage.CreatedAt AS StorageCreatedAt
FROM         dbo.Position LEFT OUTER JOIN
                      dbo.Storage ON dbo.Position.Nr = dbo.Storage.PositionNr

GO

------------------------------------------------------------------------------------------------------------------------

USE [AgvWarehouseDb]
GO

/****** Object:  View [dbo].[StorageUniqueItemView]    Script Date: 02/19/2017 13:08:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[StorageUniqueItemView]
AS
SELECT     dbo.Storage.Id, dbo.Storage.PositionNr, dbo.Storage.PartNr, dbo.Storage.FIFO, dbo.Storage.UniqItemNr, dbo.Storage.CreatedAt, dbo.Storage.UpdatedAt, 
                      dbo.UniqueItem.Nr AS UniqueItemNr, dbo.UniqueItem.BoxTypeId AS UniqueItemBoxTypeId, dbo.UniqueItem.PartNr AS UniqueItemPartNr, 
                      dbo.UniqueItem.KNr AS UniqueItemKNr, dbo.UniqueItem.KNrWithYear AS UniqueItemKNrWithYear, dbo.UniqueItem.CheckCode AS UniqueItemCheckCode, 
                      dbo.UniqueItem.KskNr AS UniqueItemKskNr, dbo.UniqueItem.QR AS UniqueItemQR, dbo.UniqueItem.State AS UniqueItemState, 
                      dbo.UniqueItem.CreatedAt AS UniqueItemCreatedAt, dbo.UniqueItem.UpdatedAt AS UniqueItemUpdatedAt, dbo.Position.isLocked AS PositionIsLocked, 
                      dbo.Position.RoadMachineIndex AS PositionRoadMachineIndex, dbo.Position.State AS PositionState, dbo.Position.Row AS PositionRow, 
                      dbo.Position.[Column] AS PositionColumn, dbo.Position.Floor AS PositionFloor, dbo.Position.WarehouseNr AS PositionWarehouseNr,dbo.Position.WarehouseAreaNr as PositionWarehouseAreaNr
FROM         dbo.Storage INNER JOIN
                      dbo.UniqueItem ON dbo.Storage.UniqItemNr = dbo.UniqueItem.Nr INNER JOIN
                      dbo.Position ON dbo.Storage.PositionNr = dbo.Position.Nr

GO

------------------------------------------------------------------------------------------------------------------------
USE [AgvWarehouseDb]
GO

/****** Object:  View [dbo].[PickListStorageView]    Script Date: 02/19/2017 13:10:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[PickListStorageView]
AS
SELECT     dbo.PickList.Nr, dbo.PickListItem.CreatedAt, dbo.PickList.UpdatedAt, dbo.PickListItem.Id AS PickListItemId, dbo.PickListItem.PickListNr AS PickListItemPickListNr, 
                      dbo.PickListItem.UniqItemNr AS PickListItemUniqItemNr, dbo.PickListItem.CreatedAt AS PickListItemCreatedAt, dbo.PickListItem.UpdatedAt AS PickListItemUpdatedAt, 
                      dbo.UniqueItem.Nr AS UniqueItemNr, dbo.UniqueItem.BoxTypeId AS UniqueItemBoxTypeId, dbo.UniqueItem.PartNr AS UniqueItemPartNr, 
                      dbo.UniqueItem.KNr AS UniqueItemKNr, dbo.UniqueItem.KNrWithYear AS UniqueItemKNrWithYear, dbo.UniqueItem.CheckCode AS UniqueItemCheckCode, 
                      dbo.UniqueItem.KskNr AS UniqueItemKskNr, dbo.UniqueItem.QR AS UniqueItemQR, dbo.UniqueItem.State AS UniqueItemState, 
                      dbo.UniqueItem.CreatedAt AS UniqueItemCreatedAt, dbo.UniqueItem.UpdatedAt AS UniqueItemUpdatedAt, dbo.Storage.Id AS StorageId, 
                      dbo.Storage.PositionNr AS StoragePositionNr, dbo.Storage.PartNr AS StoragePartNr, dbo.Storage.FIFO AS StorageFIFO, dbo.Storage.UniqItemNr AS StorageUniqItemNr, 
                      dbo.Storage.CreatedAt AS StorageCreatedAt, dbo.Storage.UpdatedAt AS StorageUpdatedAt, dbo.Position.WarehouseNr AS PositionWarehouseNr, 
                      dbo.Position.Floor AS PositionFloor, dbo.Position.[Column] AS PositionColumn, dbo.Position.Row AS PositionRow, dbo.Position.State AS PositionState, 
                      dbo.Position.RoadMachineIndex AS PositionRoadMachineIndex,dbo.Position.WarehouseAreaNr as PositionWarehouseAreaNr
FROM         dbo.PickList INNER JOIN
                      dbo.PickListItem ON dbo.PickListItem.PickListNr = dbo.PickList.Nr INNER JOIN
                      dbo.UniqueItem ON dbo.PickListItem.UniqItemNr = dbo.UniqueItem.Nr LEFT OUTER JOIN
                      dbo.Storage ON dbo.UniqueItem.Nr = dbo.Storage.UniqItemNr LEFT OUTER JOIN
                      dbo.Position ON dbo.Storage.PositionNr = dbo.Position.Nr

GO


---------------------------------------------------------------------------------------------------------
USE [AgvWarehouseDb]
GO

/****** Object:  View [dbo].[PositionStorageView]    Script Date: 02/19/2017 16:11:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[PositionStorageView]
AS
SELECT     dbo.Position.Nr, dbo.Position.WarehouseNr, dbo.Position.Floor, dbo.Position.[Column], dbo.Position.Row, dbo.Position.State, dbo.Position.RoadMachineIndex, 
                      dbo.Position.IsLocked, dbo.Position.InStorePriority, dbo.Position.WarehouseAreaNr, dbo.Storage.Id AS StorageId, dbo.Storage.PositionNr AS StoragePositionNr, 
                      dbo.Storage.PartNr AS StoragePartNr, dbo.Storage.FIFO AS StorageFIFO, dbo.Storage.UniqItemNr AS StorageUniqItemNr, dbo.Storage.CreatedAt AS StorageCreatedAt, 
                      dbo.WarehouseArea.IsLocked AS WarehouseAreaIsLocked, dbo.WarehouseArea.Remark AS WarehouseAreaRemark, 
                      dbo.WarehouseArea.Color AS WarehouseAreaColor, dbo.WarehouseArea.InStorePriority AS WarehouseAreaInStorePriority, 
                      dbo.WarehouseArea.WarehouseNr AS WarehouseAreaWarehouseNr
FROM         dbo.Position LEFT OUTER JOIN
                      dbo.Storage ON dbo.Position.Nr = dbo.Storage.PositionNr LEFT OUTER JOIN
                      dbo.WarehouseArea ON dbo.WarehouseArea.Nr = dbo.Position.WarehouseAreaNr

GO


