# FSH.CLI Documentation & Enhancement Guide

**Updated**: December 29, 2025  
**Version**: 2.0.0  
**Status**: ✅ READY FOR IMPLEMENTATION

---

## 📝 Overview

FSH.CLI has been enhanced to align with the updated FSH Framework .NET 10 Starter Kit. It now includes comprehensive commands for project creation, code generation, and guidance based on COPILOT_INSTRUCTIONS.md patterns.

---

## 🚀 New Features

### 1. **Enhanced `fsh new` Command**
- ✅ Interactive wizard (improved UX)
- ✅ 4 presets with sensible defaults
- ✅ Framework version control
- ✅ Comprehensive documentation generation

### 2. **New `fsh generate` Command** (NEW)
- Generate modules (with 7 templates)
- Generate features (CQRS)
- Generate entities (with multi-tenancy)
- Template selection (CRUD, Workflow, File, Integration, Cached, Reporting, Job)

### 3. **New `fsh help` Command** (NEW)
- General help
- Topic-specific guidance (new, generate, templates, patterns, examples)
- Real-world examples
- Quick reference

---

## 🎯 Command Reference

### Command 1: `fsh new`

Creates a new FullStackHero project with best practices.

```bash
fsh new [ProjectName] [OPTIONS]
```

#### Examples

```bash
# Interactive wizard
fsh new

# With project name
fsh new MyApp

# Using preset
fsh new MyApp --preset quickstart

# Custom configuration
fsh new MyApp \
  --type api-blazor \
  --arch monolith \
  --db postgres \
  --aspire \
  --docker \
  --terraform \
  --ci

# Microservices
fsh new MyPlatform \
  --type api \
  --arch microservices \
  --db postgres

# Serverless
fsh new MyServerless \
  --type api \
  --arch serverless \
  --terraform

# Specific version
fsh new MyApp --preset production --fsh-version 10.0.0
```

#### Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `--preset` | string | - | quickstart, production, microservices, serverless |
| `--type` | string | api | api, api-blazor |
| `--arch` | string | monolith | monolith, microservices, serverless |
| `--db` | string | postgres | postgres, sqlserver, sqlite |
| `--docker` | bool | true | Include Docker Compose |
| `--aspire` | bool | true | Include .NET Aspire AppHost |
| `--sample` | bool | false | Include sample module |
| `--terraform` | bool | false | Include Terraform IaC |
| `--ci` | bool | false | Include GitHub Actions CI |
| `--git` | bool | true | Initialize git repository |
| `--fsh-version` | string | - | Specific framework version |
| `--output` | string | . | Custom output directory |
| `--no-interactive` | bool | false | Skip wizard (non-interactive mode) |

---

### Command 2: `fsh generate` (NEW)

Generates code artifacts following best practices.

```bash
fsh generate <type> <name> [OPTIONS]
```

#### Types

| Type | Description | Examples |
|------|-------------|----------|
| `module` | New business module | `fsh generate module Products` |
| `feature` | CQRS command/query feature | `fsh generate feature CreateProduct` |
| `entity` | Domain entity with EF Core | `fsh generate entity Product` |

#### Templates (for Features & Modules)

| Template | Best For | Time | Complexity |
|----------|----------|------|-----------|
| `crud` | Simple CRUD | 2-3h | ⭐ |
| `workflow` | State machines, approvals | 4-6h | ⭐⭐⭐ |
| `file` | File uploads/management | 3-4h | ⭐⭐ |
| `integration` | External APIs | 4-5h | ⭐⭐⭐ |
| `cached` | Read-heavy, caching | 3-4h | ⭐⭐ |
| `reporting` | Analytics, dashboards | 4-6h | ⭐⭐⭐ |
| `job` | Background jobs | 2-3h | ⭐⭐ |

#### Examples

