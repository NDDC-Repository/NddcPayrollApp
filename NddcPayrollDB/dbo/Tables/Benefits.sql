CREATE TABLE [dbo].[Benefits]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SalaryScaleId] INT NULL, 
    [Benefit] NVARCHAR(100) NULL, 
    [Percentage] FLOAT NULL, 
    [DateCreated] DATETIME NULL, 
    [CreatedBy] NVARCHAR(50) NULL
)
