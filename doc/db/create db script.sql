USE [master]
GO
/****** Object:  Database [AgvWarehoueDb]    Script Date: 12/12/2016 03:11:29 ******/
CREATE DATABASE [AgvWarehoueDb] ON  PRIMARY 
( NAME = N'AgvWarehoueDb', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER20082\MSSQL\DATA\AgvWarehoueDb.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AgvWarehoueDb_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER20082\MSSQL\DATA\AgvWarehoueDb_log.ldf' , SIZE = 1536KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AgvWarehoueDb] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AgvWarehoueDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AgvWarehoueDb] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET ANSI_NULLS OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET ANSI_PADDING OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET ARITHABORT OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [AgvWarehoueDb] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [AgvWarehoueDb] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [AgvWarehoueDb] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET  DISABLE_BROKER
GO
ALTER DATABASE [AgvWarehoueDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [AgvWarehoueDb] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [AgvWarehoueDb] SET  READ_WRITE
GO
ALTER DATABASE [AgvWarehoueDb] SET RECOVERY SIMPLE
GO
ALTER DATABASE [AgvWarehoueDb] SET  MULTI_USER
GO
ALTER DATABASE [AgvWarehoueDb] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [AgvWarehoueDb] SET DB_CHAINING OFF
GO
USE [AgvWarehoueDb]
GO
/****** Object:  Table [dbo].[Tray]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tray](
	[Nr] [varchar](50) NOT NULL,
	[State] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Tray] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery](
	[Nr] [varchar](50) NOT NULL,
	[State] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Delivery] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BoxType]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BoxType](
	[Id] [int] NOT NULL,
	[TrayQty] [int] NULL,
	[Type] [int] NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_BoxType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Part]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Part](
	[Nr] [varchar](50) NOT NULL,
	[BoxType] [int] NULL,
 CONSTRAINT [PK_Part] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockTask]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StockTask](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RoadMachineIndex] [int] NULL,
	[BoxType] [int] NULL,
	[PositionNr] [varchar](50) NULL,
	[PositionFloor] [int] NULL,
	[PositionColumn] [int] NULL,
	[PositionRow] [int] NULL,
	[AgvPassFlag] [int] NULL,
	[RestPositionFlag] [int] NULL,
	[BarCode] [varchar](50) NULL,
	[State] [int] NULL,
	[Type] [int] NULL,
	[TrayReverseNo] [int] NULL,
	[TrayNum] [int] NULL,
	[DeliveryItemNum] [int] NULL,
	[DeliveryBatchId] [varchar](50) NULL,
	[TrayBatchId] [varchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_StockTask] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockMovement]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StockMovement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqItemNr] [varchar](50) NULL,
	[SourcePosition] [varchar](50) NULL,
	[AimedPosition] [varchar](50) NULL,
	[Type] [int] NULL,
	[Operator] [varchar](50) NULL,
	[Time] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_Movement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Warehouse]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Warehouse](
	[Nr] [varchar](50) NOT NULL,
	[AgvId] [int] NULL,
 CONSTRAINT [PK_WareHouse] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_WareHouse] UNIQUE NONCLUSTERED 
(
	[AgvId] ASC,
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UniqueItem]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UniqueItem](
	[Nr] [varchar](50) NOT NULL,
	[BoxTypeId] [int] NULL,
	[PartNr] [varchar](50) NULL,
	[KNr] [varchar](50) NULL,
	[KNrWithYear] [varchar](50) NULL,
	[CheckCode] [varchar](50) NULL,
	[KskNr] [varchar](50) NULL,
	[QR] [varchar](50) NULL,
	[State] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryTray]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryTray](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryNr] [varchar](50) NULL,
	[TrayNr] [varchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_DeliveryTray] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Position]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Position](
	[Nr] [varchar](50) NOT NULL,
	[WarehouseNr] [varchar](50) NOT NULL,
	[Floor] [int] NOT NULL,
	[Column] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[State] [int] NULL,
	[RoadMachineIndex] [int] NULL,
 CONSTRAINT [PK_Posation] PRIMARY KEY CLUSTERED 
(
	[Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Storage]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Storage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PositionNr] [varchar](50) NOT NULL,
	[PartNr] [varchar](50) NULL,
	[FIFO] [datetime] NULL,
	[UniqItemNr] [varchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Storage_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryItem]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryNr] [varchar](50) NULL,
	[UniqItemNr] [varchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_DeliveryItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TrayItem]    Script Date: 12/12/2016 03:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TrayItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqItemNr] [varchar](50) NULL,
	[TrayNr] [varchar](50) NULL,
	[State] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_TrayItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[PositionStorageView]    Script Date: 12/12/2016 03:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PositionStorageView]
AS
SELECT     dbo.Position.Nr, dbo.Position.WarehouseNr, dbo.Position.Floor, dbo.Position.[Column], dbo.Position.Row, dbo.Position.State, dbo.Position.RoadMachineIndex, 
                      dbo.Storage.Id AS StorageId, dbo.Storage.PositionNr AS StoragePositionNr, dbo.Storage.PartNr AS StoragePartNr, dbo.Storage.FIFO AS StorageFIFO, 
                      dbo.Storage.UniqItemNr AS StorageUniqItemNr, dbo.Storage.CreatedAt AS StorageCreatedAt
FROM         dbo.Position LEFT OUTER JOIN
                      dbo.Storage ON dbo.Position.Nr = dbo.Storage.PositionNr
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[12] 4[50] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Position"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 188
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Storage"
            Begin Extent = 
               Top = 6
               Left = 257
               Bottom = 213
               Right = 400
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PositionStorageView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PositionStorageView'
GO
/****** Object:  View [dbo].[DeliveryStorageView]    Script Date: 12/12/2016 03:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DeliveryStorageView]
AS
SELECT     dbo.Delivery.Nr, dbo.Delivery.State, dbo.Delivery.CreatedAt, dbo.Delivery.UpdatedAt, dbo.DeliveryItem.Id AS DeliveryItemId, 
                      dbo.DeliveryItem.DeliveryNr AS DeliveryItemDeliveryNr, dbo.DeliveryItem.UniqItemNr AS DeliveryItemUniqItemNr, 
                      dbo.DeliveryItem.CreatedAt AS DeliveryItemCreatedAt, dbo.DeliveryItem.UpdatedAt AS DeliveryItemUpdatedAt, dbo.UniqueItem.Nr AS UniqueItemNr, 
                      dbo.UniqueItem.BoxTypeId AS UniqueItemBoxTypeId, dbo.UniqueItem.PartNr AS UniqueItemPartNr, dbo.UniqueItem.KNr AS UniqueItemKNr, 
                      dbo.UniqueItem.KNrWithYear AS UniqueItemKNrWithYear, dbo.UniqueItem.CheckCode AS UniqueItemCheckCode, dbo.UniqueItem.KskNr AS UniqueItemKskNr, 
                      dbo.UniqueItem.QR AS UniqueItemQR, dbo.UniqueItem.State AS UniqueItemState, dbo.UniqueItem.CreatedAt AS UniqueItemCreatedAt, 
                      dbo.UniqueItem.UpdatedAt AS UniqueItemUpdatedAt, dbo.Storage.Id AS StorageId, dbo.Storage.PositionNr AS StoragePositionNr, dbo.Storage.PartNr AS StoragePartNr, 
                      dbo.Storage.FIFO AS StorageFIFO, dbo.Storage.UniqItemNr AS StorageUniqItemNr, dbo.Storage.CreatedAt AS StorageCreatedAt, 
                      dbo.Storage.UpdatedAt AS StorageUpdatedAt
FROM         dbo.Delivery INNER JOIN
                      dbo.DeliveryItem ON dbo.DeliveryItem.DeliveryNr = dbo.Delivery.Nr INNER JOIN
                      dbo.UniqueItem ON dbo.DeliveryItem.UniqItemNr = dbo.UniqueItem.Nr LEFT OUTER JOIN
                      dbo.Storage ON dbo.UniqueItem.Nr = dbo.Storage.UniqItemNr
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[22] 4[51] 2[28] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DeliveryItem"
            Begin Extent = 
               Top = 18
               Left = 227
               Bottom = 137
               Right = 370
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "UniqueItem"
            Begin Extent = 
               Top = 17
               Left = 424
               Bottom = 136
               Right = 574
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Storage"
            Begin Extent = 
               Top = 10
               Left = 651
               Bottom = 129
               Right = 794
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Delivery"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 184
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2190
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DeliveryStorageView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DeliveryStorageView'
GO
/****** Object:  View [dbo].[DeliveryItemStorageView]    Script Date: 12/12/2016 03:11:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DeliveryItemStorageView]
AS
SELECT     dbo.DeliveryItem.Id, dbo.DeliveryItem.DeliveryNr, dbo.DeliveryItem.UniqItemNr, dbo.DeliveryItem.CreatedAt, dbo.DeliveryItem.UpdatedAt, 
                      dbo.UniqueItem.Nr AS UniqueItemNr, dbo.UniqueItem.BoxTypeId AS UniqueItemBoxTypeId, dbo.UniqueItem.PartNr AS UniqueItemPartNr, 
                      dbo.UniqueItem.KNr AS UniqueItemKNr, dbo.UniqueItem.KNrWithYear AS UniqueItemKNrWithYear, dbo.UniqueItem.CheckCode AS UniqueItemCheckCode, 
                      dbo.UniqueItem.KskNr AS UniqueItemKskNr, dbo.UniqueItem.QR AS UniqueItemQR, dbo.UniqueItem.State AS UniqueItemState, 
                      dbo.UniqueItem.CreatedAt AS UniqueItemCreatedAt, dbo.UniqueItem.UpdatedAt AS UniqueItemUpdatedAt, dbo.Storage.Id AS StorageId, 
                      dbo.Storage.PositionNr AS StoragePositionNr, dbo.Storage.PartNr AS StoragePartNr, dbo.Storage.FIFO AS StorageFIFO, dbo.Storage.UniqItemNr AS StorageUniqItemNr, 
                      dbo.Storage.CreatedAt AS StorageCreatedAt, dbo.Storage.UpdatedAt AS StorageUpdatedAt
FROM         dbo.DeliveryItem INNER JOIN
                      dbo.UniqueItem ON dbo.DeliveryItem.UniqItemNr = dbo.UniqueItem.Nr LEFT OUTER JOIN
                      dbo.Storage ON dbo.UniqueItem.Nr = dbo.Storage.UniqItemNr
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[55] 2[23] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DeliveryItem"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 179
               Right = 181
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "UniqueItem"
            Begin Extent = 
               Top = 6
               Left = 219
               Bottom = 250
               Right = 369
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Storage"
            Begin Extent = 
               Top = 6
               Left = 407
               Bottom = 239
               Right = 550
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2895
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DeliveryItemStorageView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DeliveryItemStorageView'
GO
/****** Object:  ForeignKey [FK_Item_Part]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[UniqueItem]  WITH CHECK ADD  CONSTRAINT [FK_Item_Part] FOREIGN KEY([PartNr])
REFERENCES [dbo].[Part] ([Nr])
GO
ALTER TABLE [dbo].[UniqueItem] CHECK CONSTRAINT [FK_Item_Part]
GO
/****** Object:  ForeignKey [FK_UniqueItem_BoxType]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[UniqueItem]  WITH CHECK ADD  CONSTRAINT [FK_UniqueItem_BoxType] FOREIGN KEY([BoxTypeId])
REFERENCES [dbo].[BoxType] ([Id])
GO
ALTER TABLE [dbo].[UniqueItem] CHECK CONSTRAINT [FK_UniqueItem_BoxType]
GO
/****** Object:  ForeignKey [FK_DeliveryTray_Delivery]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[DeliveryTray]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryTray_Delivery] FOREIGN KEY([DeliveryNr])
REFERENCES [dbo].[Delivery] ([Nr])
GO
ALTER TABLE [dbo].[DeliveryTray] CHECK CONSTRAINT [FK_DeliveryTray_Delivery]
GO
/****** Object:  ForeignKey [FK_DeliveryTray_Tray]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[DeliveryTray]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryTray_Tray] FOREIGN KEY([TrayNr])
REFERENCES [dbo].[Tray] ([Nr])
GO
ALTER TABLE [dbo].[DeliveryTray] CHECK CONSTRAINT [FK_DeliveryTray_Tray]
GO
/****** Object:  ForeignKey [FK_Posation_WareHouse]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[Position]  WITH CHECK ADD  CONSTRAINT [FK_Posation_WareHouse] FOREIGN KEY([WarehouseNr])
REFERENCES [dbo].[Warehouse] ([Nr])
GO
ALTER TABLE [dbo].[Position] CHECK CONSTRAINT [FK_Posation_WareHouse]
GO
/****** Object:  ForeignKey [FK_Storage_Part]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_Part] FOREIGN KEY([PartNr])
REFERENCES [dbo].[Part] ([Nr])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_Part]
GO
/****** Object:  ForeignKey [FK_Storage_Posation]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_Posation] FOREIGN KEY([PositionNr])
REFERENCES [dbo].[Position] ([Nr])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_Posation]
GO
/****** Object:  ForeignKey [FK_Storage_UniqueItem]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_UniqueItem] FOREIGN KEY([UniqItemNr])
REFERENCES [dbo].[UniqueItem] ([Nr])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_UniqueItem]
GO
/****** Object:  ForeignKey [FK_DeliveryItem_Delivery]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[DeliveryItem]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryItem_Delivery] FOREIGN KEY([DeliveryNr])
REFERENCES [dbo].[Delivery] ([Nr])
GO
ALTER TABLE [dbo].[DeliveryItem] CHECK CONSTRAINT [FK_DeliveryItem_Delivery]
GO
/****** Object:  ForeignKey [FK_DeliveryItem_UniqueItem]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[DeliveryItem]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryItem_UniqueItem] FOREIGN KEY([UniqItemNr])
REFERENCES [dbo].[UniqueItem] ([Nr])
GO
ALTER TABLE [dbo].[DeliveryItem] CHECK CONSTRAINT [FK_DeliveryItem_UniqueItem]
GO
/****** Object:  ForeignKey [FK_TrayItem_Tray]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[TrayItem]  WITH CHECK ADD  CONSTRAINT [FK_TrayItem_Tray] FOREIGN KEY([TrayNr])
REFERENCES [dbo].[Tray] ([Nr])
GO
ALTER TABLE [dbo].[TrayItem] CHECK CONSTRAINT [FK_TrayItem_Tray]
GO
/****** Object:  ForeignKey [FK_TrayItem_UniqueItem]    Script Date: 12/12/2016 03:11:32 ******/
ALTER TABLE [dbo].[TrayItem]  WITH CHECK ADD  CONSTRAINT [FK_TrayItem_UniqueItem] FOREIGN KEY([UniqItemNr])
REFERENCES [dbo].[UniqueItem] ([Nr])
GO
ALTER TABLE [dbo].[TrayItem] CHECK CONSTRAINT [FK_TrayItem_UniqueItem]
GO
