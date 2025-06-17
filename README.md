# Advanced_Programming_Final_Project
Final Project for the course Advanced Programming, IIQ 2025.

# Virtual Task Monitor

**Virtual Task Monitor** is a backend-driven task execution and monitoring system built with ASP.NET Core MVC. It facilitates scheduling, execution, and monitoring of infrastructure operations by simulating or running shell commands through a web interface. This system models realistic operational behavior suitable for cloud services, IoT devices, or virtualized environments.

---

## Purpose

This project aims to simulate the operations of a virtual infrastructure monitoring tool. Although primarily intended for academic and local use, it supports real command execution and background processing to emulate production-grade service automation.

---

## Architecture Overview

| Aspect              | Details                                                      |
|---------------------|--------------------------------------------------------------|
| **Platform**        | ASP.NET Core MVC (.NET)                                       |
| **Execution Model** | Asynchronous background service utilizing `Task` and `Thread`|
| **Persistence**     | SQL Server via Entity Framework Core                         |
| **Design Patterns** | Command, Strategy, Factory, Repository                       |
| **Command Support** | Execution of Shell, PowerShell, and CMD commands via `System.Diagnostics.Process` |

---

## Core Functionalities

### 1. Task Lifecycle Management

Tasks are the core entities managing infrastructure operations.

- Creation, modification, and deletion of tasks are supported.
- Task metadata includes:  
  - Name  
  - Priority (High, Medium, Low)  
  - Scheduled execution time  
  - Command definition (simulated or real)
- Once a task has started execution, it becomes immutable.
- Failed tasks can be retried manually from the interface.

---

### 2. Prioritized Queue Execution

Task scheduling is handled through a prioritized queue system:

- Priority levels: High > Medium > Low.
- FIFO policy maintained within each priority group.
- Tasks execute sequentially, with a minimum delay between executions to simulate realistic operation timing.

---

### 3. Execution Engine

Task execution is managed by a background service that operates asynchronously and independently of the UI thread:

- Supports simulated command output or actual system-level command invocation.
- Execution results are timestamped and logged, tracking status (Pending, In Progress, Completed, Failed).
- Logs capture output and errors for auditing and troubleshooting.

---

### 4. Monitoring and Logging

A centralized dashboard provides visibility and control:

- Overview of task counts by status.
- Access to detailed execution logs.
- Filter and sort capabilities by priority, status, and date.
- Notifications inform users of task completions or failures.

---

## Technical Specifications

### Framework and Tooling

| Component              | Description                                           |
|------------------------|-------------------------------------------------------|
| ASP.NET Core MVC       | Web application framework                             |
| Entity Framework Core  | ORM for SQL Server database access                    |
| Hosted Services (`IHostedService`) | Background service execution for tasks         |
| `System.Diagnostics.Process` | Command-line execution interface                  |
| Optional Logging       | Serilog or NLog for structured application logging   |

---

### Design Patterns Applied

| Pattern          | Purpose                                                   |
|------------------|-----------------------------------------------------------|
| **Command**      | Encapsulates execution logic for different task types     |
| **Strategy**     | Selects execution mode dynamically (simulation vs real)  |
| **Factory**      | Creates task instances based on their execution type      |
| **Repository**   | Abstracts data access layer for tasks and logs            |

---

### SOLID Principles

| Principle             | Application Detail                                      |
|-----------------------|--------------------------------------------------------|
| **Single Responsibility** | Separation of concerns across controllers, services, repositories |
| **Open/Closed**           | Extendable task behavior without modifying core code |
| **Liskov Substitution**   | Unified interface for simulated and real task types   |
| **Interface Segregation** | Fine-grained interfaces tailored to component roles   |
| **Dependency Inversion**  | Dependency injection throughout the application       |

---

> **Note:** This project is under active development. Features and architectural details may evolve as implementation progresses.
