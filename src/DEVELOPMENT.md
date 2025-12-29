# Development Guide

This guide helps you set up and run the FSH Framework Playground application locally.

## Quick Start

### 1. First-Time Setup (HTTPS Certificate)

Before running the application, you need to set up the HTTPS developer certificate:

```bash
make setup
```

This will:
- Check if a valid HTTPS developer certificate exists
- Trust it on your system (macOS/Windows only)
- Enable HTTPS endpoints to work properly

### 2. Running the Application

You can run the application in several ways:

#### Run API Only (HTTPS)
```bash
make api
```
- Runs on HTTPS: `https://localhost:7030`
- Also available on HTTP: `http://localhost:5030`

#### Run API Only (HTTP)
```bash
make api-http
```
- Runs on HTTP only: `http://localhost:5030`
- Use this to avoid certificate issues

#### Run Blazor UI Only (HTTPS)
```bash
make blazor
```
- Runs on HTTPS: `https://localhost:7140`
- Also available on HTTP: `http://localhost:5032`

#### Run Blazor UI Only (HTTP)
```bash
make blazor-http
```
- Runs on HTTP only: `http://localhost:5032`

### 3. Updating API Client Code

When you update the API, regenerate the Blazor client code:

```bash
make nswag
```

This will:
- Find all NSwag configuration files
- Regenerate the generated API client code
- Update TypeScript/C# clients as needed

## Port Management

The solution uses the following default ports:

| Service | HTTPS | HTTP |
|---------|-------|------|
| **API** | 7030 | 5030 |
| **Blazor UI** | 7140 | 5032 |
| **AppHost** | 17000 | 5000 |

### Kill Processes on Used Ports

To stop all running services associated with this solution:

```bash
make kill-ports
```

#### Preview Before Killing (Dry-Run)
```bash
make kill-ports DRY_RUN=1
```

This will show which ports are in use and which PIDs would be killed without actually killing them.

## Troubleshooting

### HTTPS Certificate Issues

If you see errors like:
```
Unable to configure HTTPS endpoint. No server certificate was specified, and the default 
developer certificate could not be found or is out of date.
```

Run:
```bash
make setup
```

### Connection Refused Errors

If you see connection refused errors when running Blazor:
```
System.Net.Http.HttpRequestException: Connection refused (localhost:7030)
```

This typically means:
1. **API is not running** - Start the API first: `make api` (in another terminal)
2. **Using wrong port** - Check that Blazor is configured to connect to the correct API port
3. **Firewall issues** - Check if a firewall is blocking the ports

**Solution**: Run API and Blazor in separate terminals:
- Terminal 1: `make api`
- Terminal 2: `make blazor`

Or use HTTP instead of HTTPS to avoid certificate issues:
- Terminal 1: `make api-http`
- Terminal 2: `make blazor-http`

### NSwag Code Generation Issues

If NSwag is not installed:
```bash
dotnet tool install --global NSwag.ConsoleCore
```

Then run:
```bash
make nswag
```

## Environment Variables

You can customize behavior with environment variables:

```bash
# Run API on a different port
ASPNETCORE_URLS="http://localhost:8080" make api-http

# Pass additional arguments to dotnet run
make api DOTNET_RUN_ARGS="--no-hot-reload"
```

## Common Workflows

### Development Workflow (API + Blazor)

```bash
# Terminal 1 - Setup (first time only)
make setup

# Terminal 1 - Start API
make api-http

# Terminal 2 - Start Blazor
make blazor-http

# Terminal 3 - Regenerate clients after API changes
make nswag
```

### Production Build

```bash
# Build both projects
dotnet build Playground/Playground.Api
dotnet build Playground/Playground.Blazor

# Publish
dotnet publish -c Release Playground/Playground.Api
dotnet publish -c Release Playground/Playground.Blazor
```

## Getting Help

Run the help command anytime:
```bash
make help
```

This shows all available targets and examples.

