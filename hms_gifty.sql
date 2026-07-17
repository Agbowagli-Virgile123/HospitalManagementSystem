CREATE DATABASE HospitalManagementSystem;
GO

USE HospitalManagementSystem;

-- Staff
CREATE TABLE Staff
(
    Id VARCHAR(100) NOT NULL PRIMARY KEY,

    FirstName VARCHAR(100) NOT NULL,

    LastName VARCHAR(100) NOT NULL,

    Username VARCHAR(50) NOT NULL,

    Password VARCHAR(255) NOT NULL,

    Email VARCHAR(150) NOT NULL,

    Phone VARCHAR(20) NOT NULL,

    Position VARCHAR(100) NOT NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL
);

--INSERT INTO Staff
--(
--    Id,
--    FirstName,
--    LastName,
--    Username,
--    Password,
--    Email,
--    Phone,
--    Position
--)
--VALUES
--(
--    '100',
--    'System',
--    'Administrator',
--    'admin',
--    '123',
--    'admin@hospital.com',
--    '0550000000',
--    'Administrator'
--);

-- Department
CREATE TABLE Department
(
    Id VARCHAR(150) PRIMARY KEY,

    DeptName VARCHAR(100) NOT NULL,

    DeptDescription VARCHAR(255),

    DeptLocation VARCHAR(100),

    DeptContact VARCHAR(50),

    HeadDoctorId VARCHAR(150) NULL,

    DeptStatus VARCHAR(20) NOT NULL DEFAULT 'Active',

    CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),

    UpdatedDate DATETIME NULL,

    CONSTRAINT FK_Department_Doctor
        FOREIGN KEY (HeadDoctorId)
        REFERENCES Doctors(Id)
);



-- Doctors
CREATE TABLE Doctors
(
    Id VARCHAR(150) PRIMARY KEY,

    FirstName NVARCHAR(100) NOT NULL,

    LastName NVARCHAR(100) NOT NULL,

    Gender NVARCHAR(10),

    Specialization NVARCHAR(100) NOT NULL,

    PhoneNumber NVARCHAR(20),

    Email NVARCHAR(150),

    ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0,

    IsAvailable BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL
);
GO

-- Patients
CREATE TABLE Patients
(
    Id VARCHAR(150) PRIMARY KEY,

    FirstName NVARCHAR(100) NOT NULL,

    LastName NVARCHAR(100) NOT NULL,

    Gender NVARCHAR(10),

    DateOfBirth DATE,

    PhoneNumber NVARCHAR(20),

    Email NVARCHAR(150),

    Address NVARCHAR(300),

    EmergencyContactName NVARCHAR(150),

    EmergencyContactPhone NVARCHAR(20),

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL
);
GO

-- Appointments
CREATE TABLE Appointments
(
    Id VARCHAR(150) PRIMARY KEY,

    PatientId VARCHAR(150) NOT NULL,

    DoctorId VARCHAR(150) NOT NULL,

    AppointmentDate DATE NOT NULL,

    AppointmentTime TIME NOT NULL,

    Reason NVARCHAR(500),

    Status NVARCHAR(30) NOT NULL DEFAULT 'Pending',

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_Appointment_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_Appointment_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);
GO

-- Medical Records
CREATE TABLE MedicalRecords
(
    Id VARCHAR(150) PRIMARY KEY,

    PatientId VARCHAR(150) NOT NULL,

    DoctorId VARCHAR(150) NOT NULL,

    AppointmentId VARCHAR(150) NOT NULL,

    Diagnosis NVARCHAR(MAX),

    Treatment NVARCHAR(MAX),

    Notes NVARCHAR(MAX),

    VisitDate DATETIME NOT NULL DEFAULT GETDATE(),

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_MedicalRecord_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_MedicalRecord_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id),

    CONSTRAINT FK_MedicalRecord_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id)
);
GO

-- Medication
CREATE TABLE Medications
(
    Id VARCHAR(150) PRIMARY KEY,

    Name NVARCHAR(150) NOT NULL,

    Description NVARCHAR(500),

    UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,

    QuantityInStock INT NOT NULL DEFAULT 0,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL
);
GO

-- Prescriptions
CREATE TABLE Prescriptions
(
    Id VARCHAR(150) PRIMARY KEY,

    MedicalRecordId VARCHAR(150) NOT NULL,

    MedicationId VARCHAR(150) NOT NULL,

    Dosage NVARCHAR(100),

    Frequency NVARCHAR(100),

    Duration NVARCHAR(100),

    Instructions NVARCHAR(500),

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_Prescription_MedicalRecord
        FOREIGN KEY (MedicalRecordId)
        REFERENCES MedicalRecords(Id),

    CONSTRAINT FK_Prescription_Medication
        FOREIGN KEY (MedicationId)
        REFERENCES Medications(Id)
);
GO

-- Billings
CREATE TABLE Billing
(
    Id VARCHAR(150) PRIMARY KEY,

    PatientId VARCHAR(150) NOT NULL,

    AppointmentId VARCHAR(150) NOT NULL,

    ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0,

    MedicationFee DECIMAL(18,2) NOT NULL DEFAULT 0,

    OtherCharges DECIMAL(18,2) NOT NULL DEFAULT 0,

    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,

    PaymentMethod NVARCHAR(50),

    PaymentStatus NVARCHAR(30) NOT NULL DEFAULT 'Pending',

    BillDate DATETIME NOT NULL DEFAULT GETDATE(),

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_Billing_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_Billing_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id)
);
GO