```bash
# Generate a CRUD module
fsh generate module Products

# Generate a feature with workflow template
fsh generate feature CompleteOrder --template workflow

# Generate domain entity
fsh generate entity Product

# Force overwrite
fsh generate module Users --force

# Custom solution path
fsh generate feature CreateOrder --path /home/user/MyProject

# Skip validation
fsh generate feature DeleteProduct --no-validation
```

#### Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `--template` | string | crud | Template type (see above) |
| `--path` | string | . | Solution root path |
| `--force` | bool | false | Overwrite existing files |
| `--no-validation` | bool | false | Skip FluentValidation |

---

### Command 3: `fsh help` (NEW)

Shows comprehensive help and guidance.

```bash
fsh help [TOPIC]
```

#### Topics

| Topic | Description |
|-------|-------------|
| (none) | General help and quick reference |
| `new` | Help for `fsh new` command |
| `generate` | Help for `fsh generate` command |
| `templates` | Available module templates |
| `patterns` | Architecture patterns explained |
| `examples` | Real-world examples |

#### Examples

```bash
# General help
fsh help

# Help on specific command
fsh help new
fsh help generate

# Learn about templates
fsh help templates

# See examples
fsh help examples

# Learn patterns
fsh help patterns
```

---

## 📚 What Gets Generated

### In `fsh new`

**Always**:
- ✅ Complete BuildingBlocks infrastructure
- ✅ Multi-tenancy support
- ✅ CQRS/Mediator setup
- ✅ FluentValidation
- ✅ Database context
- ✅ Architecture tests
- ✅ **Comprehensive documentation** (COPILOT_INSTRUCTIONS.md, etc.)

**Conditionally**:
- ✅ Blazor UI (`--type api-blazor`)
- ✅ Docker Compose (`--docker true`)
- ✅ .NET Aspire (`--aspire true`)
- ✅ Sample module (`--sample true`)
- ✅ Terraform (`--terraform true`)
- ✅ GitHub Actions (`--ci true`)

### In `fsh generate module`

- ✅ Module structure
- ✅ Contracts project
- ✅ Implementation project
- ✅ Domain entity (based on template)
- ✅ DbContext & configuration
- ✅ Sample feature (CQRS)
- ✅ Module registration class

### In `fsh generate feature`

- ✅ Command/Query (in Contracts)
- ✅ Handler (implementation)
- ✅ Validator (FluentValidation)
- ✅ Endpoint (Minimal API)
- ✅ Unit tests template

### In `fsh generate entity`

- ✅ Domain entity class
- ✅ Multi-tenancy support (IMustHaveTenant)
- ✅ Audit trail (IAuditableEntity)
- ✅ EF Core configuration
- ✅ Database migration template

---

## 🔄 Generated Project Structure

```
MyApp/
├── src/
│   ├── BuildingBlocks/
│   │   ├── Core/
│   │   ├── Persistence/
│   │   ├── Web/
│   │   ├── Caching/
│   │   ├── Eventing/
│   │   ├── Jobs/
│   │   ├── Mailing/
│   │   ├── Storage/
│   │   ├── Blazor.UI/
│   │   └── Shared/
│   ├── Modules/
│   │   ├── Identity/
│   │   ├── Multitenancy/
│   │   ├── Auditing/
│   │   └── [Your Modules]/
│   ├── Playground/
│   │   ├── Playground.Api/
│   │   ├── Playground.Blazor/        (if api-blazor)
│   │   ├── FSH.Playground.AppHost/   (if aspire)
│   │   └── Migrations.PostgreSQL/
│   └── Tests/
│       ├── Architecture.Tests/
│       └── Integration.Tests/
├── terraform/                         (if terraform)
│   ├── modules/
│   └── apps/
├── scripts/
├── docker-compose.yml                 (if docker)
├── FSH.Framework.slnx
├── COPILOT_INSTRUCTIONS.md            ✨ NEW
├── COPILOT_AGENT_README.md            ✨ NEW
├── BEST_PRACTICES_AUDIT.md            ✨ NEW
└── README.md
```

