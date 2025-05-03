CREATE PROCEDURE UpdatePersonalInfo
    @Id INT,
    @Name NVARCHAR(100),
    @MiddleName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Bio NVARCHAR(255),
    @DateOfBirth DATE,
    @Email NVARCHAR(255),
    @Phone NVARCHAR(11)
AS
BEGIN
    UPDATE Profile
    SET Name = @Name,
        MiddleName = @MiddleName,
        LastName = @LastName,
        Bio = @Bio,
        DateOfBirth = @DateOfBirth,
        Email = @Email,
        Phone = @Phone
    WHERE Id = @Id;
END;


CREATE PROCEDURE UpdateAddress
    @Id INT,
    @HouseNo NVARCHAR(10),
    @Street NVARCHAR(255),
    @Barangay NVARCHAR(255),
    @City NVARCHAR(255),
    @Province NVARCHAR(255),
    @ZipCode NVARCHAR(4)
AS
BEGIN
    UPDATE Profile
    SET HouseNo = @HouseNo,
        Street = @Street,
        Barangay = @Barangay,
        City = @City,
        Province = @Province,
        ZipCode = @ZipCode
    WHERE Id = @Id;
END;


CREATE PROCEDURE UpdateUserProfile
    @UserId INT,
    @PhotoPath NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if user exists before updating
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        PRINT 'User not found';
        RETURN;
    END

    -- Update photo path
    UPDATE Users
    SET PhotoPath = @PhotoPath
    WHERE Id = @UserId;
END;
GO


ALTER PROCEDURE GetAllProfiles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id,
        p.Name,
        p.MiddleName,
        p.LastName,
        p.Bio,
        p.DateOfBirth,
        p.Email,
        p.Phone,
        p.HouseNo,
        p.Street,
        p.Barangay,
        p.City,
        p.Province,
        p.ZipCode,
        u.PhotoPath
    FROM Profile p
    LEFT JOIN Profile u ON p.Id = u.Id;
END;
GO

EXEC GetAllProfiles