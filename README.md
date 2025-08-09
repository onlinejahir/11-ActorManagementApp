# Actor Management App

A full-featured ASP.NET Core MVC web application for managing actors, biographies, movies, and genres — with integrated authentication, role-based access control, file uploads, and Entity Framework Core.

---

## Features

### ✅ Core Functionality
- 🔹 **CRUD operations** for:
  - Actors
  - Biographies (One-to-One)
  - Movies (Many-to-Many)
  - Genres
- 🔹 Strong use of **Entity Framework Core** with code-first approach and migrations
- 🔹 Structured into **Layered Architecture**:
  - Presentation (UI)
  - Business/Service Layer
  - Data Access Layer
  - Entity Models

### 🔐 Authentication & Authorization
- ASP.NET Core Identity integration with custom `AppUser`
- Role-based access control (`Admin`, `Manager`, `User`)
- Claim-based policy support (optional)
- Role and user **seeding on application startup**
- Protected controller actions with `[Authorize]` and `[Authorize(Roles = "Admin")]`

### 📁 File Uploads
- Upload image files for Actors and Biographies
- Uploaded files stored in the `wwwroot` folder
- **File download support** (e.g., download PDFs or actor bios)
- Validations for file types and size

### 📊 UI Enhancements
- Integrated **jQuery DataTables** for sortable, searchable, and paginated tables (e.g., list of actors or movies)
- Responsive design with Bootstrap

### 👤 Admin Panel
- Admin dashboard (accessible only to users with `Admin` role)
- View list of all users and their roles
- Role-based menu visibility (`Admin`, `Manager`)

## 🛠 Technologies Used
- ASP.NET Core MVC (.NET 7/8)
- Entity Framework Core (Code-First)
- ASP.NET Core Identity
- SQL Server
- Bootstrap + jQuery + jQuery DataTables
- File Upload & Download
- Dependency Injection
- Clean MVC + Service + Repository layers

---

## 🧪 Getting Started

### Prerequisites
- Visual Studio 2022 or later
- .NET SDK 7 or 8
- SQL Server LocalDB or SQL Express
