# Brainbay Characters Test Assignment

This solution demonstrates a simple character management system built with ASP.NET Core, Blazor Server, and a console application for data synchronization from the Rick and Morty API into a local database.

---

## 1. Running the Project with Docker

The solution is configured to run via `docker compose` using the `compose.yaml` file at the root of the repository.

### compose.yaml Overview

The `compose.yaml` file defines (at least) the following services:

- **WebApi service**
    - Hosts the ASP.NET Core Web API.
    - Also hosts the Blazor Server UI components.
- **Database service**
    - Provides a relational database used by both the WebApi and the console sync project.
- **Console sync service**
    - A console application responsible for synchronizing characters from the Rick and Morty API into the local database.
    - Uses a dedicated Docker Compose profile: `sync`.

### Profiles

The console sync application is **not** started by default. It is placed under a separate profile called `sync`, so you can control when it runs:

- To start **WebApi + Database** (default profile):

  ```bash
  docker compose up
  ```

- To start **only** the console sync app (assuming DB and WebApi are already running):

  ```bash
  docker compose --profile sync up
  ```

- To start **everything including sync**:

  ```bash
  docker compose --profile sync up --build
  ```

### Startup Expectations

- The **console sync project expects the database to be already running**.
- The **WebApi project**:
    - Hosts the HTTP API endpoints.
    - Serves the Blazor Server UI from the same application and base URL.

---

## 2. Project Structure

The solution is split into multiple projects with clear responsibilities.

### Console Project

- Uses an abstraction like `ICharacterSyncManager` to:
    - Fetch character data from the **Rick and Morty API**.
    - Store/synchronize this data into the **local database**.
- Intended to be run as a background job / batch process (e.g. via Docker profile `sync`).

### WebApi Project

- ASP.NET Core Web API application.
- Responsibilities:
    - Expose controllers to access and manage character data.
    - Host the **Blazor Server** components used by the frontend UI.
    - Provide **Swagger UI** for exploring and testing the API.

---

## 3. Design Considerations and Possible Enhancements

This project is intentionally simplified for demonstration purposes. In a production scenario, the following enhancements would be made.

### 3.1 Error Handling

- Currently, the solution assumes a *“happy path”* (success flow) in most places.
- There is no full-featured error handling or rich error modeling.
- Outside of a test/demo project, the typical approach would be:
    - Use **typed error contracts** in:
        - API contracts
        - Data access / storage layers
    - In the **WebApi** layer:
        - Expose errors using standardized **`ProblemDetails`** responses.
        - Provide consistent error codes and detailed error information to the client.

### 3.2 In-Memory Caching Strategy

- All records from the database are currently loaded into an **in-memory store** at once.
- There was an attempt to switch to **paged loading** of characters and caching them page by page. The complication with that approach:
    - When some characters come from memory and some from the database in the same response, it becomes unclear what value to return for headers such as `from-database`, because:
        - Part of the data originates from the cache.
        - Part is freshly loaded from the database.
- A more advanced strategy (not yet implemented) could include:
    - Per-page metadata about data sources.
    - More granular cache invalidation.
    - Response headers reflecting mixed sources in a well-defined format.

### 3.3 Database Migrations and Sync App

- The console application responsible for data synchronization:
    - **Does not perform migrations or create the database**.
- As a consequence:
    - The **WebApi** and **database** containers must be started first so that:
        - The database is created.
        - Migrations are applied.
    - Only afterwards should the **console sync application** be started (locally or via the `sync` profile in Docker).

In a more complete setup, you would likely:

- Introduce a dedicated migration step/tool.
- Ensure the console sync app either:
    - Fails fast with a clear message if the DB is not ready, or
    - Performs DB readiness checks with retries.

---

## 4. WebApi Project

The WebApi project provides HTTP endpoints for interacting with character data.

- **Swagger UI** is available at:

  ```text
  http://localhost:5000/swagger
  ```

---

## 5. Frontend (Blazor Server)

- The frontend is implemented with **Blazor Server**.
- Blazor components are hosted **within the same WebApi project**.
- The main UI page for viewing and creating characters is served directly from the **base URL** of the server. For example:

  ```text
  http://localhost:5000/
  ```
