CREATE TABLE Section (
    SectionId INT IDENTITY(1,1) PRIMARY KEY,
    SectionName NVARCHAR(50) NOT NULL,
    EnrollmentId INT NOT NULL,
    AssignedDate DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(Id)  -- Foreign key constraint
);

USE PUPSIS