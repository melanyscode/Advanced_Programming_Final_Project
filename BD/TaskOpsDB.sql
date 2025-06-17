-- Create the database
CREATE DATABASE TaskOpsDB;
GO

USE TaskOpsDB;
GO

-- Main table: Tasks
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    TaskName NVARCHAR(100) NOT NULL,
    Priority NVARCHAR(10) CHECK (Priority IN ('High', 'Medium', 'Low')) NOT NULL,
    ExecutionDate DATETIME NOT NULL,
    SimulatedCommand NVARCHAR(500) NOT NULL,
    Status NVARCHAR(15) CHECK (Status IN ('Pending', 'Running', 'Completed', 'Failed')) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME
);
GO

-- Secondary table: Execution Logs
CREATE TABLE ExecutionLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    TaskId INT NOT NULL,
    ExecutionTime DATETIME DEFAULT GETDATE(),
    ExecutionStatus NVARCHAR(15) CHECK (ExecutionStatus IN ('Completed', 'Failed')) NOT NULL,
    OutputMessage NVARCHAR(MAX),
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId) ON DELETE CASCADE
);
GO

-- Indexes for faster queries
CREATE INDEX IDX_Tasks_Status_Priority ON Tasks(Status, Priority);
CREATE INDEX IDX_ExecutionLogs_TaskId ON ExecutionLogs(TaskId);
GO

-- Dummy Data

INSERT INTO Tasks (TaskName, Priority, ExecutionDate, SimulatedCommand, Status)
VALUES
('Update antivirus definitions', 'High', DATEADD(MINUTE, -15, GETDATE()), 'update-av', 'Completed'),
('Backup documents', 'Medium', DATEADD(HOUR, 1, GETDATE()), 'backup-docs', 'Pending'),
('Scan local network', 'High', DATEADD(MINUTE, -30, GETDATE()), 'nmap -sn 192.168.0.0/24', 'Failed'),
('Clean temp files', 'Low', DATEADD(HOUR, 2, GETDATE()), 'clean-temp', 'Pending'),
('Check disk usage', 'Medium', DATEADD(MINUTE, -45, GETDATE()), 'df -h', 'Completed'),
('Restart service', 'High', DATEADD(MINUTE, -10, GETDATE()), 'systemctl restart apache2', 'Running'),
('Generate logs report', 'Low', DATEADD(MINUTE, 20, GETDATE()), 'generate-report --logs', 'Pending'),
('Test network speed', 'Medium', DATEADD(HOUR, -1, GETDATE()), 'speedtest-cli', 'Completed'),
('Monitor CPU usage', 'High', DATEADD(MINUTE, -5, GETDATE()), 'top -b -n1', 'Failed'),
('Reboot server', 'High', DATEADD(DAY, 1, GETDATE()), 'reboot', 'Pending');
