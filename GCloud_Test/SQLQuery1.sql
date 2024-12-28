DECLARE @p0 AS VARCHAR(MAX)='Test'
DECLARE @p1 AS VARCHAR(MAX)='Values'

SET NOCOUNT ON;
INSERT INTO [dbo].[SampleObjects] (Name, Description)
OUTPUT INSERTED.[Id]
VALUES (@p0, @p1);