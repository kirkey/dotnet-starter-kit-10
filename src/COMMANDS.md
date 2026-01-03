# Quick Command Reference

## 🚀 Get Started Immediately

### Option A: Full Stack with Redis (Aspire)
```bash
make apphost
```
✅ Includes: API + Blazor + Redis + PostgreSQL + Dashboard

### Option B: Quick Development (No Docker)
```bash
make api-http       # Terminal 1
make blazor-http    # Terminal 2
```
✅ No Redis needed, no Docker required

---

## 📋 All Commands

| Command | Purpose | Notes |
|---------|---------|-------|
| `make help` | Show all commands | Run anytime |
| `make setup` | Setup HTTPS cert | One-time only |
| `make apphost` | Full Aspire stack | Requires Docker |
| `make api` | Run API (HTTPS) | Uses ports 7030, 5030 |
| `make api-http` | Run API (HTTP) | Uses port 5030 only |
| `make blazor` | Run Blazor (HTTPS) | Uses ports 7140, 5032 |
| `make blazor-http` | Run Blazor (HTTP) | Uses port 5032 only |
| `make nswag` | Generate API client | After API changes |
| `make kill-ports` | Stop all services | Kills all related processes |
| `make kill-ports DRY_RUN=1` | Preview kills | Shows what would be killed |

---

## 🔗 Access Points

### With Aspire (make apphost)
Check the AppHost output for exact ports - they're shown in the terminal and Dashboard.

### Standalone (make api-http + make blazor-http)
- **Blazor UI**: http://localhost:5032
- **API**: http://localhost:5030
- **API Docs**: http://localhost:5030/scalar

---

## ⚠️ Common Issues & Fixes

| Issue | Fix |
|-------|-----|
| "Redis connection timeout" | Use `make api-http` (or `make apphost` if using Redis) |
| "Port already in use" | `make kill-ports` |
| "HTTPS certificate error" | `make setup` |
| "Docker not running" | Start Docker Desktop or use `make api-http` |
| "Blazor can't connect to API" | Ensure API is running first in separate terminal |

---

## 🔄 Typical Development Workflow

### Approach 1: Aspire (Full Stack)
```bash
# Start everything with one command
make apphost

# Open Dashboard at the shown URL
# All services run together
```

### Approach 2: Standalone (Fastest)
```bash
# Terminal 1 - API
make api-http

# Terminal 2 - Blazor UI
make blazor-http

# Open browser
open http://localhost:5032
```

### Approach 3: With Code Changes
```bash
# Terminal 1 - API
make api-http

# Terminal 2 - Blazor  
make blazor-http

# Terminal 3 - After API changes, regenerate client code
make nswag

# Refresh Blazor in browser
```

---

## 📚 Documentation Files

- **REDIS_RESOLVED.md** ← Start here for the Redis issue
- **REDIS_ASPIRE_GUIDE.md** - Detailed Aspire/Redis guide
- **DEVELOPMENT.md** - Complete development setup
- **QUICKSTART.md** - Quick start reference
- **SETUP_COMPLETE.md** - Setup status report

---

## 🛑 Stopping Services

### Stop Aspire
Press `Ctrl+C` in the AppHost terminal

### Stop Standalone Services
```bash
make kill-ports
```

---

## 🔧 Advanced Usage

### Run with Custom Arguments
```bash
# Custom URL for API
make api DOTNET_RUN_ARGS="--urls http://localhost:8080"

# Custom AppHost arguments
make apphost DOTNET_RUN_ARGS="--verbose"
```

### Preview Ports Before Killing
```bash
make kill-ports DRY_RUN=1
```

### Check HTTPS Certificate
```bash
dotnet dev-certs https --check --trust
```

---

## 💾 File Structure

```
/src/
├── Makefile                      # All commands here
├── REDIS_RESOLVED.md            # Redis issue solution
├── REDIS_ASPIRE_GUIDE.md        # Detailed Aspire guide
├── DEVELOPMENT.md               # Development setup
├── QUICKSTART.md                # Quick reference
├── Apps/
│   ├── Apps.Api/          # API service
│   ├── Apps.Blazor/       # Blazor UI
│   ├── FSH.Apps.AppHost/  # Aspire orchestrator
│   └── Migrations.PostgreSQL/   # Database migrations
└── BuildingBlocks/              # Shared libraries
```

---

## 🚀 TL;DR (Too Long; Didn't Read)

### Just want to code?
```bash
make api-http && make blazor-http
```
Then go to: **http://localhost:5032**

### Want production-like setup?
```bash
make apphost
```
Then check output for Dashboard URL

### Need help?
```bash
make help
cat REDIS_RESOLVED.md
```

---

## ✅ Verification

Everything working?

```bash
# Check help is updated
make help

# Check HTTPS cert
dotnet dev-certs https --check --trust

# Check what processes would be killed
make kill-ports DRY_RUN=1
```

---

**That's it! Choose your approach and start developing.** 🎉

