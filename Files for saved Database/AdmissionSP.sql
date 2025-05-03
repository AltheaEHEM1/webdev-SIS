-- Stored Procedure to Insert a New Admission Record
CREATE PROCEDURE CreateAdmissions
    @GradeLevel NVARCHAR(10),
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @Suffix VARCHAR(10) = NULL,
    @Email VARCHAR(100),
    @DateOfBirth DATE,
    @CivilStatus VARCHAR(10),
    @Sex VARCHAR(10),
    @Country VARCHAR(50),
    @Region VARCHAR(50),
    @City VARCHAR(50),
    @Height DECIMAL(5,2),
    @Weight DECIMAL(5,2),
    @Religion VARCHAR(50) = NULL,
    @Disability VARCHAR(100) = NULL,
    @Phone_number VARCHAR(11),
    @Landline_number VARCHAR(8) = NULL,
    @Emergency_landline_number VARCHAR(8) = NULL,
    @ContactPerson VARCHAR(100),
    @ContactNumber VARCHAR(15),
    @Relationship VARCHAR(50),
    @HouseNo VARCHAR(50),
    @Barangay VARCHAR(50),
    @Street VARCHAR(50),
    @Municipality VARCHAR(50),
    @Province VARCHAR(50),
    @ZipCode VARCHAR(10),
    @PermanentHouseNo VARCHAR(50),
    @PermanentBarangay VARCHAR(50),
    @PermanentStreet VARCHAR(50),
    @PermanentMunicipality VARCHAR(50),
    @PermanentProvince VARCHAR(50),
    @PermanentZipCode VARCHAR(10),
    @ParentFirstName VARCHAR(50),
    @ParentMiddleName VARCHAR(50),
    @ParentLastName VARCHAR(50),
    @ParentContactNo VARCHAR(15),
    @ParentRelationship VARCHAR(50),
    @GuardianFirstName VARCHAR(50),
    @GuardianMiddleName VARCHAR(50),
    @GuardianLastName VARCHAR(50),
    @GuardianContactNo VARCHAR(15),
    @GuardianRelationship VARCHAR(50),
    @SchoolName VARCHAR(100),
    @SchoolAddress VARCHAR(255),
    @SchoolContact VARCHAR(15),
    @SchoolLandline VARCHAR(8) = NULL,
    @SchoolType VARCHAR(10),
    @YearOfGraduation INT,
    @LRN VARCHAR(12),
    @GWA DECIMAL(4,2),
	@TermsAccepted BIT
AS
BEGIN
    INSERT INTO Admissions (
        GradeLevel, FirstName, MiddleName, LastName, Suffix, Email, DateOfBirth, CivilStatus, Sex, Country, Region, City, 
        Height, Weight, Religion, Disability, Phone_number, Landline_number, Emergency_landline_number, ContactPerson, 
        ContactNumber, Relationship, HouseNo, Barangay, Street, Municipality, Province, ZipCode, PermanentHouseNo, 
        PermanentBarangay, PermanentStreet, PermanentMunicipality, PermanentProvince, PermanentZipCode, ParentFirstName, 
        ParentMiddleName, ParentLastName, ParentContactNo, ParentRelationship, GuardianFirstName, GuardianMiddleName, 
        GuardianLastName, GuardianContactNo, GuardianRelationship, SchoolName, SchoolAddress, SchoolContact, 
        SchoolLandline, SchoolType, YearOfGraduation, LRN, GWA, TermsAccepted
    )
    VALUES (
        @GradeLevel, @FirstName, @MiddleName, @LastName, @Suffix, @Email, @DateOfBirth, @CivilStatus, @Sex, @Country, 
        @Region, @City, @Height, @Weight, @Religion, @Disability, @Phone_number, @Landline_number, @Emergency_landline_number, 
        @ContactPerson, @ContactNumber, @Relationship, @HouseNo, @Barangay, @Street, @Municipality, @Province, @ZipCode, 
        @PermanentHouseNo, @PermanentBarangay, @PermanentStreet, @PermanentMunicipality, @PermanentProvince, @PermanentZipCode, 
        @ParentFirstName, @ParentMiddleName, @ParentLastName, @ParentContactNo, @ParentRelationship, @GuardianFirstName, 
        @GuardianMiddleName, @GuardianLastName, @GuardianContactNo, @GuardianRelationship, @SchoolName, @SchoolAddress, 
        @SchoolContact, @SchoolLandline, @SchoolType, @YearOfGraduation, @LRN, @GWA, @TermsAccepted
    );
END;
EXEC CreateAdmissions;





-- Soft Delete an Admission Record
CREATE PROCEDURE SoftDeleteAdmissions
    @Id INT
AS
BEGIN
    UPDATE Admissions
    SET IsDeleted = 1
    WHERE Id = @Id;
END;
EXEC SoftDeleteAdmissions @Id = 5;