-- Audit Logs
CREATE TABLE AuditLogs
(
    Id VARCHAR(150) PRIMARY KEY,

    StaffId VARCHAR(100) NOT NULL,

    Action NVARCHAR(50) NOT NULL,

    TableName NVARCHAR(100) NOT NULL,

    RecordId INT NOT NULL,

    Description NVARCHAR(500),

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AuditLog_Staff
        FOREIGN KEY (StaffId)
        REFERENCES Staff(Id)
);
GO


-- Doctor table alter
ALTER TABLE Doctors
ADD
    DepartmentId VARCHAR(150) NOT NULL,
    LicenseNumber VARCHAR(150) NOT NULL,
    CONSTRAINT FK_Doctors_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES Department(Id);

-- Insert sample data into Department table
INSERT INTO Department (Id, DeptName, DeptDescription, DeptLocation, DeptContact, HeadDoctorId)
VALUES
('D001', 'Cardiology', 'Heart-related treatments and surgeries', 'Building A, Floor 2', '0551234567', NULL),
('D002', 'Neurology', 'Brain and nervous system treatments', 'Building B, Floor 3', '0552345678', NULL),
('D003', 'Pediatrics', 'Child healthcare services', 'Building C, Floor 1', '0553456789', NULL),
('D004', 'Orthopedics', 'Bone and joint treatments and surgeries', 'Building D, Floor 4', '0554567890', NULL),
('D005', 'Dermatology', 'Skin-related treatments and consultations', 'Building E, Floor 2', '0555678901', NULL);

--Add Date of Birth to Patient
ALTER TABLE Patients
ADD
       BloodGroup VARCHAR(50) NULL,
       MaritalStatus VARCHAR(50) NOT NULL,
       City VARCHAR(50) NULL,
       Country VARCHAR(50) NULL,
       Nationality VARCHAR(50) NULL,
       AllergyDescription VARCHAR(MAX) NULL,
       Status VARCHAR(50) NOT NULL DEFAULT 'Active'

-- Insert sample data into Patients table
INSERT INTO Patients
(
    Id,
    FirstName,
    LastName,
    Gender,
    DateOfBirth,
    PhoneNumber,
    Email,
    Address,
    EmergencyContactName,
    EmergencyContactPhone,
    BloodGroup,
    MaritalStatus,
    City,
    Country,
    Nationality,
    AllergyDescription,
    Status
)
VALUES
('PAT001', 'John', 'Doe', 'Male', '1988-05-15', '+1-555-1001', 'john.doe@email.com',
 '123 Maple Street', 'Jane Doe', '+1-555-2001',
 'O+', 'Married', 'New York', 'USA', 'American', 'Penicillin allergy', 'Active'),

('PAT002', 'Mary', 'Johnson', 'Female', '1992-08-22', '+1-555-1002', 'mary.johnson@email.com',
 '45 Oak Avenue', 'Peter Johnson', '+1-555-2002',
 'A+', 'Single', 'Chicago', 'USA', 'American', 'None', 'Active'),

('PAT003', 'David', 'Williams', 'Male', '1979-12-10', '+1-555-1003', 'david.williams@email.com',
 '89 Pine Road', 'Sarah Williams', '+1-555-2003',
 'B+', 'Married', 'Houston', 'USA', 'American', 'Peanut allergy', 'Inactive'),

('PAT004', 'Linda', 'Brown', 'Female', '1985-04-18', '+1-555-1004', 'linda.brown@email.com',
 '67 Cedar Lane', 'Michael Brown', '+1-555-2004',
 'AB+', 'Married', 'Phoenix', 'USA', 'American', 'Latex allergy', 'Active'),

('PAT005', 'James', 'Miller', 'Male', '1995-09-30', '+1-555-1005', 'james.miller@email.com',
 '901 Birch Drive', 'Emily Miller', '+1-555-2005',
 'O-', 'Single', 'Dallas', 'USA', 'American', NULL, 'Active'),

('PAT006', 'Sophia', 'Davis', 'Female', '2000-01-12', '+1-555-1006', 'sophia.davis@email.com',
 '222 Elm Street', 'Olivia Davis', '+1-555-2006',
 'A-', 'Single', 'Seattle', 'USA', 'American', 'Dust allergy', 'Active'),

('PAT007', 'Daniel', 'Wilson', 'Male', '1983-07-08', '+1-555-1007', 'daniel.wilson@email.com',
 '456 Walnut Street', 'Emma Wilson', '+1-555-2007',
 'B-', 'Divorced', 'Boston', 'USA', 'American', 'Shellfish allergy', 'Active'),

('PAT008', 'Emma', 'Moore', 'Female', '1998-11-25', '+1-555-1008', 'emma.moore@email.com',
 '78 Spruce Avenue', 'Noah Moore', '+1-555-2008',
 'AB-', 'Single', 'Denver', 'USA', 'American', NULL, 'Active'),

('PAT009', 'Michael', 'Taylor', 'Male', '1975-03-05', '+1-555-1009', 'michael.taylor@email.com',
 '145 Aspen Court', 'Laura Taylor', '+1-555-2009',
 'O+', 'Married', 'Miami', 'USA', 'American', 'Diabetes', 'Inactive'),

('PAT010', 'Olivia', 'Anderson', 'Female', '1990-06-14', '+1-555-1010', 'olivia.anderson@email.com',
 '333 Cherry Boulevard', 'Ethan Anderson', '+1-555-2010',
 'A+', 'Married', 'San Francisco', 'USA', 'American', 'None', 'Active');

 ALTER TABLE Appointments
ADD
    DepartmentId VARCHAR(150) NOT NULL,
Reason VARCHAR(MAX) NULL,
Note VARCHAR(MAX) NULL,
    CONSTRAINT FK_Doctors_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES Department(Id);