# 🤖 Copilot Agent Documentation Hub

This directory contains comprehensive guidance for AI Copilot Agents working with the FSH .NET 10 Starter Kit.

## 📚 Documentation Structure

### 🎯 Quick Start (Start Here!)
- **[COPILOT_QUICKSTART.md](COPILOT_QUICKSTART.md)** - Your entry point
  - Quick decision tree
  - Pre-change checklist
  - Common tasks with examples
  - Anti-patterns to avoid

### 📖 Core Instructions (READ BEFORE CHANGES!)
- **[COPILOT_INSTRUCTIONS.md](COPILOT_INSTRUCTIONS.md)** - Comprehensive patterns guide
  - Architecture patterns
  - Module structure
  - Feature implementation (CQRS)
  - Domain entities
  - DbContext patterns
  - Validation rules
  - Naming conventions
  - Anti-patterns
  - ~25,000 words of detailed guidance

### 🏗️ Architecture Deep Dive
- **[ARCHITECTURE_GUIDE.md](src/ARCHITECTURE_GUIDE.md)** - Complete architecture documentation
  - High-level architecture
  - Technology stack
  - Project structure
  - Core concepts
  - How to add new features (step-by-step)
  - Scaling strategy
  - Best practices

### 🎨 Module Templates
- **[MODULE_TEMPLATES.md](src/MODULE_TEMPLATES.md)** - Decision trees and templates
  - Template A: Basic CRUD Module (2-3h)
  - Template B: Workflow Module (4-6h)
  - Template C: File Module (3-4h)
  - Template D: Integration Module (4-5h)
  - Template E: Cached Module (3-4h)
  - Template F: Reporting Module (4-6h)
  - Template G: Job Module (2-3h)

### ⚡ Quick Reference
- **[CLAUDE.md](CLAUDE.md)** - Build & run commands
  - Build/test commands
  - Module patterns
  - Endpoint patterns
  - Configuration
  - Blazor UI components

### 📊 Audit Report
- **[BEST_PRACTICES_AUDIT.md](BEST_PRACTICES_AUDIT.md)** - Latest audit status
  - Overall score: 98/100 ⭐⭐⭐⭐⭐
  - Architecture patterns audit
  - Security checklist
  - Performance checklist
  - Recommendations

## 🎯 Recommended Reading Order

### For First-Time Contributors
```
Day 1: CLAUDE.md + COPILOT_QUICKSTART.md (30 min)
Day 2: COPILOT_INSTRUCTIONS.md (1-2 hours)
Day 3: Study existing modules (2-3 hours)
Day 4: ARCHITECTURE_GUIDE.md (1-2 hours)
Day 5: Try implementing Template A (2-3 hours)
```

### For Experienced Developers
```
Step 1: COPILOT_QUICKSTART.md (10 min)
Step 2: Skim COPILOT_INSTRUCTIONS.md (30 min)
Step 3: Refer to specific sections as needed
```

### For Quick Changes
```
1. Read relevant section in COPILOT_INSTRUCTIONS.md
2. Check MODULE_TEMPLATES.md if creating new module
3. Look at similar code in Identity module
4. Make changes following patterns
5. Run architecture tests
```

## 🚀 Quick Start Workflow

### Making Changes
```bash
# 1. Read documentation
cat COPILOT_QUICKSTART.md

# 2. Make changes following patterns
# (See COPILOT_INSTRUCTIONS.md)

# 3. Build
dotnet build src/FSH.Framework.slnx

# 4. Test
dotnet test src/Tests/Architecture.Tests

# 5. Verify
git status
git diff
```

## ✅ Pre-Change Checklist

Before making ANY changes:

- [ ] Read COPILOT_QUICKSTART.md
- [ ] Read relevant section in COPILOT_INSTRUCTIONS.md
- [ ] Understand the pattern you're implementing
- [ ] Check MODULE_TEMPLATES.md if creating new module
- [ ] Look at similar code in existing modules

## 🎓 Key Concepts to Understand

### Architecture
- ✅ Modular Monolith
- ✅ Vertical Slice Architecture
- ✅ CQRS (Command/Query Separation)
- ✅ Event-Driven (Outbox/Inbox)
- ✅ Multi-Tenancy (Database-per-tenant)

