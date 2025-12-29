# FSH.CLI - FullStackHero Command Line Interface

A powerful CLI tool for creating, scaffolding, and managing FullStackHero .NET 10 projects with comprehensive code generation and best practices enforcement.

## 📦 Installation

```bash
# Install as global tool
dotnet tool install -g FSH.CLI

# Or install from NuGet
dotnet tool install -g FSH.CLI --version latest

# Update to latest
dotnet tool update -g FSH.CLI
```

## 🚀 Quick Start

```bash
# Interactive wizard (recommended)
fsh new

# Using a preset (fastest)
fsh new MyApp --preset quickstart

# Full customization (non-interactive)
fsh new MyApp --type api-blazor --arch monolith --db postgres --aspire --docker
```

## 📋 Main Command: `fsh new`

Creates a new FullStackHero project with comprehensive scaffolding.

### Interactive Mode (Default)

```bash
fsh new
# or with initial project name
fsh new MyApp
```

The wizard will guide you through:
1. Preset selection or custom configuration
2. Project type (API only or API + Blazor)
3. Architecture style (Monolith, Microservices, Serverless)
4. Database provider selection
5. Optional features (Docker, Aspire, Terraform, GitHub Actions)

### Non-Interactive Mode

```bash
fsh new MyApp --no-interactive --type api-blazor --arch monolith --db postgres
```

### Presets (Quick Start Templates)

| Preset | Type | Arch | Features | Use Case |
|--------|------|------|----------|----------|
| `quickstart` | API | Monolith | Docker, Aspire, Sample Module | Getting started fast |
| `production` | API+Blazor | Monolith | Aspire, Terraform, CI, Docker | Production-ready SaaS |
| `microservices` | API | Microservices | Docker, Aspire | Scaling horizontally |
| `serverless` | API | Serverless | Terraform (AWS Lambda) | Event-driven workloads |

### Project Type Options

```bash
--type api              # API only (REST)
--type api-blazor       # API + Blazor SSR+WASM UI
```

### Architecture Options

```bash
--arch monolith         # Single codebase (recommended for MVP)
--arch microservices    # Multiple services (for teams)
--arch serverless       # AWS Lambda + API Gateway (for event-driven)
```

### Database Options

```bash
--db postgres           # PostgreSQL (recommended, default)
--db sqlserver          # SQL Server
--db sqlite             # SQLite (development only)
```

### Feature Flags

```bash
--docker true|false     # Include Docker Compose (default: true)
--aspire true|false     # Include .NET Aspire AppHost (default: true)
--sample true|false     # Include sample module (default: false)
--terraform true|false  # Include Terraform IaC (default: false)
--ci true|false         # Include GitHub Actions CI/CD (default: false)
--git true|false        # Initialize git repository (default: true)
```

### Version Control

```bash
--fsh-version 10.0.0    # Use specific framework version
--fsh-version 10.0.0-rc.1  # Use pre-release version
# If not specified, uses the CLI's version
```

### Output

```bash
--output ./projects     # Custom output directory (default: current)
-o ~/Dev/MyApp          # Shorthand
```

## 📋 Complete Examples

### Example 1: Quickstart (Interactive)
```bash
fsh new MyEcommerce
# Follow the wizard, select "quickstart" preset
```

### Example 2: Production Monolith
```bash
fsh new MyApp \
  --preset production \
  --type api-blazor \
  --arch monolith \
  --db postgres \
  --aspire \
  --docker \
  --terraform \
  --ci
```

### Example 3: Microservices API
```bash
fsh new MyPlatform \
  --type api \
  --arch microservices \
  --db postgres \
  --aspire \
  --docker \
  --output ~/projects
```

### Example 4: Serverless Backend
```bash
fsh new MyServerlessApp \
  --type api \
  --arch serverless \
  --db postgres \
  --terraform
```

### Example 5: SQLite Development
```bash
fsh new MyLocalApp \
  --type api-blazor \
  --arch monolith \
  --db sqlite \
  --no-interactive
```

### Example 6: Specific Version
```bash
fsh new MyApp \
  --preset quickstart \
  --fsh-version 10.0.0
```

## 🎯 Generated Project Structure

After running `fsh new`, you'll get:

```
MyApp/
├── src/
│   ├── BuildingBlocks/       # Reusable infrastructure
│   ├── Modules/              # Business domain modules
│   ├── Playground/           # Host applications
│   │   ├── Playground.Api/
│   │   ├── Playground.Blazor/ (if --type api-blazor)
│   │   ├── FSH.Playground.AppHost/ (if --aspire)
│   │   └── Migrations.PostgreSQL/
│   └── Tests/                # Test projects
├── terraform/                # IaC (if --terraform)
├── scripts/                  # Utility scripts
├── docker-compose.yml        # (if --docker)
├── FSH.Framework.slnx        # Solution file
└── README.md                 # Project documentation
```

## ⚙️ What Gets Generated