---

## 📖 Usage Workflows

### Workflow 1: Quick Start

```bash
# 1. Create project
fsh new MyApp --preset quickstart

# 2. Navigate and explore
cd MyApp
cat COPILOT_INSTRUCTIONS.md

# 3. Build and run
dotnet build src/FSH.Framework.slnx
dotnet run --project src/Playground/Playground.Api

# 4. Next: Create your first feature
fsh generate feature GetProducts --template crud
```

### Workflow 2: Production SaaS

```bash
# 1. Create with production preset
fsh new MySaaS --preset production --fsh-version 10.0.0

# 2. Add custom modules
fsh generate module Subscriptions --template workflow
fsh generate module Notifications --template job
fsh generate module Billing --template integration

# 3. Generate features in each module
fsh generate feature CreateSubscription --template crud
fsh generate feature ProcessPayment --template integration

# 4. Generate entities
fsh generate entity Subscription
fsh generate entity Invoice

# 5. Build and test
dotnet build src/FSH.Framework.slnx
dotnet test src/FSH.Framework.slnx
```

### Workflow 3: Microservices

```bash
# 1. Create microservices structure
fsh new MyPlatform --type api --arch microservices

# 2. Generate service modules
fsh generate module Users
fsh generate module Products
fsh generate module Orders

# 3. Add complex features
fsh generate feature CompleteOrder --template workflow
fsh generate feature SearchProducts --template cached

# 4. Deploy with Docker/Aspire
dotnet run --project src/Playground/FSH.Playground.AppHost
```

### Workflow 4: Learning & Exploration

```bash
# 1. Get help
fsh help

# 2. Learn about templates
fsh help templates

# 3. See examples
fsh help examples

# 4. Understand patterns
fsh help patterns

# 5. Create sample project
fsh new Learning --preset quickstart --sample true

# 6. Study the code and documentation
cd Learning
cat COPILOT_INSTRUCTIONS.md
```

---

## 🎓 Implementation Notes

### For Developers

1. **Pattern Alignment**: All generated code follows COPILOT_INSTRUCTIONS.md
2. **Best Practices**: Architecture tests enforce generated code quality
3. **Customizable**: Templates can be customized in the CLI
4. **Documentation**: Every generated artifact includes inline documentation

### For AI Agents

1. **Code Generation**: Use `fsh generate` to scaffold consistently
2. **Pattern Reference**: Use `fsh help patterns` for guidance
3. **Examples**: Use `fsh help examples` for real-world patterns
4. **Validation**: All generated code passes architecture tests

---

## 🔧 Configuration

### Default Options

Create `fsh.config.json` in your project root:

```json
{
  "defaults": {
    "type": "api-blazor",
    "architecture": "monolith",
    "database": "postgres",
    "docker": true,
    "aspire": true,
    "git": true
  },
  "templates": {
    "modulePath": "src/Modules",
    "featurePath": "Features/v1",
    "domainPath": "Domain"
  }
}
```

---

## 📊 Command Matrix

| Goal | Command | Example |
|------|---------|---------|
| Create MVP | `fsh new` + preset quickstart | `fsh new MyApp --preset quickstart` |
| Create SaaS | `fsh new` + preset production | `fsh new MyApp --preset production` |
| Create Microservices | `fsh new` + arch microservices | `fsh new MyApp --arch microservices` |
| Create Serverless | `fsh new` + arch serverless | `fsh new MyApp --arch serverless` |
| Add simple CRUD | `fsh generate feature` + template crud | `fsh generate feature GetProducts --template crud` |
| Add workflow | `fsh generate feature` + template workflow | `fsh generate feature ApproveOrder --template workflow` |
| Add module | `fsh generate module` | `fsh generate module Notifications --template job` |
| Get help | `fsh help` | `fsh help patterns` |

