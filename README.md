# Demo Project: Authentication & Blazor WASM

This repository contains a full-stack authentication system featuring a **.NET 10 Web API** backend and a **Blazor WebAssembly** frontend, supported by **OpenTelemetry** observability stack and **Nginx** as an API Gateway.



## Quick Start: Running the Stack

The entire ecosystem is containerized for a single-command setup.

1.  **Clone and Launch:**
    ```bash
    docker-compose up -d
    ```
2.  **Verify Services:** Ensure all containers are healthy (especially `postgres` and `auth-api`).
    
    *Note: If you run into network issues, try pulling the images individually.*
    ```bash
    docker pull hubviwe/thebase.blazor.wasm:0.9.14
    docker pull hubviwe/thebase.auth.api:0.9.14
    docker pull postgres:16.3-alpine
    docker pull otel/opentelemetry-collector:0.103.0
    docker pull jaegertracing/all-in-one:1.58
    docker pull datalust/seq:2024.3
    docker pull grafana/grafana:10.4.4
    docker pull prom/prometheus:v2.45.6
    ```



## Step-by-Step: Your First Authentication Flow

Follow these steps to test the end-to-end integration and trigger the observability pipeline.

### Step 1: Registration
* **Navigate to:** http://localhost:5109/authentication-register
* **Action:** Fill in your details and click **Register**.
* **What happens:** The Blazor WASM app sends a POST request to the Nginx gateway, which proxies it to the `auth-api`. A new user is created in PostgreSQL, and a JWT is issued.

### Step 2: Automatic Routing
* **Action:** Upon successful registration, the app is configured to automatically route you.
* **Destination:** You will be redirected to **http://localhost:5109/authentication-account**.
* **Verification:** You should see your user profile details loaded into the UI components.

### Step 3: Update Account Details
* **Action:** Modify your display name or profile information on the account page and click **Save**.
* **Verification:** A successful "Update" notification confirms the backend has persisted the changes.



## Observability: See it in Action

As you perform the steps above, the system generates "live" telemetry. Here is how to view it:

### 1. Traces (Distributed Request Flow)
* **Tool:** [Jaeger UI](http://localhost:16686)
* **What to look for:** Select the `auth-api` service and click "Find Traces." You will see the full lifecycle of your `Register` and `Update` requests, including the database spans.

### 2. Logs (Structured Events)
* **Tool:** [Seq UI](http://localhost:81)
* **What to look for:** Filter by `RequestPath = "/account/register"`. You’ll see structured logs containing the metadata of your registration attempt.

### 3. Metrics (System Health)
* **Tool:** [Grafana](http://localhost:3000/d/KdDACDp4z/asp-net-core?orgId=1) (Login: `admin` / `admin`)
* **What to look for:** Open the **"ASP.NET Core"** dashboard. You can monitor the request rate triggered by your user flow.



## Running Integration Tests

To verify the backend logic independently of the UI:

1.  **Navigate to tests:**
    ```bash
    cd src/Authentication.Test
    ```
2.  **Run tests:**
    ```bash
    dotnet test
    ```
*Note: These tests utilize `AuthenticationWebApplicationFactory` to simulate the full API stack.*
> [!IMPORTANT]  
> Ensure your local environment is running docker as a a PostgreSQL container database will be used instead of an in-memory provider.



## Endpoint Reference

| Resource | URL |
| :--- | :--- |
| **Registration Page** | `http://localhost:5109/authentication-register` |
| **Login Page** | `http://localhost:5109/authentication-loginr` |
| **Account Management** | `http://localhost:5109/authentication-account` |
| **API Docs (Scalar)** | `http://localhost:5056/scalar` |
