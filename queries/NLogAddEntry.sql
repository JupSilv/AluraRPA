GO
/****** Object:  StoredProcedure [dbo].[NLogAddEntry]    Script Date: 15/02/2021 09:29:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[NLogAddEntry] (
  @machineName nvarchar(200),
  @level varchar(5),
  @logged datetime,
  @userName nvarchar(200),
  @threadid nvarchar(200),
  @message nvarchar(max),
  @logger nvarchar(250),
  @exception nvarchar(max)
) AS
BEGIN
  INSERT INTO [dbo].[NLog] (
    [MachineName],
    [Level],
	[Logged],
    [UserName],
	[ThreadId],
    [Message],
	[Logger],
    [Exception]
  ) VALUES (
    @machineName,
    @level,
	@logged,
    @userName,
	@threadid,
    @message,
	@logger,
    @exception
  );
END

