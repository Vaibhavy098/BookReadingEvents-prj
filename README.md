# 📚 Book Reading Events Management System

> A production-ready ASP.NET Core Web API for creating, managing, and participating in book reading events — built with N-Layer Architecture, JWT Authentication, and Role-Based Access Control.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Database Design](#database-design)
- [API Endpoints](#api-endpoints)
- [Authentication Flow](#authentication-flow)
- [Validation Rules](#validation-rules)
- [Exception Handling](#exception-handling)
- [Unit Testing](#unit-testing)
- [Database Backup](#Database Backup)
- [Setup Instructions](#setup-instructions)
- [Swagger Usage](#swagger-usage)
- [Assumptions & Design Decisions](#assumptions--design-decisions)
- [Future Enhancements](#future-enhancements)
- [Author](#author)

---

## Project Overview

The **Book Reading Events Management System** is an ASP.NET Core Web API designed to facilitate the organisation and participation of book reading events. The system allows users to register, create public or private reading events, invite others, and engage through comments.

The API is built following **N-Layer (Clean Separation) Architecture**, ensuring clear separation of concerns between the API, application logic, domain model, and infrastructure. It is designed to be consumed by any frontend client — Angular, React, MVC, or mobile applications.

### Problem Statement

Design and develop an ASP.NET Core Web API that:

- Manages book reading events with support for public and private visibility
- Implements authentication and role-based access control
- Supports event invitations via email
- Allows authenticated users to comment on events
- Exposes a well-documented RESTful API

---

## Features

### Authentication & Authorization

| Feature | Details |
|---|---|
| User Registration | Email, password, and full name |
| User Login | Returns a signed JWT token |
| JWT Bearer Authentication | Token validated on all protected endpoints |
| Role-Based Access Control | `Anonymous`, `RegisteredUser`, `Admin` |
| Admin Seeding | Admin account auto-seeded at startup |

### Events

| Feature | Access |
|---|---|
| Create Event | Registered User |
| View Public Events | Anonymous |
| View Event Details | Anonymous (public) / Auth (private) |
| Edit Event | Creator or Admin |
| My Events | Registered User |
| Invited Events | Registered User |

### Invitations

- Private events support invitation via email addresses
- Invited users can access private events they are invited to

### Comments

- Authenticated users can post comments on events
- Comments are returned as part of event details

---

## Technology Stack

### Backend

| Technology | Purpose |
|---|---|
| ASP.NET Core Web API (.NET 8) | HTTP API framework |
| C# | Primary language |
| Entity Framework Core | ORM and database access |
| PostgreSQL | Database |
| ASP.NET Core Identity | User management and password hashing |
| JWT Authentication | Stateless token-based auth |

### Libraries & Tools

| Library | Purpose |
|---|---|
| FluentValidation | Request model validation |
| Swagger / OpenAPI | API documentation and testing UI |
| xUnit | Unit test framework |
| Moq | Mocking framework for unit tests |
| FluentAssertions | Expressive test assertions |

---

## Architecture

The application follows **N-Layer Architecture**, with each layer having a clearly defined responsibility. Dependencies flow strictly inward — outer layers depend on inner layers, never the reverse.

```
┌──────────────────────────────────────────────┐
│                  API Layer                   │
│   Controllers · Middleware · HTTP I/O        │
├──────────────────────────────────────────────┤
│             Application Layer                │
│   Services · DTOs · Validators · Exceptions  │
├──────────────────────────────────────────────┤
│               Domain Layer                   │
│     Entities · Enums · Repository Contracts  │
├──────────────────────────────────────────────┤
│            Infrastructure Layer              │
│   EF Core · Repositories · JWT · Seeder      │
└──────────────────────────────────────────────┘
```

### Layer Responsibilities

#### API Layer (`BookReadingEvents.API`)
- Hosts ASP.NET Core controllers
- Handles HTTP request/response lifecycle
- Registers middleware (exception handling, auth)
- Contains Swagger configuration

**Controllers:**
- `AuthController` — Register and Login endpoints
- `EventsController` — All event and comment endpoints

#### Application Layer (`BookReadingEvents.Application`)
- Contains all business logic
- Defines Data Transfer Objects (DTOs) for request and response
- Declares service interfaces and implementations
- Contains FluentValidation validators
- Defines custom domain exceptions

**Services:**
- `AuthService` — Registration, login, token generation coordination
- `EventService` — Event CRUD, invitation handling, access control
- `CommentService` — Comment creation and retrieval

#### Domain Layer (`BookReadingEvents.Domain`)
- Framework-agnostic core model
- Entity definitions and enumerations
- Repository interface contracts (no implementation)

**Entities:** `ApplicationUser`, `Event`, `Invitation`, `Comment`

#### Infrastructure Layer (`BookReadingEvents.Infrastructure`)
- Implements repository contracts from the Domain layer
- Hosts `ApplicationDbContext` (EF Core)
- Implements `JwtService` for token generation
- Contains `AdminSeeder` for startup data seeding

#### Test Layer (`BookReadingEvents.Tests`)
- xUnit unit tests for service classes
- Uses Moq to mock repository dependencies
- Uses FluentAssertions for readable assertions

---

## Project Structure

```
BookReadingEvents/
├── BookReadingEvents.API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── EventsController.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   └── Program.cs
│
├── BookReadingEvents.Application/
│   ├── DTOs/
│   │   ├── Auth/
│   │   │   ├── RegisterRequest.cs
│   │   │   ├── LoginRequest.cs
│   │   │   └── LoginResponse.cs
│   │   └── Events/
│   │       ├── CreateEventRequest.cs
│   │       ├── UpdateEventRequest.cs
│   │       └── EventResponse.cs
│   ├── Exceptions/
│   │   ├── NotFoundException.cs
│   │   ├── ForbiddenException.cs
│   │   └── BusinessRuleException.cs
│   ├── Services/
│   │   ├── Interfaces/
│   │   │   ├── IAuthService.cs
│   │   │   ├── IEventService.cs
│   │   │   └── ICommentService.cs
│   │   └── Implementations/
│   │       ├── AuthService.cs
│   │       ├── EventService.cs
│   │       └── CommentService.cs
│   └── Validators/
│       ├── RegisterRequestValidator.cs
│       ├── LoginRequestValidator.cs
│       ├── CreateEventRequestValidator.cs
│       └── UpdateEventRequestValidator.cs
│
├── BookReadingEvents.Domain/
│   ├── Entities/
│   │   ├── ApplicationUser.cs
│   │   ├── Event.cs
│   │   ├── Invitation.cs
│   │   └── Comment.cs
│   ├── Enums/
│   │   └── EventType.cs
│   └── Interfaces/
│       ├── IEventRepository.cs
│       ├── IInvitationRepository.cs
│       └── ICommentRepository.cs
│
├── BookReadingEvents.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Repositories/
│   │   ├── EventRepository.cs
│   │   ├── InvitationRepository.cs
│   │   └── CommentRepository.cs
│   ├── Services/
│   │   └── JwtService.cs
│   └── Seeders/
│       └── AdminSeeder.cs
│
└── BookReadingEvents.Tests/
    ├── EventServiceTests.cs
    └── AuthServiceTests.cs
```

---

## Database Design

### Entity Relationship Overview

```
ApplicationUser ──< Event (creator)
Event           ──< Invitation
Event           ──< Comment
ApplicationUser ──< Comment (author)
```

### ApplicationUser

Extends `IdentityUser` (ASP.NET Core Identity).

| Column | Type | Notes |
|---|---|---|
| Id | string (GUID) | Primary key (Identity) |
| Email | string | Unique, required |
| UserName | string | Defaults to email |
| FullName | string | Max 50 characters |

### Event

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key, auto-increment |
| Title | string | Max 50 characters, required |
| Date | DateOnly | Event date |
| Location | string | Required |
| StartTime | TimeOnly | Event start time |
| DurationHours | int | Max 4 hours |
| Type | EventType (enum) | `Public` or `Private` |
| Description | string | Max 50 characters |
| OtherDetails | string | Max 500 characters |
| CreatorId | string | FK → ApplicationUser |
| CreatedAt | DateTime | UTC timestamp |

### Invitation

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key |
| EventId | int | FK → Event |
| InvitedEmail | string | Email of invited user |
| InvitedAt | DateTime | UTC timestamp |

### Comment

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key |
| EventId | int | FK → Event |
| UserId | string | FK → ApplicationUser |
| CommentText | string | Required |
| CreatedAt | DateTime | UTC timestamp |

---

## API Endpoints

### Base URL

```
https://localhost:<port>/api
```

---

### Authentication

#### Register

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "P@ssw0rd",
  "fullName": "John Doe"
}
```

**Response:** `200 OK`

---

#### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "P@ssw0rd"
}
```

**Response:**

```json
{
  "token": "<JWT Token>",
  "email": "john@example.com",
  "fullName": "John Doe"
}
```

---

### Events

| Method | Endpoint | Auth Required | Description |
|---|---|---|---|
| `GET` | `/api/events/public` | No | Retrieve all public events |
| `GET` | `/api/events/{id}` | Conditional | Public events: open. Private events: creator, invited user, or admin |
| `POST` | `/api/events` | Yes | Create a new event |
| `PUT` | `/api/events/{id}` | Yes | Update an event (creator or admin only) |
| `GET` | `/api/events/my` | Yes | Retrieve events created by the logged-in user |
| `GET` | `/api/events/invited` | Yes | Retrieve private events the user is invited to |

#### Create Event — Request Body

```json
{
  "title": "The Alchemist Reading Circle",
  "date": "2025-08-15",
  "location": "Central Library, Hall B",
  "startTime": "18:00",
  "durationHours": 2,
  "type": "Private",
  "description": "Monthly reading of Paulo Coelho",
  "otherDetails": "Bring your own copy. Refreshments provided.",
  "invitedEmails": ["alice@example.com", "bob@example.com"]
}
```

---

### Comments

| Method | Endpoint | Auth Required | Description |
|---|---|---|---|
| `POST` | `/api/events/{id}/comments` | Yes | Add a comment to an event |

#### Add Comment — Request Body

```json
{
  "commentText": "Really enjoyed this session. Looking forward to the next one!"
}
```

---

## Authentication Flow

```
1. Client → POST /api/auth/register      → Account created
2. Client → POST /api/auth/login         → JWT token returned
3. Client → Stores token (local/session)
4. Client → GET /api/events/my
           Authorization: Bearer <token>  → Protected resource returned
```

The JWT token contains the user's `Id`, `Email`, and `Role` as claims. Tokens are validated by the ASP.NET Core JWT middleware on every request to a protected endpoint.

**Example Authorization Header:**

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Role Claims

| Role | How Assigned |
|---|---|
| `Admin` | Seeded user with fixed email `myadmin@bookevents.com` |
| `RegisteredUser` | All users registered via `/api/auth/register` |

---

## Validation Rules

Validation is handled using **FluentValidation**, applied at the application layer before any service logic executes.

### Registration (`RegisterRequestValidator`)

| Field | Rule |
|---|---|
| Email | Required, valid email format, must be unique |
| Password | Required, minimum 5 characters, at least 1 special character |
| FullName | Required, maximum 50 characters |

### Login (`LoginRequestValidator`)

| Field | Rule |
|---|---|
| Email | Required, valid email format |
| Password | Required |

### Create Event (`CreateEventRequestValidator`)

| Field | Rule |
|---|---|
| Title | Required, maximum 50 characters |
| Location | Required |
| DurationHours | Required, maximum 4 hours |
| Description | Maximum 50 characters |
| OtherDetails | Maximum 500 characters |

### Update Event (`UpdateEventRequestValidator`)

Applies the same rules as Create Event. Additionally enforced at the service level:

- Only the event creator or an Admin may update an event
- Past events cannot be modified

---

## Exception Handling

A global **ExceptionHandlingMiddleware** intercepts all unhandled exceptions and maps them to appropriate HTTP responses, ensuring consistent error formatting across the API.

### Exception to HTTP Status Mapping

| Exception | HTTP Status | Scenario |
|---|---|---|
| `NotFoundException` | `404 Not Found` | Event or user does not exist |
| `ForbiddenException` | `403 Forbidden` | User lacks permission to access/modify a resource |
| `BusinessRuleException` | `400 Bad Request` | Rule violation (e.g., editing a past event) |
| Unhandled Exception | `500 Internal Server Error` | Unexpected server-side error |

### Error Response Format

```json
{
  "statusCode": 403,
  "message": "You do not have permission to edit this event."
}
```

---

## Unit Testing

Unit tests are written using **xUnit**, with **Moq** for dependency mocking and **FluentAssertions** for readable assertions.

Tests cover the service layer exclusively. Repository interfaces are mocked, isolating business logic from database access.

### EventService Tests

| Test | Validates |
|---|---|
| `CreateEvent_Should_Create_Event` | Event is persisted via repository when valid input is provided |
| `CreateEvent_Should_Create_Invitations_When_Emails_Are_Provided` | Invitations are created for each provided email |
| `CreateEvent_Should_Not_Create_Invitations_When_Email_List_Is_Empty` | No invitations are created when email list is empty or null |
| `GetPublicEvents_Should_Return_Public_Events` | Only events with `Type = Public` are returned |
| `GetMyEvents_Should_Return_User_Events` | Only events where `CreatorId` matches the user are returned |

### AuthService Tests

| Test | Validates |
|---|---|
| `Register_Should_Throw_When_Email_Already_Exists` | `BusinessRuleException` is thrown when a duplicate email is registered |
| `Login_Should_Throw_When_User_Not_Found` | `NotFoundException` is thrown when credentials do not match any account |

### Running Tests

```bash
dotnet test
```

---
## Database

The application currently uses **Supabase PostgreSQL** as the primary database provider.

### Database Hosting

* Provider: PostgreSQL
* Hosting Platform: Supabase
* ORM: Entity Framework Core
* Provider Package: Npgsql.EntityFrameworkCore.PostgreSQL

### Database Migration

The database schema is managed using Entity Framework Core migrations.

To apply migrations:

```powershell
Update-Database
```

### Database Backup

A SQL Server backup file is also included as part of the submission for evaluation purposes.

#### Location

```text
Database/BookReadingEventsDb.bak
```

#### Restore Instructions

1. Open SQL Server Management Studio (SSMS).
2. Right-click **Databases**.
3. Select **Restore Database**.
4. Choose **Device**.
5. Select **BookReadingEventsDb.bak**.
6. Complete the restore wizard.

### Notes

* The application was originally developed using SQL Server.
* During deployment preparation, the database was migrated to Supabase PostgreSQL due Azure SQL provisioning restrictions on the provided subscription.
* The SQL Server backup is retained for reference, evaluation, and local restoration if required.


## Setup Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote instance)
- Visual Studio 2022 / VS Code / Rider

---

### 1. Clone the Repository

```bash
git clone <repository-url>
cd BookReadingEvents
```

### 2. Configure Connection String

Update `appsettings.json` in the `BookReadingEvents.API` project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BookReadingEventsDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-256-bit-secret-key-here",
    "Issuer": "BookReadingEventsAPI",
    "Audience": "BookReadingEventsClient",
    "ExpiryMinutes": 60
  }
}
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

```bash
dotnet ef database update \
  --project BookReadingEvents.Infrastructure \
  --startup-project BookReadingEvents.API
```

> This creates the database schema and seeds the Admin user automatically via `AdminSeeder`.

### 5. Run the Application

```bash
dotnet run --project BookReadingEvents.API
```

The API will start at `https://localhost:<port>`. The port is shown in the console output.

---

## Swagger Usage

Swagger UI is enabled in the development environment for interactive API exploration and testing.

### Access Swagger

```
https://localhost:<port>/swagger
```

### Authenticating in Swagger

1. Call `POST /api/auth/login` to obtain a JWT token.
2. Click the **Authorize** button (🔓) at the top right of the Swagger page.
3. Enter the token in the following format:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

4. Click **Authorize** → **Close**.
5. All subsequent requests from Swagger will include the token in the `Authorization` header.

---

## Assumptions & Design Decisions

| # | Assumption / Decision |
|---|---|
| 1 | The Admin account is pre-seeded at application startup with a fixed email: `myadmin@bookevents.com`. |
| 2 | Invitations are stored using email addresses. If the invited user later registers, their access is resolved by email matching. |
| 3 | Private events are accessible only to the creator, explicitly invited users, and the Admin. |
| 4 | Only the creator of an event or an Admin can update the event details. |
| 5 | Past events (where the date is in the past) cannot be modified, enforced at the service layer. |
| 6 | FluentValidation validators are registered using the `AddFluentValidationAutoValidation()` extension, so validation runs automatically before controller actions. |
| 7 | JWT tokens are short-lived (configurable in `appsettings.json`). Refresh tokens are not implemented in this version. |
| 8 | The `EventType` enum with values `Public` and `Private` drives visibility logic throughout the system. |
| 9 | Comments are not paginated in this version; all comments are returned with the event details. |
| 10 | UserName in ASP.NET Core Identity is set to the user's email address for simplicity. |

---

## Future Enhancements

| Enhancement | Description |
|---|---|
| Email Notifications | Send invitation emails when a private event is created |
| Pagination | Add pagination to event listings and comment retrieval |
| Event Search & Filtering | Filter by date, location, type, or keyword |
| Refresh Tokens | Implement token refresh to extend session life without re-login |
| Docker Support | Containerise the API and database using Docker Compose |
| Azure Deployment | Deploy to Azure App Service with Azure SQL Database |
| Rating / Reactions | Allow users to rate or react to events they attended |
| Event Cancellation | Allow creators to cancel events and notify invited users |

---

## Author

**Vaibhav Yadav**
ASP.NET Core Web API Assignment

---

> *Built with ASP.NET Core 8 · Entity Framework Core · JWT · FluentValidation · xUnit*
