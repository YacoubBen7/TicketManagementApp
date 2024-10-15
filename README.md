
# Ticket Management Application

This repository consists of two main components:

1. **Ticket-App-Front**: An Angular application for managing tickets.
2. **Backend**: A .NET Core Web API that manages the server-side logic.

## Prerequisites

To run the Angular frontend and .NET backend locally, you need the following tools installed:

- **Angular CLI** (v16): Install via npm:
  ```bash
  npm install -g @angular/cli@16
  ```
- **.NET SDK** (v8.0): [Download the .NET SDK](https://dotnet.microsoft.com/download)
- **MS SQL Server**

## Cloning the Repository

Clone the repository and navigate into the project directory:

```bash
git clone https://github.com/YacoubBen7/TicketManagementApp.git
cd .\ManageTicketProject
```

## Setting up the Frontend (Angular)

### Step 1: Navigate to the Frontend Directory

Go to the frontend directory:

```bash
cd .\Ticket-App-Front
```

### Step 2: Install Dependencies

Install the required Node.js packages:

```bash
npm install
```

### Step 3: Run the App

Start the Angular app:

```bash
ng serve
```

### Optional: Running Tests

To run unit tests:

```bash
ng test
```

## Setting up the Backend (.NET)

### Step 1: Configure Database Credentials

Navigate to the backend project folder:

```bash
cd .\Backend\TicketService.API
```

Open the **appsettings.json** file and update the connection string under the `"ConnectionStrings"` section to match your SQL Server configuration:

```json
"DefaultConnection":"Server=localhost,1433;Database=TicketServiceDb;User=sa;Password=YourPassword123;TrustServerCertificate=True;"
```

You can adjust the settings for the server address, port, database name, and credentials based on your local environment. The example above uses:

- `localhost` because the database is running locally.
- Default SQL Server port `1433`.
- The database `TicketServiceDb` (which will be created automatically).
- SQL Server credentials (`sa` and `YourPassword123`).

Ensure these match your SQL Server instance, whether it's a local installation, a remote database, or a Docker container. You may need to modify the server address, port, database name, or credentials based on your setup.

### Step 2: Apply Database Migrations

To set up the database and tables for storing tickets, run the following command in the `TicketService.API` folder:

```bash
dotnet ef database update
```

> Note: If the command fails, ensure the Entity Framework tools are installed by running:

```bash
dotnet tool install --global dotnet-ef
```

### Step 3: Run the Backend

To run the backend application:

```bash
dotnet run
```

### Optional: Running Backend Unit Tests

To run unit tests for the backend, navigate to the `TicketService.UnitTests` folder:

```bash
cd ..\TicketService.UnitTests\
dotnet test
```

---

Once these steps are complete, both the Angular frontend and .NET backend should be running and accessible locally.
