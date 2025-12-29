# Quick Start Guide for AI Copilot Agents

> **For Copilot Agents**: This is your entry point when working with this codebase.

## 🎯 Before Making ANY Changes

1. **Read First**: `/COPILOT_INSTRUCTIONS.md` (REQUIRED)
2. **Reference**: `/CLAUDE.md` for build/run commands
3. **Deep Dive**: `/ARCHITECTURE_GUIDE.md` for understanding
4. **Templates**: `/MODULE_TEMPLATES.md` for implementation patterns

## 📋 Quick Decision Tree

```
What are you doing?
│
├─ Adding a new feature to existing module?
│  └─ Read: COPILOT_INSTRUCTIONS.md → Feature Implementation Pattern
│
├─ Creating a new module?
│  └─ Read: MODULE_TEMPLATES.md → Choose template
│
├─ Fixing a bug?
│  └─ Read: COPILOT_INSTRUCTIONS.md → Understand patterns first
│
├─ Refactoring code?
│  └─ Read: COPILOT_INSTRUCTIONS.md → Check anti-patterns
│
└─ Modifying database?
   └─ Read: COPILOT_INSTRUCTIONS.md → DbContext Pattern
```

## ✅ Pre-Change Checklist

Before making changes, ensure you understand:

- [ ] Vertical Slice Architecture
- [ ] CQRS Pattern (Commands vs Queries)
- [ ] Mediator Pattern (not MediatR!)
- [ ] Multi-Tenancy (IMustHaveTenant)
- [ ] Endpoint Pattern (static extension methods)
- [ ] Validation Pattern (FluentValidation)
- [ ] Naming Conventions
- [ ] Module Structure

## 🚀 Common Tasks

### Task 1: Add a New Feature to Existing Module

**Steps**:
1. Create command/query in `Modules.{Module}.Contracts/v1/{Feature}/`
2. Create validator in `Modules.{Module}/Features/v1/{Feature}/`
3. Create handler in `Modules.{Module}/Features/v1/{Feature}/`
4. Create endpoint in `Modules.{Module}/Features/v1/{Feature}/`
5. Register endpoint in `{Module}Module.cs`

**Reference**: COPILOT_INSTRUCTIONS.md → Feature Implementation Pattern

### Task 2: Add a New Module

**Steps**:
1. Choose template from MODULE_TEMPLATES.md (A-G)
2. Create directory structure
3. Create contracts project
4. Create implementation project
5. Create domain entities
6. Create DbContext
7. Create features (commands/queries/handlers/endpoints)
8. Create module registration class
9. Register in host application

**Reference**: MODULE_TEMPLATES.md → Full templates

### Task 3: Add Database Migration

**Steps**:
```bash
cd src/Playground/Migrations.PostgreSQL
dotnet ef migrations add {MigrationName} \
    --context {Module}DbContext \
    --startup-project ../Playground.Api
```

**Reference**: CLAUDE.md → Build & Run Commands

## 🎨 Code Patterns (Quick Reference)

### Command Pattern
```csharp
// Contracts
public record CreateEntityCommand(string Name) : ICommand<Guid>;

// Handler
public class CreateEntityCommandHandler : ICommandHandler<CreateEntityCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateEntityCommand cmd, CancellationToken ct)
    {
        // Implementation
    }
}

// Validator
public class CreateEntityCommandValidator : AbstractValidator<CreateEntityCommand>
{
    public CreateEntityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

// Endpoint
public static RouteHandlerBuilder MapCreateEntityEndpoint(this IEndpointRouteBuilder endpoints)
{
    return endpoints.MapPost("/entities", async (cmd, mediator, ct) =>
        TypedResults.Created($"/entities/{await mediator.Send(cmd, ct)}", default));
}
```

## 🚫 Anti-Patterns (AVOID)

```csharp
// ❌ DON'T
using MediatR;                              // Use Mediator
using FSH.Modules.Identity;                 // Use .Contracts
var users = db.Users.ToList();              // Use ToListAsync(ct)
[ApiController]                             // Use Minimal APIs
public class UsersController                // Use endpoints

// ✅ DO
using Mediator;
using FSH.Modules.Identity.Contracts;
var users = await db.Users.ToListAsync(ct);
public static RouteHandlerBuilder MapXxxEndpoint(...)
```

## 🧪 Testing

```bash
# Build
dotnet build src/FSH.Framework.slnx

# Architecture tests
dotnet test src/Tests/Architecture.Tests

# All tests
dotnet test src/FSH.Framework.slnx
```

## 📊 Status Check

Before submitting changes:

```bash
# Build
✅ dotnet build src/FSH.Framework.slnx

# Architecture tests
✅ dotnet test src/Tests/Architecture.Tests

# Code style
✅ No warnings in build output
```

## 🆘 Help

- **Architecture Questions**: Read ARCHITECTURE_GUIDE.md
- **Implementation Questions**: Read COPILOT_INSTRUCTIONS.md
- **Template Questions**: Read MODULE_TEMPLATES.md
- **Build Questions**: Read CLAUDE.md

## 📈 Audit Status

Last audit: 2025-12-29  
Status: ✅ PASSED (98/100)  
Report: BEST_PRACTICES_AUDIT.md

---

**Remember**: When in doubt, look at existing code in the Identity module! 🎯
