IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[ExecuteMigrationScript]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].ExecuteMigrationScript