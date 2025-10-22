# RBAC Changes and Rationale

This document summarizes all RBAC-related updates, why they were made, and how to use them.

## Summary

- Secure-by-default API via fallback auth policy.
- Permission-based authorization using dynamic policies: `Permission:Module.Action`.
- Admin role gets full access by being assigned all permissions (no bypass logic).
- JWT token now includes `ClaimTypes.NameIdentifier` for stable user ID.
- Permission data hardened with a unique index and seeded catalog.
- Manage implies all within a module (`Module.Manage` covers Read/Create/Update/Delete).
- Permission cache invalidated on role changes.

## Changes Made

- Tokens
  - Added NameIdentifier claim for user ID lookups.
  - File: `src/API/Services/TokenService.cs`

- Authorization default
  - Enabled fallback policy: all endpoints require authentication unless `[AllowAnonymous]`.
  - File: `src/API/Extensions/ConfigureServices.cs`

- Permission policies on controllers (admin endpoints)
  - Category: Read/Create/Update/Delete
    - File: `src/API/Controllers/CategoryController.cs`
  - Product: Read/Create/Update/Delete
    - File: `src/API/Controllers/ProductController.cs`
  - Brand: Read/Create/Update/Delete
    - File: `src/API/Controllers/ProductBrandController.cs`
  - Size: Read/Create/Update/Delete
    - File: `src/API/Controllers/SizeController.cs`
  - Color: Read/Create/Update/Delete
    - File: `src/API/Controllers/ColorController.cs`
  - PriceAdjustment: Read/Create/Update/Delete
    - File: `src/API/Controllers/PriceAdjustmentController.cs`
  - ProductSku: Read/Create/Update/Delete
    - File: `src/API/Controllers/ProductSkuController.cs`
  - Image: Create (processing)
    - File: `src/API/Controllers/ImageController.cs`
  - Permission: Read/Create/Update/Delete
    - File: `src/API/Controllers/PermissionController.cs`

- Public endpoints (explicitly allowed)
  - Account: login, register, email-exist, refresh, logout
    - File: `src/API/Controllers/AccountController.cs`
  - Basket: get/update/delete
    - File: `src/API/Controllers/BasketController.cs`
  - Payment: Stripe webhook
    - File: `src/API/Controllers/PaymentController.cs`

- Manage implies all
  - If user has `Module.Manage`, they are authorized for any `Module.Action`.
  - File: `src/API/Services/PermissionService.cs`

- Permission catalog + Admin seeding
  - Seeded permissions = AppModule x {Read, Create, Update, Delete, Manage}.
  - Assigned all permissions to Admin role.
  - File: `src/Infrastructure/Data/ApplicationDbContextSeed.cs`

- Permission data integrity
  - Unique index on `Permission.Name` (e.g., `Category.Read`).
  - File: `src/Infrastructure/Data/Configurations/PermissionConfiguration.cs`

- Cache invalidation on role changes
  - After role permission updates, removes `user_permissions_{userId}` from cache for users in that role.
  - File: `src/API/Services/RoleService.cs`

## Why These Changes

- Fallback policy prevents accidental public exposure of new endpoints.
- NameIdentifier claim avoids claim-type mismatch (JWT had `sub`, handler expects `NameIdentifier`).
- Dynamic policies keep controllers simple and scalable.
- Seeding ensures Admin always has complete access without special-casing.
- Unique index prevents duplicate permissions; data consistency for lookups.
- Manage implies all reduces policy duplication for module owners.
- Cache invalidation ensures permission changes apply promptly to affected users.

## How to Use

- Protect endpoints
  - Read: `[Authorize(Policy = "Permission:Category.Read")]`
  - Create: `[Authorize(Policy = "Permission:Category.Create")]`
  - Update: `[Authorize(Policy = "Permission:Category.Update")]`
  - Delete: `[Authorize(Policy = "Permission:Category.Delete")]`
  - Manage: `[Authorize(Policy = "Permission:Category.Manage")]`

- Public endpoints
  - Must be explicitly marked: `[AllowAnonymous]`.

- Add new module
  - Add value to `Core.Enums.AppModule`.
  - On app start, seeder creates the 5 permissions and assigns them to Admin.

## Migration

- The new unique index on `Permission.Name` requires a DB migration.
  - `dotnet ef migrations add PermissionNameUniqueIndex`
  - `dotnet ef database update`

## Testing Tips

- Unauthenticated → 401 due to fallback policy (unless `[AllowAnonymous]`).
- Authenticated without permission → 403 from permission policy.
- Authenticated with permission → 200.
- Update role permissions and verify affected users lose/gain access without restart (cache invalidation).

## Related Files (for reference)

- Dynamic policies: `src/API/Helpers/Authorizations/DynamicAuthorizationPolicyProvider.cs`
- Authorization handler: `src/API/Helpers/Authorizations/PermissionAuthorizationHandler.cs`
- Permission checks + cache: `src/API/Services/PermissionService.cs`
- Role edits + cache invalidation: `src/API/Services/RoleService.cs`
- Seeding: `src/Infrastructure/Data/ApplicationDbContextSeed.cs`
- Unique index: `src/Infrastructure/Data/Configurations/PermissionConfiguration.cs`
