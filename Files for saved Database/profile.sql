USE PUPSIS;

CREATE TABLE Profile (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100) NULL, 
    LastName NVARCHAR(100) NOT NULL,
    Bio NVARCHAR(255) NULL,
    DateOfBirth DATE NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL CHECK (Email LIKE '_%@_%._%'),  -- Ensures a basic email format
    Phone NVARCHAR(11) NOT NULL CHECK (Phone LIKE '[0-9]%' AND LEN(Phone) = 11),  -- Ensures exactly 11 digits
    HouseNo NVARCHAR(10) NOT NULL CHECK (HouseNo LIKE '[0-9]%'),  -- Ensures only digits
    Street NVARCHAR(255) NULL,
    Barangay NVARCHAR(255) NULL,
    City NVARCHAR(255) NULL,
    Province NVARCHAR(255) NULL,
    ZipCode NVARCHAR(4) NOT NULL CHECK (ZipCode LIKE '[0-9]%' AND LEN(ZipCode) = 4),  -- Ensures exactly 4 digits
    PhotoPath NVARCHAR(255) NULL  
);
