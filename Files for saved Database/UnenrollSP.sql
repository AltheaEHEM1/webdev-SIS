CREATE PROCEDURE GetAllUnenrollStudents
AS
BEGIN
    SELECT 
        Id, 
        GradeLevel, 
        FirstName, 
        MiddleName, 
        LastName, 
        Suffix, 
        Email, 
        DateOfBirth, 
        CivilStatus, 
        Sex, 
        Country, 
        Region, 
        City, 
        Height, 
        Weight, 
        Religion, 
        Disability, 
        Phone_number, 
        Landline_number, 
        Emergency_landline_number, 
        ContactPerson, 
        ContactNumber, 
        Relationship, 
        HouseNo, 
        Barangay, 
        Street, 
        Municipality, 
        Province, 
        ZipCode, 
        PermanentHouseNo, 
        PermanentBarangay, 
        PermanentStreet, 
        PermanentMunicipality, 
        PermanentProvince, 
        PermanentZipCode, 
        ParentFirstName, 
        ParentMiddleName, 
        ParentLastName, 
        ParentContactNo, 
        ParentRelationship, 
        GuardianFirstName, 
        GuardianMiddleName, 
        GuardianLastName, 
        GuardianContactNo, 
        GuardianRelationship, 
        SchoolName, 
        SchoolAddress, 
        SchoolContact, 
        SchoolLandline, 
        SchoolType, 
        YearOfGraduation, 
        LRN, 
        GWA, 
        DateEnrolled, 
        IsDeleted, 
        SectionName, 
        UnenrolledDate, 
        ReasonDescription
    FROM UnenrolledStudents
    WHERE IsDeleted = 0
    ORDER BY LastName, FirstName;
END;
EXEC GetAllUnenrollStudents;





CREATE PROCEDURE GetByIdUnenrollStudents
    @Id INT
AS
BEGIN
    SELECT 
        Id, 
        GradeLevel, 
        FirstName, 
        MiddleName, 
        LastName, 
        Suffix, 
        Email, 
        DateOfBirth, 
        CivilStatus, 
        Sex, 
        Country, 
        Region, 
        City, 
        Height, 
        Weight, 
        Religion, 
        Disability, 
        Phone_number, 
        Landline_number, 
        Emergency_landline_number, 
        ContactPerson, 
        ContactNumber, 
        Relationship, 
        HouseNo, 
        Barangay, 
        Street, 
        Municipality, 
        Province, 
        ZipCode, 
        PermanentHouseNo, 
        PermanentBarangay, 
        PermanentStreet, 
        PermanentMunicipality, 
        PermanentProvince, 
        PermanentZipCode, 
        ParentFirstName, 
        ParentMiddleName, 
        ParentLastName, 
        ParentContactNo, 
        ParentRelationship, 
        GuardianFirstName, 
        GuardianMiddleName, 
        GuardianLastName, 
        GuardianContactNo, 
        GuardianRelationship, 
        SchoolName, 
        SchoolAddress, 
        SchoolContact, 
        SchoolLandline, 
        SchoolType, 
        YearOfGraduation, 
        LRN, 
        GWA, 
        DateEnrolled, 
        IsDeleted, 
        SectionName, 
        UnenrolledDate, 
        ReasonDescription
    FROM UnenrolledStudents
    WHERE Id = @Id AND IsDeleted = 0;
END;

EXEC GetByIdUnenrollStudents @Id = 1;





DROP PROCEDURE IF EXISTS GetAllUnenrolledStudents;
GO

