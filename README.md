GiftOfTheGivers Web Application

A web-based donation management system developed using ASP.NET Core 8.0, Entity Framework Core, and Identity. This application allows administrators and users to manage money and goods donations, track disasters, and maintain donation records securely.

Default Admin Login

Email: admin@admin.com

Password: Admin@123

Gift of the Givers Web Application

Student: ST10174327 Cynthia Panzu
Module: Applied Programming

Project Overview

The Gift of the Givers Web Application is an ASP.NET Core–based humanitarian platform that enables disaster relief operations through a structured digital solution. Users can register, log in, report disasters, donate goods, donate money, and sign up as volunteers. The application also features an Admin dashboard, allowing administrators to manage volunteers, create and assign tasks, and monitor disaster relief activities.

The system was developed with a strong focus on Azure DevOps integration, utilizing Git repositories, branching strategies, and CI/CD pipelines.

Azure DevOps Repository: GiftOfTheGivers-WebApp (Azure DevOps)

Git Repositories

Repository Setup

Repository Created: GiftOfTheGivers-WebApp

Branching Strategy: Gitflow model implemented

Branches:

main (production)

develop (integration)

feature/volunteer-management

feature/donation-system

feature/user-authentication

Collaboration Features

Public repository access enabled for instructor review.

Organized and structured ASP.NET Core solution with clear separation of concerns.

Evidence of Work (Commits, Branches, Repo Structure)

Screenshots of commits and branch hierarchy are included in the submission.

Source Code Quality

Project Architecture

ASP.NET Core MVC pattern implemented.

Structured folder hierarchy ensuring maintainability and scalability.

Build Pipelines

CI/CD implemented using Azure Pipelines with YAML configuration.

Pipeline stages:

Restore NuGet packages

Build solution

Run unit tests

Publish build artifacts

Deploy to Azure App Service

Challenge: Encountered Parallelism Limitation on the free tier of Azure DevOps.

Action Taken: Submitted an official request for additional parallelism capacity.

Outcome: Build pipeline configuration validated, but execution blocked due to free-tier restrictions.

Web Application Features & Screenshots

Public Pages

Home Page: Navigation bar with disaster logging, donation options, login/register, and footer.

About Us Page: Displays mission, philosophy, and history with donation prompt.

Contact Page: Embedded map and contact form.

Register/Login Pages: Secure authentication for users and admin access.

User Functionality

Report disasters (specifying type and location).

Donate goods or money through dedicated forms.

Register as a volunteer to participate in relief programs.

User Dashboard to track personal activity.

Admin Functionality

Admin Dashboard: Provides overview of volunteer activities and statistics.

Task Management: Admins can create, assign, and monitor volunteer tasks.

Volunteer Management: Approve and manage volunteer registrations.

Unit Tests

Unit tests implemented using xUnit and Moq.

Tests cover critical functionalities including:

Donation creation and validation (money and goods)

Volunteer registration and task assignment logic

Disaster reporting and retrieval

Controller action results and view models

Can be run locally with:

dotnet test


All tests are included in the GiftOfTheGiversWebApp.Tests project.

Technology Stack

Backend: ASP.NET Core 8.0, C#

Frontend: Razor Pages, Bootstrap 5

Database: SQL Server, Entity Framework Core

Authentication: ASP.NET Core Identity

Testing: xUnit, Moq

Tools: Visual Studio 2022, .NET CLI, EF Core Tools

Installation

Clone the repository:

git clone https://github.com/yourusername/GiftOfTheGiversWebApp.git
cd GiftOfTheGiversWebApp/ST10174327_GiftOfTheGiversWebApp


Restore dependencies:

dotnet restore


Build the project:

dotnet build

Database Setup

Update appsettings.json with a valid SQL Server connection string:

"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=GiftOfTheGivers;Trusted_Connection=True;MultipleActiveResultSets=true"
}


Create the database using Entity Framework migrations:

dotnet ef migrations add InitialCreate
dotnet ef database update


Note: Make sure you are in the project directory containing the .csproj file.

Usage

Run the application:

dotnet run


Open your browser and navigate to:

https://localhost:5001


Log in as Admin using the seeded account:

Email: admin@admin.com

Password: Test1234!

Explore features:

Create and manage money donations.

Record goods donations and purchases.

Track disasters and allocate aid.

View reports and inventory status.

Run unit tests for validation:

dotnet test

Project Structure
ST10174327_GiftOfTheGiversWebApp/
├── Controllers/       # MVC controllers
├── Data/              # ApplicationDbContext and migrations
├── Models/            # Data models (Disaster, GoodsDonation, GoodsPurchase, Volunteer, etc.)
├── Views/             # Razor views for each controller
├── wwwroot/           # Static files (CSS, JS, images)
├── Tests/             # Unit tests using xUnit
├── appsettings.json   # Application configuration
└── ST10174327_GiftOfTheGiversWebApp.csproj

Contributing

Fork the repository

Create a new branch:

git checkout -b feature/YourFeature


Commit your changes:

git commit -m "Add new feature"


Push your branch:

git push origin feature/YourFeature


Create a Pull Request

License

This project is licensed under the MIT License. See LICENSE
 for details.

Author

Cynthia Panzu
IIE Rosebank College - Information Technology (Software Development)

Notes

All forms include both client-side and server-side validation.

Admins have full access to all CRUD operations; regular users can only view their own disaster reports.

Inventory and donation tracking is automatically calculated to prevent over-purchasing or over-allocation.

Unit tests provide confidence in the correctness of core features.

Ensure proper connection strings and database configuration before running migrations.