### Always Included
- ✅ BuildingBlocks (Core, Persistence, Web, Caching, etc.)
- ✅ Module structure with best practices
- ✅ Database context with multi-tenancy support
- ✅ API endpoints (Minimal APIs)
- ✅ FluentValidation setup
- ✅ Architecture tests
- ✅ Documentation (COPILOT_INSTRUCTIONS.md, etc.)

### Conditional (Based on Options)
- ✅ Blazor UI (if `--type api-blazor`)
- ✅ Docker Compose (if `--docker true`)
- ✅ .NET Aspire AppHost (if `--aspire true`)
- ✅ Sample Module (if `--sample true`)
- ✅ Terraform IaC (if `--terraform true`)
- ✅ GitHub Actions CI (if `--ci true`)

## 🔧 Advanced Options

### Combine Presets with Overrides

```bash
# Start with quickstart but add Terraform
fsh new MyApp --preset quickstart --terraform

# Start with production but remove Terraform
fsh new MyApp --preset production --terraform false
```

### Silent Mode with Custom Paths

```bash
fsh new MyApp \
  --no-interactive \
  --type api-blazor \
  --arch monolith \
  --db postgres \
  --output /home/user/projects \
  --fsh-version 10.0.0
```

## 📚 Next Steps After Project Creation

```bash
cd MyApp

# Restore NuGet packages
dotnet restore src/FSH.Framework.slnx

# Build solution
dotnet build src/FSH.Framework.slnx

# Run with Aspire (if included)
dotnet run --project src/Playground/FSH.Playground.AppHost

# Run API standalone
dotnet run --project src/Playground/Playground.Api

# Run tests
dotnet test src/FSH.Framework.slnx

# Generate API clients from OpenAPI
./scripts/openapi/generate-api-clients.ps1 -SpecUrl "https://localhost:7030/openapi/v1.json"
```

## 📖 Documentation & Resources

Generated projects include comprehensive documentation:

- **CLAUDE.md** - Build & run commands, quick reference
- **COPILOT_INSTRUCTIONS.md** - Comprehensive code patterns guide
- **ARCHITECTURE_GUIDE.md** - Deep architecture documentation
- **MODULE_TEMPLATES.md** - Decision trees for different module types
- **BEST_PRACTICES_AUDIT.md** - Best practices checklist
- **COPILOT_AGENT_README.md** - AI agent guidance

## 🆘 Troubleshooting

### "dotnet CLI not found"
```bash
# Ensure .NET 10 is installed
dotnet --version
```

### "Directory exists and is not empty"
```bash
# The tool will ask to confirm overwrite, or use a different name
fsh new MyApp2
```

### "Invalid preset"
```bash
# Use one of: quickstart, production, microservices, serverless
fsh new MyApp --preset quickstart
```

### "Template not found"
```bash
# Update to latest CLI version
dotnet tool update -g FSH.CLI
```

## 🎓 Learning Resources

1. **Getting Started**: Start with `COPILOT_AGENT_README.md` in generated project
2. **Architecture**: Review `ARCHITECTURE_GUIDE.md` for design patterns
3. **Implementation**: Check `COPILOT_INSTRUCTIONS.md` for code patterns
4. **Templates**: Use `MODULE_TEMPLATES.md` for new features
5. **Reference Code**: Look at Identity module in existing projects

## 🐛 Reporting Issues

If you encounter issues with FSH.CLI:

```bash
# Check version
fsh --version

# Run with verbose output
fsh new MyApp --verbose  # (if supported)

# Check your .NET version
dotnet --version
```

Report issues at: https://github.com/fullstackhero/dotnet-starter-kit/issues

## ✨ Features

✅ **Interactive Wizard** - Rich TUI with guided setup  
✅ **Quick Presets** - Pre-configured templates (Quickstart, Production, Microservices, Serverless)  
✅ **Flexible Architecture** - Monolith, Microservices, or Serverless  
✅ **Database Support** - PostgreSQL, SQL Server, SQLite  
✅ **Container Ready** - Docker Compose integration  
✅ **Cloud Native** - .NET Aspire support  
✅ **Infrastructure as Code** - Terraform templates (AWS)  
✅ **CI/CD Ready** - GitHub Actions templates  
✅ **Best Practices** - Architecture tests and documentation  
✅ **Version Control** - Git repository initialization  

## 📦 What's Included

Every generated project includes:

- ✅ .NET 10 (latest framework)
- ✅ ASP.NET Core with Minimal APIs
- ✅ Entity Framework Core
- ✅ Multi-tenancy support (Finbuckle)
- ✅ CQRS pattern (Mediator library)
- ✅ FluentValidation
- ✅ Redis caching
- ✅ Hangfire background jobs
- ✅ OpenTelemetry observability
- ✅ Structured logging (Serilog)
- ✅ JWT authentication
- ✅ OpenAPI/Swagger documentation
- ✅ Architecture tests (NetArchTest)

## 📄 License

MIT - See LICENSE file in project

## 👥 Support

- **Documentation**: https://github.com/fullstackhero/dotnet-starter-kit
- **Issues**: https://github.com/fullstackhero/dotnet-starter-kit/issues
- **Discussions**: https://github.com/fullstackhero/dotnet-starter-kit/discussions
