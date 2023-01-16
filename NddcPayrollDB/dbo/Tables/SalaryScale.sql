CREATE TABLE [dbo].[SalaryScale]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GradeLevel] NVARCHAR(50) NULL, 
    [Category] NVARCHAR(50) NULL, 
    [BasicSalary] MONEY NULL, 
    [DateCreated] DATETIME NULL, 
    [CreatedBy] NVARCHAR(50) NULL
)
