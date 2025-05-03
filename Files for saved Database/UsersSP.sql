-- Update the sp_InsertUser stored procedure if needed
ALTER PROCEDURE sp_InsertUser
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(50),
    @Subject NVARCHAR(100) = NULL,
    @Position NVARCHAR(50) = NULL,
    @Status NVARCHAR(20) = 'Active',
    @NewUserId INT OUTPUT
AS
BEGIN
    INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Role, Subject, Position, Status, CreatedAt)
    VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Role, @Subject, @Position, @Status, GETDATE())
    
    SET @NewUserId = SCOPE_IDENTITY()
END



ALTER PROCEDURE sp_LoginUser
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Users
    WHERE Email = @Email
    AND Status = 'Active'
    AND IsDeleted = 0;
END








USE PUPSIS

CREATE PROCEDURE sp_SelectUserById
    @Id INT
AS
BEGIN
    -- Retrieve user by ID, ensuring the user is not soft deleted
    SELECT * FROM Users
    WHERE Id = @Id
    AND IsDeleted = 0;
END;




/*ALTER PROCEDURE sp_SoftDeleteUser
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET IsDeleted = 1
    WHERE Id = @Id AND IsDeleted = 0;
END;

*/

USE PUPSIS
GO

SELECT * FROM Users WHERE Id = 1 -- Replace with actual number


CREATE PROCEDURE sp_ChangePassword
    @UserId INT,
    @NewPasswordHash NVARCHAR(255)
AS
BEGIN
    -- Save the current password to RecentPasswordHash first
    UPDATE Users
    SET 
        RecentPasswordHash = PasswordHash,
        PasswordHash = @NewPasswordHash
    WHERE Id = @UserId AND IsDeleted = 0;
END;


SELECT Email, PasswordHash FROM Users WHERE Email = 'emilyn@gmail.com';



ALTER PROCEDURE sp_SoftDeleteUser
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @RowCount INT = 0;
    
    UPDATE Users
    SET 
        IsDeleted = 1,
        Status = 'Inactive'  -- Also update status for consistency
    WHERE Id = @Id AND IsDeleted = 0;
    
    SET @RowCount = @@ROWCOUNT;
    
    -- Return the number of rows affected
    RETURN @RowCount;
END;


ALTER PROCEDURE UpdateFaculty
    @Id INT,
    @Subject NVARCHAR(100),
    @Position NVARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET 
        Subject = @Subject,
        Position = @Position
    WHERE Id = @Id AND Role = 'Faculty' AND IsDeleted = 0;
END;



CREATE PROCEDURE GetUserByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Users
    WHERE Email = @Email AND Status = 'Active'
END

