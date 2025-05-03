CREATE PROCEDURE LoginUser
    @Email NVARCHAR(255),
    @UserCount INT OUTPUT,
    @Role NVARCHAR(50) OUTPUT,
    @HashedPassword NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve hashed password and role from the Users table
    SELECT @HashedPassword = Password, @Role = Role
    FROM Users
    WHERE Email = @Email;

    -- If no user is found, return 0
    IF @HashedPassword IS NULL
    BEGIN
        SET @UserCount = 0;
        RETURN;
    END

    -- User exists, password verification happens in C#
    SET @UserCount = 1;
END;