CREATE PROCEDURE GetAllUnenrolledStudents
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id, 
        GradeLevel, 
        FirstName, 
        MiddleName, 
        LastName, 
        Suffix, 
        Email, 
        DateOfBirth, 
        CivilStatus, 
        Sex, 
        Country, 
        Region, 
        City, 
        Height, 
        Weight, 
        Religion, 
        Disability, 
        Phone_number, 
        Landline_number, 
        Emergency_landline_number, 
        ContactPerson, 
        ContactNumber, 
        Relationship, 
        HouseNo, 
        Barangay, 
        Street, 
        Municipality, 
        Province, 
        ZipCode, 
        PermanentHouseNo, 
        PermanentBarangay, 
        PermanentStreet, 
        PermanentMunicipality, 
        PermanentProvince, 
        PermanentZipCode, 
        ParentFirstName, 
        ParentMiddleName, 
        ParentLastName, 
        ParentContactNo, 
        ParentRelationship, 
        GuardianFirstName, 
        GuardianMiddleName, 
        GuardianLastName, 
        GuardianContactNo, 
        GuardianRelationship, 
        SchoolName, 
        SchoolAddress, 
        SchoolContact, 
        SchoolLandline, 
        SchoolType, 
        YearOfGraduation, 
        LRN, 
        GWA, 
        DateEnrolled, 
        IsDeleted, 
        SectionName, 
        UnenrolledDate, 
        ReasonDescription
    FROM UnenrolledStudents
    WHERE IsDeleted = 0
    ORDER BY LastName, FirstName;
END;
GO














EXEC GetAllUnenrolledStudents 


CREATE PROCEDURE UnenrollStudent
    @StudentId INT,
    @ReasonDescription VARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO UnenrolledStudents (
        GradeLevel, FirstName, MiddleName, LastName, Suffix, Email, DateOfBirth, CivilStatus, Sex,
        Country, Region, City, Height, Weight, Religion, Disability, Phone_number, Landline_number,
        Emergency_landline_number, ContactPerson, ContactNumber, Relationship, HouseNo, Barangay, Street,
        Municipality, Province, ZipCode, PermanentHouseNo, PermanentBarangay, PermanentStreet, PermanentMunicipality,
        PermanentProvince, PermanentZipCode, ParentFirstName, ParentMiddleName, ParentLastName, ParentContactNo,
        ParentRelationship, GuardianFirstName, GuardianMiddleName, GuardianLastName, GuardianContactNo,
        GuardianRelationship, SchoolName, SchoolAddress, SchoolContact, SchoolLandline, SchoolType, YearOfGraduation,
        LRN, GWA, DateEnrolled, IsDeleted, SectionName, UnenrolledDate, ReasonDescription
    )
    SELECT
        GradeLevel, FirstName, MiddleName, LastName, Suffix, Email, DateOfBirth, CivilStatus, Sex,
        Country, Region, City, Height, Weight, Religion, Disability, Phone_number, Landline_number,
        Emergency_landline_number, ContactPerson, ContactNumber, Relationship, HouseNo, Barangay, Street,
        Municipality, Province, ZipCode, PermanentHouseNo, PermanentBarangay, PermanentStreet, PermanentMunicipality,
        PermanentProvince, PermanentZipCode, ParentFirstName, ParentMiddleName, ParentLastName, ParentContactNo,
        ParentRelationship, GuardianFirstName, GuardianMiddleName, GuardianLastName, GuardianContactNo,
        GuardianRelationship, SchoolName, SchoolAddress, SchoolContact, SchoolLandline, SchoolType, YearOfGraduation,
        LRN, GWA, DateEnrolled, IsDeleted, SectionName, GETDATE(), @ReasonDescription
    FROM Students
    WHERE Id = @StudentId;

    DELETE FROM Students WHERE Id = @StudentId;
END;





USE PUPSIS




CREATE PROCEDURE GetUnenrolledStudents
AS
BEGIN
    SELECT * FROM UnenrolledStudents
    WHERE IsDeleted = 0
END


EXEC GetUnenrolledStudents 


CREATE PROCEDURE GetByIdUnenrolledStudents
    @Id INT
AS
BEGIN
    SELECT * FROM UnenrolledStudents
    WHERE Id = @Id AND IsDeleted = 0;
END;

EXEC GetByIdUnenrolledStudents @Id = 1;