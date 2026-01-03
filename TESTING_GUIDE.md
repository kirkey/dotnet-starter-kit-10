# Testing Guide - Bug Fixes

## Fix #1: DateTimeOffset UTC Conversion

### Manual Test Steps:

1. **Start the API**:
   ```bash
   cd /Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Apps/Apps.Api
   dotnet run
   ```

2. **Login and get a valid token**:
   ```bash
   curl -X POST https://localhost:7030/api/v1/identity/token/issue \
     -H "Content-Type: application/json" \
     -d '{"email":"admin@root.com","password":"123Pa$$word!","tenantId":"root"}'
   ```

3. **Update a Todo with DueDate** (using the token from step 2):
   ```bash
   curl -X PUT https://localhost:7030/api/v1/todo/{todoId} \
     -H "Authorization: Bearer {your_token}" \
     -H "Content-Type: application/json" \
     -d '{
       "id": "{todoId}",
       "name": "Updated Todo",
       "description": "Test update",
       "priority": 2,
       "dueDate": "2026-01-22T15:34:04.8556000+08:00"
     }'
   ```

4. **Expected Result**: 
   - ✅ 200 OK response (no 500 error)
   - ✅ Todo updated successfully
   - ✅ No "Cannot write DateTimeOffset" errors in logs

---

## Fix #2: Static Assets Authorization

### Manual Test Steps:

1. **Open browser DevTools** (F12) and go to Console tab

2. **Navigate to any profile page**:
   ```
   https://localhost:7140/profile
   ```

3. **Check for 401 errors in Console**:
   - ✅ No 401 errors for `/assets/defaults/profile-picture.webp`
   - ✅ Profile picture displays correctly
   - ✅ No red error messages about unauthorized asset access

4. **Alternative: Check Network tab**:
   - Open Developer Tools → Network tab
   - Look for requests to `/assets/*`
   - ✅ All asset requests should return 200 OK
   - ❌ Should NOT return 401 Unauthorized

---

## Automated Test Commands

### Build the Solution
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src
dotnet build -c Release
```

### Run Unit Tests
```bash
dotnet test
```

### Run Integration Tests (if available)
```bash
dotnet test --filter "Category=Integration"
```

---

## Expected Before/After Behavior

### Before Fix #1:
```
[13:46:47 ERR] Exception at /api/v1/todo/0c5a4819-2483-46be-89a7-0c29c2791eef - An unexpected error occurred
System.ArgumentException: Cannot write DateTimeOffset with Offset=08:00:00 to PostgreSQL type 'timestamp with time zone'
```

### After Fix #1:
```
✅ Todo updated successfully
[13:46:47 INF] Todo item updated: 0c5a4819-2483-46be-89a7-0c29c2791eef
```

---

### Before Fix #2:
```
[13:46:11 ERR] Exception at /assets/defaults/profile-picture.webp - Unauthorized access
```

### After Fix #2:
```
✅ Assets load without errors
✅ Profile pictures display correctly
```

---

## Database Impact

No database schema changes required. The fixes work with the existing schema:
- PostgreSQL `timestamp with time zone` column continues to work
- UTC conversion happens in the application layer before saving
- Existing data is not affected

---

## Rollback Instructions (if needed)

If you need to revert these changes:

1. **For DateTimeOffset fix**:
   ```bash
   git checkout src/Modules/Todo/Modules.Todo/Domain/Todo.cs
   ```

2. **For Assets Authorization fix**:
   ```bash
   git checkout src/Modules/Identity/Modules.Identity/Authorization/PathAwareAuthorizationHandler.cs
   ```

---

## Performance Impact

**Negligible** - Both fixes are minimal overhead:
1. UTC conversion is a single method call (`ToUniversalTime()`)
2. Path segment comparison is already part of the authorization pipeline

No database migration required, no performance degradation expected.

