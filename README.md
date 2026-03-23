<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
# Project Management System (React + .NET API + MSSQL)

This repository provides a full-stack starter that can be used to build a portal web application from scratch, including configurable header/footer, role-based access, login/register, and per-customer portal design management.

## Tech Stack

- **Frontend:** React (Vite)
- **Backend:** ASP.NET Core Web API (.NET 8)
- **Database:** Microsoft SQL Server (EF Core)
- **Auth:** JWT + Role-based authorization

## Roles implemented

- **Admin** — all access
- **Portal-Admin** — manage portal for employee/customer
- **Portal-Employee** — manage customer scope
- **Customer-Admin** — manage employee and user
- **Customer-Employee** — can edit portal design/content
- **Customer-User** — normal login/register usage

## Backend setup
=======
=======
>>>>>>> theirs
=======
>>>>>>> theirs
# Project Management System

A more complete **React JS + .NET 8 Web API + MSSQL** starter for building portal-style web applications where header, footer, homepage content, menu links, user access, and customer-specific portal branding can be managed from one admin experience.

## What this project now includes

- **React frontend** with:
  - login and self-registration
  - dashboard experience for portal administration
  - portal designer with live preview
  - role matrix
  - customer directory view
  - managed user creation view
- **.NET 8 API** with:
  - JWT authentication
  - role-based authorization policies
  - MSSQL-ready EF Core data layer
  - seeded demo users and starter tenant portals
- **Portal configuration model** for:
  - header title
  - footer text
  - primary/secondary colors
  - hero title/subtitle
  - support email
  - announcement bar
  - navigation links
  - homepage content sections

## Role mapping

| Role | Purpose |
| --- | --- |
| Admin | Full platform access |
| Portal-Admin | Manage employee and customer portal layout/branding |
| Portal-Employee | Manage customer operational visibility |
| Customer-Admin | Manage employee and user accounts inside one customer |
| Customer-Employee | Update their own portal design/content |
| Customer-User | Standard portal login/register usage |

## Backend structure

- `backend/ProjectManagement.Api/Controllers`
  - `AuthController` for login/register
  - `PortalDesignController` for viewing/updating portal configuration
  - `UsersController` for managed user creation/listing
  - `CustomersController` for tenant summaries
- `backend/ProjectManagement.Api/Data`
  - `AppDbContext`
  - `DbSeeder`
- `backend/ProjectManagement.Api/Models`
  - user, role, portal design, navigation, and section models
- `backend/ProjectManagement.Api/Dtos`
  - auth, portal, user, and customer DTOs

## Frontend structure

- `frontend/src/pages`
  - `AuthPage`
  - `Dashboard`
- `frontend/src/components`
  - `PortalDesigner`
  - `RoleMatrix`
  - `UserManagement`
  - `CustomerDirectory`
- `frontend/src/context`
  - `AuthContext`

## Local development

### Backend
<<<<<<< ours
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
=======
>>>>>>> theirs

```bash
cd backend/ProjectManagement.Api
dotnet restore
dotnet run
```

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
API starts with Swagger in Development mode.

Default seeded admin user:

- Email: `admin@system.local`
- Password: `Admin@123`

> Update `appsettings.json` with your real SQL Server connection and JWT key before deployment.

## Frontend setup
=======
### Frontend
>>>>>>> theirs

```bash
cd frontend
=======
=======
>>>>>>> theirs
### Frontend

```bash
cd frontend
cp .env.example .env
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
npm install
npm run dev
```

<<<<<<< ours
<<<<<<< ours
<<<<<<< ours
Frontend expects API at `http://localhost:5000/api`.

## Core endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/portaldesign/me`
- `PUT /api/portaldesign/{customerCode}`

## What this starter demonstrates

- JWT authentication and role claims
- Fine-grained role authorization policies
- Centralized portal builder for header/footer/theme content
- Role matrix and dashboard view in React
=======
Frontend is configured to call the API at `http://localhost:5000/api`.
=======
Frontend defaults to `VITE_API_BASE_URL=/api`, and the included Vite proxy forwards API calls to `http://localhost:5000` for local development. You can override that with a custom `frontend/.env` file if needed.
>>>>>>> theirs
=======
Frontend defaults to `VITE_API_BASE_URL=/api`, and the included Vite proxy forwards API calls to `http://localhost:5000` for local development. You can override that with a custom `frontend/.env` file if needed.
>>>>>>> theirs

## Demo accounts seeded

| Email | Password | Role |
| --- | --- | --- |
| `admin@system.local` | `Admin@123` | `Admin` |
| `customeradmin@acme.local` | `Admin@123` | `Customer-Admin` |

## Important configuration

Update the values in `backend/ProjectManagement.Api/appsettings.json` before real deployment:

- SQL Server connection string
- JWT signing key
- issuer/audience values as needed

## Notes

- This starter currently bootstraps the database with `Database.EnsureCreated()` for quick local setup.
<<<<<<< ours
<<<<<<< ours
- For production, you should add EF Core migrations and environment-specific configuration.
>>>>>>> theirs
=======
- The backend launch profile binds to `http://localhost:5000` and `https://localhost:7001` so the frontend proxy and API use matching local ports out of the box.
- For production, you should add EF Core migrations and environment-specific configuration.
>>>>>>> theirs
=======
- The backend launch profile binds to `http://localhost:5000` and `https://localhost:7001` so the frontend proxy and API use matching local ports out of the box.
- For production, you should add EF Core migrations and environment-specific configuration.
>>>>>>> theirs
