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

```bash
cd backend/ProjectManagement.Api
dotnet restore
dotnet run
```

API starts with Swagger in Development mode.

Default seeded admin user:

- Email: `admin@system.local`
- Password: `Admin@123`

> Update `appsettings.json` with your real SQL Server connection and JWT key before deployment.

## Frontend setup

```bash
cd frontend
npm install
npm run dev
```

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
