CREATE PROCEDURE [dbo].ExecuteMigrationScript
           @fileName VARCHAR(255),
           @hash VARCHAR(255),
           @scriptText NVARCHAR(MAX)

AS

DECLARE @t1 DATETIME;
DECLARE @t2 DATETIME;
DECLARE @duration INT;

BEGIN TRANSACTION

SET @t1 = GETDATE();

EXEC sys.sp_executesql @scriptText

SET @t2 = GETDATE();
SET @duration = DATEDIFF(millisecond,@t1,@t2);

INSERT INTO [dbo].[Migrations]
           ([FileName]
           ,[Hash]
           ,[ExecutionDate]
           ,[Duration])
     VALUES
           (@fileName
           ,@hash
           ,GETUTCDATE()
           ,@duration)

COMMIT TRANSACTION