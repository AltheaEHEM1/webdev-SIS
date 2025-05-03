-- READ PROCEDURE to get all active students
CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SELECT * FROM Student WHERE IsDeleted = 0;  -- Fetch only active records
END;



ALTER PROCEDURE GetAllStudents
AS
BEGIN
    SELECT * FROM Students
    WHERE IsDeleted = 0
    ORDER BY Id DESC;
END;




-- READ PROCEDURE to get a single student by ID, with grade-level filtering
CREATE PROCEDURE GetSingleStudentByGrade
    @Id INT,
    @GradeLevel NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;  -- Prevent extra messages

    SELECT Id, SectionName, CountStudent, Adviser, GradeLevel, DateCreated
    FROM Student
    WHERE Id = @Id AND GradeLevel = @GradeLevel AND IsDeleted = 0;
END;

ALTER PROCEDURE GetSingleStudentByGrade
    @Id INT
AS
BEGIN
    SELECT * FROM Students
    WHERE Id = @Id AND IsDeleted = 0;
END;




-- UPDATE PROCEDURE for updating student details (all grades)
CREATE PROCEDURE UpdateStudentPersonalInfo
    @Id INT,
    @FirstName NVARCHAR(100),
    @MiddleName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Suffix NVARCHAR(20),
    @Email NVARCHAR(255),
    @DateOfBirth DATE,
    @CivilStatus NVARCHAR(50),
    @Sex NVARCHAR(10),
    @Country NVARCHAR(100),
    @Region NVARCHAR(100),
    @City NVARCHAR(100),
    @Height DECIMAL(5,2),
    @Weight DECIMAL(5,2),
    @Religion NVARCHAR(100),
    @Disability NVARCHAR(100)
AS
BEGIN
    UPDATE Students
    SET FirstName = @FirstName,
        MiddleName = @MiddleName,
        LastName = @LastName,
        Suffix = @Suffix,
        Email = @Email,
        DateOfBirth = @DateOfBirth,
        CivilStatus = @CivilStatus,
        Sex = @Sex,
        Country = @Country,
        Region = @Region,
        City = @City,
        Height = @Height,
        Weight = @Weight,
        Religion = @Religion,
        Disability = @Disability
    WHERE Id = @Id
END


CREATE PROCEDURE UpdateStudentContact
    @Id INT,
    @Landline_number NVARCHAR(15),
    @Phone_number NVARCHAR(15),
    @Emergency_landline_number NVARCHAR(15),
    @ContactPerson NVARCHAR(100),
    @ContactNumber NVARCHAR(15),
    @Relationship NVARCHAR(50)
AS
BEGIN
    UPDATE Students
    SET Landline_number = @Landline_number,
        Phone_number = @Phone_number,
        Emergency_landline_number = @Emergency_landline_number,
        ContactPerson = @ContactPerson,
        ContactNumber = @ContactNumber,
        Relationship = @Relationship
    WHERE Id = @Id
END


CREATE PROCEDURE UpdateStudentAddress
    @Id INT,
    @HouseNo NVARCHAR(20),
    @Barangay NVARCHAR(100),
    @Street NVARCHAR(100),
    @Municipality NVARCHAR(100),
    @Province NVARCHAR(100),
    @ZipCode NVARCHAR(10),
    @PermanentHouseNo NVARCHAR(20),
    @PermanentBarangay NVARCHAR(100),
    @PermanentStreet NVARCHAR(100),
    @PermanentMunicipality NVARCHAR(100),
    @PermanentProvince NVARCHAR(100),
    @PermanentZipCode NVARCHAR(10)
AS
BEGIN
    UPDATE Students
    SET HouseNo = @HouseNo,
        Barangay = @Barangay,
        Street = @Street,
        Municipality = @Municipality,
        Province = @Province,
        ZipCode = @ZipCode,
        PermanentHouseNo = @PermanentHouseNo,
        PermanentBarangay = @PermanentBarangay,
        PermanentStreet = @PermanentStreet,
        PermanentMunicipality = @PermanentMunicipality,
        PermanentProvince = @PermanentProvince,
        PermanentZipCode = @PermanentZipCode
    WHERE Id = @Id
END


CREATE PROCEDURE UpdateStudentFamily
    @Id INT,
    @ParentFirstName NVARCHAR(100),
    @ParentMiddleName NVARCHAR(100),
    @ParentLastName NVARCHAR(100),
    @ParentContactNo NVARCHAR(15),
    @ParentRelationship NVARCHAR(50),
    @GuardianFirstName NVARCHAR(100),
    @GuardianMiddleName NVARCHAR(100),
    @GuardianLastName NVARCHAR(100),
    @GuardianContactNo NVARCHAR(15),
    @GuardianRelationship NVARCHAR(50)
AS
BEGIN
    UPDATE Students
    SET ParentFirstName = @ParentFirstName,
        ParentMiddleName = @ParentMiddleName,
        ParentLastName = @ParentLastName,
        ParentContactNo = @ParentContactNo,
        ParentRelationship = @ParentRelationship,
        GuardianFirstName = @GuardianFirstName,
        GuardianMiddleName = @GuardianMiddleName,
        GuardianLastName = @GuardianLastName,
        GuardianContactNo = @GuardianContactNo,
        GuardianRelationship = @GuardianRelationship
    WHERE Id = @Id
END

CREATE PROCEDURE UpdateStudentSchool
    @Id INT,
    @SchoolName NVARCHAR(150),
    @SchoolAddress NVARCHAR(150),
    @SchoolContact NVARCHAR(15),
    @SchoolType NVARCHAR(50),
    @YearOfGraduation INT,
    @LRN NVARCHAR(20),
    @GWA DECIMAL(4,2)
AS
BEGIN
    UPDATE Students
    SET SchoolName = @SchoolName,
        SchoolAddress = @SchoolAddress,
        SchoolContact = @SchoolContact,
        SchoolType = @SchoolType,
        YearOfGraduation = @YearOfGraduation,
        LRN = @LRN,
        GWA = @GWA
    WHERE Id = @Id
END





-- DELETE PROCEDURE (soft delete)
CREATE PROCEDURE SoftDeleteStudent
    @Id INT
AS
BEGIN
    UPDATE Student
    SET IsDeleted = 1  -- Mark record as deleted
    WHERE Id = @Id;
END;





-- EXECUTE EXAMPLES
-- Adding students
EXEC AddStudent @SectionName = '8-Aquamarine', @CountStudent = 35, @Adviser = 'Mr. Smith', @GradeLevel = 'Grade 8';
EXEC AddStudent @SectionName = '8-Sapphire', @CountStudent = 40, @Adviser = 'Ms. Johnson', @GradeLevel = 'Grade 8';

-- Getting all students
EXEC GetAllStudents;

-- Getting a single Grade 7 student
EXEC GetSingleStudentByGrade @Id = 1, @GradeLevel = 'Grade 7';

-- Updating student details
EXEC UpdateStudent @Id = 1, @SectionName = '8-Amethyst', @CountStudent = 38, @Adviser = 'Ms. Carter', @GradeLevel = 'Grade 8';

-- Soft deleting a student
EXEC SoftDeleteStudent @Id = 2;
