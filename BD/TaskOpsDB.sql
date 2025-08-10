-- Create the database

CREATE DATABASE TaskOpsDB;
GO

USE TaskOpsDB;
GO

CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    TaskName NVARCHAR(100) NOT NULL,
    Priority NVARCHAR(10) CHECK (Priority IN ('High', 'Medium', 'Low')) NOT NULL,
    Executable NVARCHAR(255) NOT NULL, 
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME
);
GO

CREATE TABLE [User] (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'Operator')) NOT NULL
);
GO


CREATE TABLE UserTask (
    UserTaskId INT PRIMARY KEY IDENTITY(1,1),
    TaskId INT NOT NULL,
    UserId INT NOT NULL,
    Status NVARCHAR(15) CHECK (Status IN ('Pending', 'Running', 'Completed', 'Failed')) NOT NULL DEFAULT 'Pending',
    Result NVARCHAR(MAX),
    ExecutionDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE
);
GO

CREATE TABLE ExecutionLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    TaskId INT NOT NULL,
    ExecutionTime DATETIME DEFAULT GETDATE(),
    ExecutionStatus NVARCHAR(15) CHECK (ExecutionStatus IN ('Completed', 'Failed')) NOT NULL,
    OutputMessage NVARCHAR(MAX),
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId) ON DELETE CASCADE
);
GO

CREATE INDEX IDX_Tasks_Priority ON Tasks(Priority);
CREATE INDEX IDX_ExecutionLogs_TaskId ON ExecutionLogs(TaskId);
CREATE INDEX IDX_UserTask_UserId_TaskId ON UserTask(UserId, TaskId);
GO

INSERT INTO Tasks (TaskName, Priority, Executable, CreatedAt, UpdatedAt)
VALUES 
(
    N'Monitor CPU Usage',
    'High',
    N'powershell.exe -Command "Get-Counter -Counter ''\Processor(_Total)\% Processor Time''"',
    GETDATE(),
    NULL
),
(
    N'Clean Temp Folder',
    'Medium',
    N'powershell.exe -Command "Remove-Item -Path C:\Temp\* -Force -Recurse"',
    GETDATE(),
    NULL
),
(
    N'Check Disk Space',
    'Low',
    N'powershell.exe -Command "Get-PSDrive -PSProvider FileSystem | Select-Object Name,Free,Used"',
    GETDATE(),
    NULL
);


