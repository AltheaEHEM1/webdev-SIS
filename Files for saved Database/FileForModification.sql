USE PUPSIS

ALTER TABLE [PUPSIS].[dbo].[Admission]
DROP COLUMN GradeLevel;

ALTER TABLE [PUPSIS].[dbo].[Admission]
DROP CONSTRAINT chk_GradeLevel;

ALTER TABLE [PUPSIS].[dbo].[Admission]
ADD GradeLevel NVARCHAR(10) NULL CHECK (GradeLevel IN ('Grade7', 'Grade8', 'Grade9', 'Grade10'));


ALTER TABLE Admission ADD DateOfRegistration DATETIME DEFAULT GETDATE();

SELECT COLUMN_NAME, CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'Admission';

EXEC sp_help 'Admission';

EXEC sp_help 'Announcements';

ALTER TABLE Admission 
ALTER COLUMN Id INT NOT NULL;

