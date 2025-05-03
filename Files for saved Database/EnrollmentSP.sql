CREATE PROCEDURE GetByIdEnrollment
    @Id INT
AS
BEGIN
    SELECT * FROM Enrollment
    WHERE Id = @Id AND IsDeleted = 0;
END;
EXEC GetByIdEnrollment @Id = 1;