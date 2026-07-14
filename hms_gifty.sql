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

-- Doctor table alter
ALTER TABLE Doctors
ADD
    DepartmentId VARCHAR(150) NOT NULL,
    LicenseNumber VARCHAR(150) NOT NULL,
    CONSTRAINT FK_Doctors_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES Department(Id);


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