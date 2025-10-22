RBAC Overview

- Goals
  - Enforce role-based, permission-driven access across admin APIs.
  - Keep Admin as “full access” by granting all permissions, not bypassing checks.
  - Make permissions scalable via AppModule + Action pairs and dynamic policies.

- Key Concepts
  - Modules: values from `Core.Enums.AppModule` (e.g., Product, Category, Brand, Size, Color, PriceAdjustment, ProductSku, Image, Order, Basket).
  - Actions: fixed set: Read, Create, Update, Delete, Manage.
  - Permission Name: `${Module}.${Action}` (e.g., `Category.Read`).
  - Policy Name: `Permission:${Module}.${Action}` used by `[Authorize(Policy = "...")]`.
  - Manage Implies All: `Module.Manage` authorizes all actions in that module.

What Changed

- Claims
  - Added `ClaimTypes.NameIdentifier` to access tokens so the authorization handler can resolve the user id reliably (MapInboundClaims is disabled).

- Authorization
  - Dynamic policy provider handles any `Permission:*` policy by creating a requirement for the targeted permission.
  - Controllers secured with permission policies for Read/Create/Update/Delete.
    - `src/API/Controllers/CategoryController.cs`
    - `src/API/Controllers/ProductController.cs`
    - `src/API/Controllers/ProductBrandController.cs`
    - `src/API/Controllers/SizeController.cs`
    - `src/API/Controllers/ColorController.cs`
    - `src/API/Controllers/PriceAdjustmentController.cs`
    - `src/API/Controllers/ProductSkuController.cs`
    - `src/API/Controllers/ImageController.cs`
    - `src/API/Controllers/PermissionController.cs` (admin-only CRUD)

- Permissions Data
  - Standardized permission `Name` as `${Module}.${Action}`.
  - Enforced unique index on `Permission.Name` in EF config: `builder.HasIndex(p => p.Name).IsUnique()`.
  - Seeder builds a full permission catalog from `AppModule` x Actions and assigns all to the `Admin` role.

- Caching
  - User permission cache key: `user_permissions_{userId}`.
  - Role edits now invalidate cached permissions for users in that role in `RoleService`.
  - Consider distributed cache if running multiple instances.

How It Works

- Dynamic Policies
  - Request `[Authorize(Policy = "Permission:Category.Create")]`.
  - `DynamicAuthorizationPolicyProvider` creates a policy that requires `Category.Create`.
  - `PermissionAuthorizationHandler` resolves `userId` from `NameIdentifier` and calls `IPermissionService.UserHasPermissionAsync`.

- Manage Implies All
  - When checking `Module.Action`, the service also honors `Module.Manage`.
  - Example: `Permission:Product.Update` passes if user has either `Product.Update` or `Product.Manage`.

Admin = Full Access

- No bypass in code. Admin gets all permissions via seeding, so all policy checks pass.
- As you add modules to `AppModule`, the seeder creates the new permissions and assigns them to Admin automatically on next run.

Developer Guide

- Protecting Endpoints
  - Read: `[Authorize(Policy = "Permission:Category.Read")]`
  - Create: `[Authorize(Policy = "Permission:Category.Create")]`
  - Update: `[Authorize(Policy = "Permission:Category.Update")]`
  - Delete: `[Authorize(Policy = "Permission:Category.Delete")]`
  - Manage: `[Authorize(Policy = "Permission:Category.Manage")]` (optional if you want single super-action per module)

- Public vs Secure
  - If most endpoints should be secure-by-default, configure a fallback policy in `AddAuthorization` and mark public endpoints `[AllowAnonymous]`.
  - Otherwise, decorate only admin endpoints with `[Authorize]` and permission policies.

- Data Migrations
  - The new unique index on `Permission.Name` requires a migration: `dotnet ef migrations add PermissionNameUniqueIndex` then update the database.

- Testing
  - Unauthenticated → 401, Authenticated without permission → 403, With permission → success.
  - Cover at least one controller per module (create/update/delete) in integration tests.

Operational Notes

- Auditing (recommended): log role/permission changes with who/when/what.
- Authorization failure logs: include policy and permission name for diagnosis.
- Token TTL and revocation: if adding permission claims to tokens in the future, keep TTL short or add a “permissions version” claim so revocations propagate.

File Pointers

- Token NameIdentifier claim: `src/API/Services/TokenService.cs`
- Dynamic policies: `src/API/Helpers/Authorizations/DynamicAuthorizationPolicyProvider.cs`
- Permission handler: `src/API/Helpers/Authorizations/PermissionAuthorizationHandler.cs`
- Permission checks + cache: `src/API/Services/PermissionService.cs`
- Role edits + cache invalidation: `src/API/Services/RoleService.cs`
- Unique index: `src/Infrastructure/Data/Configurations/PermissionConfiguration.cs`
- Seeding permissions + Admin: `src/Infrastructure/Data/ApplicationDbContextSeed.cs`

Next Steps

- Run migrations to apply the unique index change.
- Verify seeding runs (first app start) and Admin receives all permissions.
- Decide on fallback policy usage and annotate public endpoints `[AllowAnonymous]` if enabling it.