-- Get All Admissions (Excluding Soft Deleted Records)
ALTER PROCEDURE GetAllAdmissions
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
        DateCreated,
        IsDeleted,  
		TermsAccepted-- Ensure this column is included
    FROM Admissions
    WHERE IsDeleted = 0;
END;



EXEC GetAllAdmissions;


USE PUPSIS



-- Get Admission by ID
CREATE PROCEDURE GetByIdAdmissions
    @Id INT
AS
BEGIN
    SELECT * FROM Admissions
    WHERE Id = @Id AND IsDeleted = 0;
END;
EXEC GetByIdAdmissions @Id = 1;







-- Update an Admission Record Using COALESCE
ALTER PROCEDURE UpdateAdmissions
    @Id INT,
    @GradeLevel NVARCHAR(10) = NULL,
    @FirstName VARCHAR(50) = NULL,
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50) = NULL,
    @Suffix VARCHAR(10) = NULL,
    @Email VARCHAR(100) = NULL,
    @DateOfBirth DATE = NULL,
    @CivilStatus VARCHAR(10) = NULL,
    @Sex VARCHAR(10) = NULL,
    @Country VARCHAR(50) = NULL,
    @Region VARCHAR(50) = NULL,
    @City VARCHAR(50) = NULL,
    @Height DECIMAL(5,2) = NULL,
    @Weight DECIMAL(5,2) = NULL,
    @Religion VARCHAR(50) = NULL,
    @Disability VARCHAR(100) = NULL,
    @Phone_number VARCHAR(11) = NULL,
    @Landline_number VARCHAR(8) = NULL,
    @Emergency_landline_number VARCHAR(8) = NULL,
    @ContactPerson VARCHAR(100) = NULL,
    @ContactNumber VARCHAR(15) = NULL,
    @Relationship VARCHAR(50) = NULL,
    @HouseNo VARCHAR(50) = NULL,
    @Barangay VARCHAR(50) = NULL,
    @Street VARCHAR(50) = NULL,
    @Municipality VARCHAR(50) = NULL,
    @Province VARCHAR(50) = NULL,
    @ZipCode VARCHAR(10) = NULL,
    @PermanentHouseNo VARCHAR(50) = NULL,
    @PermanentBarangay VARCHAR(50) = NULL,
    @PermanentStreet VARCHAR(50) = NULL,
    @PermanentMunicipality VARCHAR(50) = NULL,
    @PermanentProvince VARCHAR(50) = NULL,
    @PermanentZipCode VARCHAR(10) = NULL,
    @ParentFirstName VARCHAR(50) = NULL,
    @ParentMiddleName VARCHAR(50) = NULL,
    @ParentLastName VARCHAR(50) = NULL,
    @ParentContactNo VARCHAR(15) = NULL,
    @ParentRelationship VARCHAR(50) = NULL,
    @GuardianFirstName VARCHAR(50) = NULL,
    @GuardianMiddleName VARCHAR(50) = NULL,
    @GuardianLastName VARCHAR(50) = NULL,
    @GuardianContactNo VARCHAR(15) = NULL,
    @GuardianRelationship VARCHAR(50) = NULL,
    @SchoolName VARCHAR(100) = NULL,
    @SchoolAddress VARCHAR(255) = NULL,
    @SchoolContact VARCHAR(15) = NULL,
    @SchoolLandline VARCHAR(8) = NULL,
    @SchoolType VARCHAR(10) = NULL,
    @YearOfGraduation INT = NULL,
    @LRN VARCHAR(12) = NULL,
    @GWA DECIMAL(4,2) = NULL