---

## ✅ Quality Assurance

### Generated Code Quality

All generated code is:
- ✅ **Consistent**: Follows COPILOT_INSTRUCTIONS.md patterns
- ✅ **Tested**: Passes architecture tests
- ✅ **Documented**: Includes inline documentation
- ✅ **Scalable**: Ready for production
- ✅ **Maintained**: Updates with CLI versions

### Validation

```bash
# After generation
dotnet build src/FSH.Framework.slnx
dotnet test src/Tests/Architecture.Tests
```

---

## 🚀 Roadmap

### Phase 1 (Current)
- ✅ Enhanced `fsh new` with better UX
- ✅ New `fsh generate` command
- ✅ New `fsh help` command
- ✅ Full documentation

### Phase 2 (Planned)
- 🔄 Integrate code generation with templates
- 🔄 Add `fsh config` command
- 🔄 Add `fsh validate` command
- 🔄 Add `fsh doctor` command (diagnose issues)

### Phase 3 (Future)
- 🔄 Interactive module builder
- 🔄 Code refactoring tools
- 🔄 Migration helpers
- 🔄 Performance analyzer

---

## 🔗 Integration Points

### With COPILOT_INSTRUCTIONS.md
- All generated code follows patterns documented there
- Naming conventions aligned
- Validation rules auto-included
- Multi-tenancy enforced

### With Architecture Tests
- Generated code passes architecture tests
- Module isolation enforced
- Version dependencies checked
- Namespace conventions verified

### With BuildingBlocks
- Uses latest BuildingBlocks version
- Leverages all available infrastructure
- Multi-tenancy integration
- Event-driven setup

---

## 📝 Examples

### Example 1: E-Commerce Platform

```bash
# Create base
fsh new EcommerceHub --preset production --type api-blazor

cd EcommerceHub

# Add modules
fsh generate module Products --template cached
fsh generate module Orders --template workflow
fsh generate module Payments --template integration
fsh generate module Inventory --template crud
fsh generate module Notifications --template job

# Add features
fsh generate feature SearchProducts
fsh generate feature PlaceOrder --template workflow
fsh generate feature ProcessPayment --template integration

# Generate entities
fsh generate entity Product
fsh generate entity Order
fsh generate entity Payment

# Build
dotnet build src/FSH.Framework.slnx
dotnet test src/FSH.Framework.slnx
```

### Example 2: Monitoring & Analytics SaaS

```bash
# Create base
fsh new AnalyticsPro --preset production

cd AnalyticsPro

# Add modules
fsh generate module Events --template cached
fsh generate module Reports --template reporting
fsh generate module Dashboards --template crud
fsh generate module Integrations --template integration
fsh generate module Exports --template job

# Build
dotnet build src/FSH.Framework.slnx
```

### Example 3: Serverless Microservices

```bash
# Create base
fsh new ServerlessPlatform --type api --arch serverless --terraform

cd ServerlessPlatform

# Add Lambda-friendly modules
fsh generate module WebhookProcessors --template job
fsh generate module DataTransformers --template job
fsh generate module APIGateway --template crud

# Deploy
terraform plan
terraform apply
```

---

## 📞 Support & Feedback

- **Documentation**: See README.md in CLI directory
- **Issues**: GitHub Issues
- **Examples**: Use `fsh help examples`
- **Patterns**: Use `fsh help patterns`
- **Questions**: Check COPILOT_INSTRUCTIONS.md in generated project

---

## 🎯 Success Criteria

After using FSH.CLI, you should have:

- ✅ A fully functional FullStackHero project
- ✅ All best practices implemented
- ✅ Architecture tests passing
- ✅ Comprehensive documentation
- ✅ Ready-to-use module templates
- ✅ Professional code generation
- ✅ Clear development path

---

**Last Updated**: December 29, 2025  
**Version**: 2.0.0  
**Status**: ✅ COMPLETE