### Patterns
- ✅ Mediator Pattern (not MediatR!)
- ✅ Repository Pattern (via DbContext)
- ✅ Specification Pattern
- ✅ Domain Events
- ✅ Integration Events

### Best Practices
- ✅ Minimal APIs (no controllers)
- ✅ Static extension methods for endpoints
- ✅ FluentValidation for all commands
- ✅ Async/await with CancellationToken
- ✅ Proper multi-tenancy (IMustHaveTenant)

## 🚫 Common Mistakes to Avoid

### ❌ DON'T DO THIS
```csharp
using MediatR;                          // Use Mediator
using FSH.Modules.Identity;             // Use .Contracts only
public class UsersController            // Use Minimal APIs
var users = db.Users.ToList();          // Use ToListAsync(ct)
[ApiController]                         // No attribute needed
public class User                       // Add IMustHaveTenant
```

### ✅ DO THIS INSTEAD
```csharp
using Mediator;
using FSH.Modules.Identity.Contracts;
public static RouteHandlerBuilder MapXxxEndpoint(...)
var users = await db.Users.ToListAsync(ct);
// No attributes needed for Minimal APIs
public class User : IMustHaveTenant
```

## 📊 Quality Metrics

### Current Status (2025-12-29)
- **Overall Score**: 98/100 ⭐⭐⭐⭐⭐
- **Architecture Tests**: 5/5 PASSED ✅
- **Build Status**: SUCCESS ✅
- **Code Quality**: EXCELLENT ✅

### Key Metrics
- Modules: 3
- Endpoints: 48
- Validators: 20
- Handlers: 51
- Features (v1): 55
- DbContexts: 3
- Entity Configurations: 6

## 🔍 Finding Information

### "How do I add a new feature?"
→ Read: COPILOT_INSTRUCTIONS.md → Feature Implementation Pattern

### "How do I create a new module?"
→ Read: MODULE_TEMPLATES.md → Choose template (A-G)

### "What's the command to run tests?"
→ Read: CLAUDE.md → Build & Run Commands

### "How do I implement multi-tenancy?"
→ Read: COPILOT_INSTRUCTIONS.md → Domain Entity Pattern

### "What validation rules should I use?"
→ Read: COPILOT_INSTRUCTIONS.md → Validation Rules

### "How do I structure endpoints?"
→ Read: COPILOT_INSTRUCTIONS.md → Endpoint Pattern

### "What's the naming convention?"
→ Read: COPILOT_INSTRUCTIONS.md → Naming Conventions

## 🆘 Getting Help

### Documentation Priority
1. **Quick Question?** → COPILOT_QUICKSTART.md
2. **Implementation Details?** → COPILOT_INSTRUCTIONS.md
3. **Architecture Question?** → ARCHITECTURE_GUIDE.md
4. **New Module?** → MODULE_TEMPLATES.md
5. **Build/Run Commands?** → CLAUDE.md

### When in Doubt
Look at existing code in the **Identity module** - it's the reference implementation!

## 📈 Success Criteria

Your changes are ready when:

- ✅ Follows patterns in COPILOT_INSTRUCTIONS.md
- ✅ Architecture tests pass
- ✅ Build succeeds with no warnings
- ✅ Uses correct naming conventions
- ✅ Includes proper validation
- ✅ Implements multi-tenancy correctly
- ✅ Uses async/await with CancellationToken
- ✅ Has proper endpoint documentation

## 🎯 Golden Rules

1. **Always use Vertical Slice Architecture**
2. **Always implement CQRS (Command/Query separation)**
3. **Always include multi-tenancy (IMustHaveTenant)**
4. **Always use Mediator library (not MediatR)**
5. **Always validate with FluentValidation**
6. **Always use async/await with CancellationToken**
7. **Always follow naming conventions**
8. **Never reference other module implementations (only .Contracts)**
9. **Never put business logic in endpoints**
10. **Never skip architecture tests**

## 📞 Support Channels

- **GitHub Issues**: For bugs and feature requests
- **Documentation**: This directory
- **Code Examples**: Identity module
- **Architecture Tests**: `src/Tests/Architecture.Tests`

---

## 📝 Document Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2025-12-29 | Initial comprehensive documentation |

---

**Last Updated**: 2025-12-29  
**Next Review**: After major version updates

---

**Remember**: Quality over speed. Take time to understand the patterns before making changes! 🎯
