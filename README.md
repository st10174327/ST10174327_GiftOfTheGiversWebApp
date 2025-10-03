

# GiftOfTheGivers Web Application

A web-based donation management system developed using **ASP.NET Core 8.0**, **Entity Framework Core**, and **Identity**. This application allows administrators and users to manage money and goods donations, track disasters, and maintain donation records securely.


Default Admin Login
	•	Email: admin@admin.com
	•	Password: Admin@123


---
Gift of the Givers Web Application

Student: ST10174327 Cynthia Panzu
Module: Applied Programming 


Project Overview

The Gift of the Givers Web Application is an ASP.NET Core–based humanitarian platform that enables disaster relief operations through a structured digital solution. Users can register, log in, report disasters, donate goods, donate money, and sign up as volunteers. The application also features an Admin dashboard, allowing administrators to manage volunteers, create and assign tasks, and monitor disaster relief activities.

The system was developed with a strong focus on Azure DevOps integration, utilizing Git repositories, branching strategies, and CI/CD pipelines.

Azure DevOps Repository:
GiftOfTheGivers-WebApp (Azure DevOps)


Git Repositories

Repository Setup
	•	Repository Created: GiftOfTheGivers-WebApp
	•	Branching Strategy: Gitflow model implemented
	•	Branches:
	•	main (production)
	•	develop (integration)
	•	feature/volunteer-management
	•	feature/donation-system
	•	feature/user-authentication

Collaboration Features
	•	Public repository access enabled for instructor review.
	•	Organized and structured ASP.NET Core solution with clear separation of concerns.

Evidence of Work (Commits, Branches, Repo Structure):
	•	Screenshots of commits and branch hierarchy are included in the submission.

Source Code Quality

Project Architecture
	•	ASP.NET Core MVC pattern implemented.
	•	Structured folder hierarchy ensuring maintainability and scalability.

Build Pipelines

Pipeline Configuration
	•	CI/CD implemented using Azure Pipelines with YAML configuration.
	•	Pipeline stages:
	1.	Restore NuGet packages
	2.	Build solution
	3.	Run unit tests
	4.	Publish build artifacts
	5.	Deploy to Azure App Service

Challenge: Encountered Parallelism Limitation on the free tier of Azure DevOps.
	•	Action Taken: Submitted an official request for additional parallelism capacity.
	•	Outcome: Build pipeline configuration validated, but execution blocked due to free-tier restrictions.

Web Application Features & Screenshots

Public Pages
	•	Home Page: Navigation bar with disaster logging, donation options, login/register, and footer.
	•	About Us Page: Displays mission, philosophy, and history with donation prompt.
	•	Contact Page: Embedded map and contact form.
	•	Register/Login Pages: Secure authentication for users and admin access.

User Functionality
	•	Report disasters (specifying type and location).
	•	Donate goods or money through dedicated forms.
	•	Register as a volunteer to participate in relief programs.
	•	User Dashboard to track personal activity.

Admin Functionality
	•	Admin Dashboard: Provides overview of volunteer activities and statistics.
	•	Task Management: Admins can create, assign, and monitor volunteer tasks.
	•	Volunteer Management: Approve and manage volunteer registrations.


Pipelines Evidence
	•	YAML configuration file created and linked to project.
	•	Documentation of failed build logs due to Parallelism Limitation.

References
	•	Microsoft Docs – ASP.NET Core MVC
	•	Microsoft Docs – ASP.NET Identity
	•	Entity Framework Core – EF Core Documentation
	•	Bootstrap 5 – Bootstrap Documentation
	•	Azure DevOps – Pipelines

## Table of Contents
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

---

## Features
- **User Authentication & Roles**  
  - Admin and User roles using ASP.NET Identity.  
  - Secure login and registration with password policies.  
- **Money Donations**  
  - Record donations with date, amount, and donor information.  
  - Track remaining funds and validate donation entries.  
- **Goods Donations & Purchases**  
  - Manage inventory of donated goods.  
  - Purchase and allocate goods while maintaining stock levels.  
  - Automatic calculation of total purchase price.  
- **Disaster Management**  
  - Create, edit, view, and delete disaster events.  
  - Track disaster start/end dates, aid type, and active status.  
- **Reports & Validation**  
  - Server-side and client-side validation for all forms.  
  - Detailed views for donations, goods, and disaster events.  

---

## Technology Stack
- **Backend:** ASP.NET Core 8.0, C#  
- **Frontend:** Razor Pages, Bootstrap 5  
- **Database:** SQL Server, Entity Framework Core  
- **Authentication:** ASP.NET Core Identity  
- **Tools:** Visual Studio 2022, .NET CLI, EF Core Tools  

---

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/GiftOfTheGiversWebApp.git
   cd GiftOfTheGiversWebApp/ST10174327_GiftOfTheGiversWebApp
````

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Build the project:

   ```bash
   dotnet build
   ```

---

## Database Setup

1. Update your `appsettings.json` with a valid SQL Server connection string:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=GiftOfTheGivers;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. Create the database using Entity Framework migrations:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

> **Note:** Make sure you are in the project directory containing the `.csproj` file.

---

## Usage

1. Run the application:

   ```bash
   dotnet run
   ```

2. Open your browser and navigate to:

   ```
   https://localhost:5001
   ```

3. Log in as Admin using the seeded account:

   * **Email:** `admin@admin.com`
   * **Password:** `Test1234!`

4. Explore features:

   * Create and manage money donations.
   * Record goods donations and purchases.
   * Track disasters and allocate aid.
   * View reports and inventory status.

---

## Project Structure

```
ST10174327_GiftOfTheGiversWebApp/
├── Controllers/       # MVC controllers
├── Data/              # ApplicationDbContext and migrations
├── Models/            # Data models (Disaster, GoodsDonation, GoodsPurchase, Volunteer, etc.)
├── Views/             # Razor views for each controller
├── wwwroot/           # Static files (CSS, JS, images)
├── appsettings.json   # Application configuration
└── ST10174327_GiftOfTheGiversWebApp.csproj
```

---

## Contributing

1. Fork the repository
2. Create a new branch:

   ```bash
   git checkout -b feature/YourFeature
   ```
3. Commit your changes:

   ```bash
   git commit -m "Add new feature"
   ```
4. Push your branch:

   ```bash
   git push origin feature/YourFeature
   ```
5. Create a Pull Request

---

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---

## Author

**Cynthia Panzu**
IIE Rosebank College - Information Technology (Software Development)

---

## Notes

* All forms include both client-side and server-side validation.
* Admins have full access to all CRUD operations; regular users can only view their own disaster reports.
* Inventory and donation tracking is automatically calculated to prevent over-purchasing or over-allocation.
* Ensure proper connection strings and database configuration before running migrations.

```

---


```
