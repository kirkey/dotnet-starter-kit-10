# Bug Fixes Summary - January 3, 2026

## Issues Fixed

### 1. **DateTimeOffset Timezone Incompatibility with PostgreSQL** ✅
**Problem**: When updating Todo items with a DueDate, the application was sending `DateTimeOffset` with +08:00 offset, but PostgreSQL's `timestamp with time zone` only accepts UTC (offset 0).

**Error**: 
```
System.ArgumentException: Cannot write DateTimeOffset with Offset=08:00:00 to PostgreSQL type 'timestamp with time zone', only offset 0 (UTC) is supported.
```

**Files Fixed**:
- `/src/Modules/Todo/Modules.Todo/Domain/Todo.cs`

**Changes Made**:
1. Updated `Todo.Create()` factory method to convert DueDate to UTC: `DueDate = dueDate?.ToUniversalTime()`
2. Updated `Todo.Update()` method to convert DueDate to UTC: `DueDate = dueDate?.ToUniversalTime()`

**Impact**: All Todo DueDate values are now normalized to UTC before being saved to the database, ensuring compatibility with PostgreSQL.

---

### 2. **Static Assets Authorization** ✅
**Problem**: Static assets like `/assets/defaults/profile-picture.webp` were being blocked by JWT authorization, causing 401 Unauthorized errors even though they should be publicly accessible.

**Errors**:
```
[13:46:11 ERR] Exception at /assets/defaults/profile-picture.webp - Unauthorized access
```

**Files Fixed**:
- `/src/Modules/Identity/Modules.Identity/Authorization/PathAwareAuthorizationHandler.cs`

**Changes Made**:
Added `/assets` to the `allowedPaths` array to bypass JWT authorization for all static asset files.

```csharp
PathString[] allowedPaths = new[]
{
    new PathString("/scalar"),
    new PathString("/openapi"),
    new PathString("/favicon.ico"),
    new PathString("/assets")  // ← Added this path
};
```

**Impact**: Static assets are now accessible without authentication, improving user experience and reducing unnecessary error logs.

---

## Testing Recommendations

1. **Test Todo Update with DueDate**:
   - Create a Todo with a DueDate in a non-UTC timezone
   - Verify it saves successfully to PostgreSQL
   - Verify the DueDate is stored correctly as UTC

2. **Test Static Asset Access**:
   - Navigate to profile pages to verify profile pictures load without authorization errors
   - Check browser console for 401 errors on static asset requests

3. **Build Verification**:
   - Run: `dotnet build` to ensure all changes compile successfully
   - Run: `dotnet test` to verify unit tests pass

---

## Related Issues Noted (Not Critical)

1. **Database Migration Pending**: The log shows "The model for context 'TenantDbContext' has pending changes" but this doesn't prevent the application from running.

2. **Unauthorized API Access**: Several API endpoints return 401 when accessed without valid JWT token. This is expected behavior but indicates token refresh issues in the Blazor client - user needs to re-authenticate.

---

## Files Modified Summary

| File | Change Type | Details |
|------|-------------|---------|
| `Todo.cs` | Logic Update | Added UTC conversion for DateTimeOffset fields |
| `PathAwareAuthorizationHandler.cs` | Configuration | Added `/assets` path to authorization bypass list |

---

## Environment Information

- **Database**: PostgreSQL (requires UTC DateTimeOffset)
- **.NET Version**: .NET 10
- **Date Fixed**: January 3, 2026
- **Build Status**: ✅ Should compile successfully after these changes

