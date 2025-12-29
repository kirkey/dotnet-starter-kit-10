# Quick Reference

## Essential Commands

```bash
# First time only - Setup HTTPS certificate
make setup

# Start API (HTTPS)
make api

# Start Blazor UI (HTTPS)
make blazor

# Regenerate client code from API
make nswag

# Stop all services
make kill-ports
make kill-ports DRY_RUN=1  # preview first

# Show all available commands
make help
```

## Ports Used

- **API**: `https://localhost:7030` or `http://localhost:5030`
- **Blazor**: `https://localhost:7140` or `http://localhost:5032`

## Common Issues & Fixes

| Issue | Solution |
|-------|----------|
| **HTTPS certificate error** | `make setup` |
| **Connection refused** | Start API first: `make api` (in another terminal) |
| **Blazor can't reach API** | Use HTTP: `make api-http` and `make blazor-http` |
| **Ports already in use** | `make kill-ports` |
| **NSwag not found** | `dotnet tool install --global NSwag.ConsoleCore` |

## Development Setup (Two Terminals)

**Terminal 1:**
```bash
make api
```

**Terminal 2:**
```bash
make blazor
```

That's it! The Blazor app will connect to the API.