AS
BEGIN
    UPDATE Admissions
    SET GradeLevel = COALESCE(@GradeLevel, GradeLevel),
        FirstName = COALESCE(@FirstName, FirstName),
        MiddleName = COALESCE(@MiddleName, MiddleName),
        LastName = COALESCE(@LastName, LastName),
        Suffix = COALESCE(@Suffix, Suffix),
        Email = COALESCE(@Email, Email),
        DateOfBirth = COALESCE(@DateOfBirth, DateOfBirth),
        CivilStatus = COALESCE(@CivilStatus, CivilStatus),
        Sex = COALESCE(@Sex, Sex),
        Country = COALESCE(@Country, Country),
        Region = COALESCE(@Region, Region),
        City = COALESCE(@City, City),
        Height = COALESCE(@Height, Height),
        Weight = COALESCE(@Weight, Weight),
        Religion = COALESCE(@Religion, Religion),
        Disability = COALESCE(@Disability, Disability),
        Phone_number = COALESCE(@Phone_number, Phone_number),
        Landline_number = COALESCE(@Landline_number, Landline_number),
        Emergency_landline_number = COALESCE(@Emergency_landline_number, Emergency_landline_number),
        ContactPerson = COALESCE(@ContactPerson, ContactPerson),
        ContactNumber = COALESCE(@ContactNumber, ContactNumber),
        Relationship = COALESCE(@Relationship, Relationship),
        HouseNo = COALESCE(@HouseNo, HouseNo),
        Barangay = COALESCE(@Barangay, Barangay),
        Street = COALESCE(@Street, Street),
        Municipality = COALESCE(@Municipality, Municipality),
        Province = COALESCE(@Province, Province),
        ZipCode = COALESCE(@ZipCode, ZipCode),
        PermanentHouseNo = COALESCE(@PermanentHouseNo, PermanentHouseNo),
        PermanentBarangay = COALESCE(@PermanentBarangay, PermanentBarangay),
        PermanentStreet = COALESCE(@PermanentStreet, PermanentStreet),
        PermanentMunicipality = COALESCE(@PermanentMunicipality, PermanentMunicipality),
        PermanentProvince = COALESCE(@PermanentProvince, PermanentProvince),
        PermanentZipCode = COALESCE(@PermanentZipCode, PermanentZipCode),
        ParentFirstName = COALESCE(@ParentFirstName, ParentFirstName),
        ParentMiddleName = COALESCE(@ParentMiddleName, ParentMiddleName),
        ParentLastName = COALESCE(@ParentLastName, ParentLastName),
        ParentContactNo = COALESCE(@ParentContactNo, ParentContactNo),
        ParentRelationship = COALESCE(@ParentRelationship, ParentRelationship),
        GuardianFirstName = COALESCE(@GuardianFirstName, GuardianFirstName),
        GuardianMiddleName = COALESCE(@GuardianMiddleName, GuardianMiddleName),
        GuardianLastName = COALESCE(@GuardianLastName, GuardianLastName),
        GuardianContactNo = COALESCE(@GuardianContactNo, GuardianContactNo),
        GuardianRelationship = COALESCE(@GuardianRelationship, GuardianRelationship),
        SchoolName = COALESCE(@SchoolName, SchoolName),
        SchoolAddress = COALESCE(@SchoolAddress, SchoolAddress),
        SchoolContact = COALESCE(@SchoolContact, SchoolContact),
        SchoolLandline = COALESCE(@SchoolLandline, SchoolLandline),
        SchoolType = COALESCE(@SchoolType, SchoolType),
        YearOfGraduation = COALESCE(@YearOfGraduation, YearOfGraduation),
        LRN = COALESCE(@LRN, LRN),
        GWA = COALESCE(@GWA, GWA)
    WHERE Id = @Id AND IsDeleted = 0;
END;

EXEC UpdateAdmissions  
    @Id = 1,  
    @GradeLevel = 'Grade 10',  
    @Email = 'new.email@example.com',  
    @Phone_number = '09987654321',  
    @GuardianContactNo = '09112223334',  
    @SchoolContact = '09223334455',  
    @GWA = 88.50;  



EXEC CreateAdmissions
    @GradeLevel = 'Grade 8',
    @FirstName = 'Maria',
    @MiddleName = 'Lopez',
    @LastName = 'Santos',
    @Suffix = NULL,
    @Email = 'maria.santos@example.com',
    @DateOfBirth = '2009-08-20',
    @CivilStatus = 'Single',
    @Sex = 'Female',
    @Country = 'Philippines',
    @Region = 'Region IV-A',
    @City = 'Cavite City',
    @Height = 155.40,
    @Weight = 50.25,
    @Religion = 'Catholic',
    @Disability = NULL,
    @Phone_number = '09234567890',
    @Landline_number = '12345678',
    @Emergency_landline_number = NULL,
    @ContactPerson = 'Juan Santos',
    @ContactNumber = '09987654321',
    @Relationship = 'Father',
    @HouseNo = '456',
    @Barangay = 'San Isidro',
    @Street = 'Mabini St.',
    @Municipality = 'Cavite City',
    @Province = 'Cavite',
    @ZipCode = '4100',
    @PermanentHouseNo = '456',
    @PermanentBarangay = 'San Isidro',
    @PermanentStreet = 'Mabini St.',
    @PermanentMunicipality = 'Cavite City',
    @PermanentProvince = 'Cavite',
    @PermanentZipCode = '4100',
    @ParentFirstName = 'Juan',
    @MaidenName = 'Reyes',
    @ParentLastName = 'Santos',
    @ParentContactNo = '09981234567',
    @ParentRelationship = 'Father',
    @GuardianFirstName = 'Ana',
    @GuardianMiddleName = 'Reyes',
    @GuardianLastName = 'Lopez',
    @GuardianContactNo = '09129876543',
    @GuardianRelationship = 'Aunt',
    @SchoolName = 'Saint Mary’s Academy',
    @SchoolAddress = '123 Private Rd, Cavite City',
    @SchoolContact = '09233456789',
    @SchoolLandline = '87654321',
    @SchoolType = 'Private',
    @YearOfGraduation = 2024,
    @LRN = '234567890123',
    @GWA = 91.50;





