USE [AgvWarehouseDb]
GO

/****** Object:  StoredProcedure [dbo].[SelectCurrentStorage]    Script Date: 05/02/2017 17:44:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectCurrentStorage]
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
select UniqueItemPartNr as '配置号',	UniqueItemKNr as '大众K号',
   UniqueItemKskNr as '莱尼内部K号', UniqueItemCheckCode as	'验证码',PositionNr as '库位号', FIFO as 'FIFO',
     '箱型'=case when UniqueItemBoxTypeId=1 then 'Big' when UniqueItemBoxTypeId=2 then 'Small' else '' end,	PositionRoadMachineIndex as '巷道机号'
 from StorageUniqueItemView;
END


GO


