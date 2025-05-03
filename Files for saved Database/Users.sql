CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100) UNIQUE,
    PasswordHash NVARCHAR(255),
    Role NVARCHAR(50),
    Subject NVARCHAR(100),
    Position NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
	Status NVARCHAR(20) DEFAULT 'Active',
	IsDeleted BIT DEFAULT 0,
	RecentPasswordHash NVARCHAR(255)
);




	USE PUPSIS