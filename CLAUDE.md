**Project Overview**

- ECommerce_NET is a layered .NET 9 Web API for an e-commerce platform. It provides product catalog, categories, sizes, colors, brands, orders, pricing adjustments, baskets (Redis-backed), image handling, and RBAC-based administration endpoints.
- The solution is organized into three projects:
  - `Core` (domain entities, DTOs, specifications, interfaces)
  - `Infrastructure` (EF Core, PostgreSQL, repositories, unit of work, Redis, migrations, seeding)
  - `API` (presentation layer: controllers, services, middleware, auth, response model, configuration)
- Key characteristics:
  - Repository + Unit of Work + Specification patterns for query composition and clean data access.
  - ASP.NET Core Identity for users/roles; permission-based authorization with dynamic policies (RBAC).
  - JWT access tokens + secure, cookie-backed refresh tokens with session management and rate limiting.
  - Redis used for shopping basket storage and response caching.
  - Mapster for fast DTO mapping; Swagger for API exploration in Development.


**Tech Stack**

- Runtime: .NET 9, ASP.NET Core Web API
- Data: EF Core with PostgreSQL; Migrations in `src/Infrastructure/Migrations`
- Identity/Auth: ASP.NET Core Identity (users/roles), JWT (HMAC-SHA256), cookie refresh tokens
- Caching/Queues: Redis via StackExchange.Redis; Redis Commander (optional) in `docker-compose.yml`
- Object Mapping: Mapster + MapsterMapper
- Payments: Stripe Webhooks (development example key present)
- Docs: Swagger (enabled in Development)


**Architecture & Key Files**

- Solution and Projects
  - `ECommerce.sln` – solution file
  - `src/Core/Core.csproj` – domain: entities, DTOs, specs, interfaces
  - `src/Infrastructure/Infrastructure.csproj` – persistence: EF Core, repositories, UoW, Redis, seed
  - `src/API/API.csproj` – Web API: controllers, services, middleware, auth/configuration

- API Composition
  - Startup/Composition Root: `src/API/Program.cs`
  - API DI and configuration: `src/API/Extensions/ConfigureServices.cs`
    - Controllers, Swagger, CORS, JWT bearer auth, fallback authorization policy, Mapster config
    - Dynamic permission policies and handler registration
  - Infrastructure DI: `src/Infrastructure/ConfigureServices.cs`
    - PostgreSQL DbContext, Redis connection, Identity, repositories, unit of work
  - Core DI: `src/Core/ConfigureServices.cs`

- Persistence & Domain
  - DbContext: `src/Infrastructure/Data/ApplicationDbContext.cs`
    - Soft-delete filter via `ISoftDeletable`/`BaseEntity.IsDeleted`
    - Identity integration; DbSet per aggregate; global configurations via `ApplyConfigurationsFromAssembly`
  - Seed & Permissions: `src/Infrastructure/Data/ApplicationDbContextSeed.cs`
    - Creates Admin/User roles, default admin, seeds permission catalog (AppModule x {Read,Create,Update,Delete,Manage}) and assigns to Admin
    - Seeds brands, colors, sizes, delivery methods from `SeedData`
  - Migrations: `src/Infrastructure/Migrations/*`
  - Entities: `src/Core/Entities/**` (e.g., `Product`, `Category`, `ProductBrand`, `Order`, `PriceAdjustment`, `ProductSku`, Identity types)
  - Specifications: `src/Core/Specifications/**` and evaluator `src/Infrastructure/Data/SpecificationEvaluator.cs`
  - Repositories/UoW: `src/Infrastructure/Data/Repositories/*.cs`, `src/Infrastructure/Data/UnitOfWork.cs`
  - Basket (Redis): `src/Infrastructure/Data/Repositories/BasketRepository.cs`

- Application Services and Mapping
  - Services: `src/API/Services/*.cs` (Product, Category, Color, Size, Brand, ProductSku, PriceAdjustment, Image, Order, Payment, Account, Permission, Role, Token, ResponseCache)
  - Mapster mapping configuration: `src/API/Helpers/MappingConfig.cs`

- API Endpoints (Controllers)
  - Base controller: `src/API/Controllers/ApiControllerBase.cs` with route `api/[controller]` and kebab-case transform
  - Feature controllers (see “API or Feature Summary” for endpoints)

- Security & Middleware
  - JWT Token Service: `src/API/Services/TokenService.cs` (adds standard claims; `NameIdentifier` included)
  - Permission policies (dynamic):
    - `src/API/Helpers/Authorizations/DynamicAuthorizationPolicyProvider.cs`
    - `src/API/Helpers/Authorizations/PermissionAuthorizationHandler.cs` (calls `IPermissionService`)
    - `src/API/Services/PermissionService.cs` (user permission resolution + caching, Manage implies all)
  - Fallback authorization policy (secure-by-default): `src/API/Extensions/ConfigureServices.cs`
  - Exception handling: `src/API/Middlewares/ExceptionMiddleware.cs` -> unified error responses
  - SPA origin validation for sensitive cookie endpoints: `src/API/Middlewares/SpaOriginValidationMiddleware.cs`
  - Refresh rate limiting for `/api/account/refresh`: `src/API/Middlewares/RefreshRateLimitMiddleware.cs`
  - Response model: `src/API/Commons/Response/ApiResponse.cs`, `ApiSuccessResponse`, `ApiErrorResponse`; factory in `src/API/Helpers/ResponseFactory.cs`
  - Route token transformer: `src/API/Helpers/KebabCaseTransformer.cs`
  - Optional response caching attribute: `src/API/Attributes/CachedAttribute.cs` + `ResponseCacheService`

- Configuration
  - `src/API/appsettings.json` – connection strings, JWT/Token settings, Stripe keys, `ApiUrl`
  - `src/API/appsettings.Development.json` – SPA origin override, logging
  - `src/API/Properties/launchSettings.json` – localhost HTTP/HTTPS URLs, Development environment
  - `docker-compose.yml` – Redis + Redis Commander services
  - RBAC notes: `RBAC_CHANGES.md`


**Setup & Run Instructions**

- Prerequisites
  - .NET 9 SDK
  - PostgreSQL (local or managed). Create database and user; update `DefaultConnection` in `src/API/appsettings.json` as needed.
  - Redis (local or via Docker). `docker-compose.yml` provides Redis and Redis Commander.
  - Optional: Stripe test keys if exercising payment webhook in non-dev contexts.

- Configuration
  - Update `src/API/appsettings.json`:
    - `ConnectionStrings:DefaultConnection` (PostgreSQL)
    - `ConnectionStrings:Redis` (Redis endpoint)
    - `Token:Key` (strong secret), `Token:Issuer`, `Token:Audience` (optional)
    - `Token:AccessTokenExpiration` (minutes), `Token:RefreshTokenExpirationDays`, `Token:MaxSessionsPerUser` (optional)
    - `Spa:Origin` in `src/API/appsettings.Development.json` if your frontend origin differs
    - Replace development Stripe keys and webhook secret if used beyond local testing

- Database
  - Apply migrations (from the repo root):
    - `dotnet restore`
    - `dotnet tool install --global dotnet-ef` (if EF CLI not installed)
    - `dotnet ef database update --project src/Infrastructure --startup-project src/API`
  - On first run, the app also runs `ApplicationDbContextSeed.SeedAsync` to create roles, admin user, permissions, and baseline data (sizes/colors/brands/delivery).

- Redis (Docker optional)
  - Start Redis and Commander: `docker compose up -d redis redis-commander`
  - Redis Commander UI: http://localhost:8081 (default credentials set in `docker-compose.yml`)

- Run the API
  - Development: `dotnet run --project src/API`
  - Swagger UI (Development): `https://localhost:7229/swagger` or `http://localhost:5229/swagger`

- Notes
  - Sensitive cookie endpoints (refresh/logout/sessions) require correct `Origin/Referer` matching `Spa:Origin` and are rate limited.
  - Replace the hard-coded Stripe webhook secret in `src/API/Controllers/PaymentController.cs` with a secure configuration source for non-local use.


**API or Feature Summary**

- Common Patterns
  - Base route: `api/{controller}` (kebab-case). Example: `ProductController` -> `api/product`.
  - Response envelope: success and error responses via `ApiSuccessResponse<T>` and `ApiErrorResponse`.
  - Pagination: `Core.Common.Pagination<T>` returned by search endpoints.
  - Authorization: all endpoints require authentication by default. Public endpoints use `[AllowAnonymous]`. Admin/management endpoints use policies: `Permission:{Module}.{Action}`.
  - Typical CRUD pattern:
    - `POST api/{resource}/search` – paginated search with `SpecParams`
    - `GET api/{resource}/{id}` – fetch by id
    - `POST api/{resource}/create` – create (DTO)
    - `POST api/{resource}/update` – update (DTO)
    - `DELETE api/{resource}/{id}` – delete by id

- Accounts and Auth (`api/account`)
  - `POST login` (anonymous): email/password -> JWT + HttpOnly refresh cookie (`rt`)
  - `POST register` (anonymous): creates user, returns tokens
  - `GET` (auth): current user profile
  - `GET email-exist?email=` (anonymous)
  - `POST refresh` (anonymous): rotates refresh token; origin-checked + rate limited
  - `POST logout` (anonymous): revokes current refresh token; origin-checked
  - `POST logout-all` (auth): revokes all sessions
  - `GET sessions` (auth): list user sessions
  - `POST sessions/{sessionId}/revoke` (auth): revoke one session
  - `POST sessions/revoke-others` (auth): revoke all other sessions

- Permissions and Roles
  - Permissions (`api/permission`) – requires permissions
    - `GET {id}` – `Permission:Permission.Read`
    - `POST search` – `Permission:Permission.Read`
    - `POST create` – `Permission:Permission.Create`
    - `POST update` – `Permission:Permission.Update`
    - `DELETE {id}` – `Permission:Permission.Delete`
  - Roles are managed via `RoleService` with repository methods for assignment; endpoints can be added consistently to follow the pattern. Permission cache invalidated on role updates.

- Catalog
  - Products (`api/product`) – `Permission:Product.*` for admin endpoints
    - `POST search` (filter by name/specs), `GET {id}`, `POST create`, `POST update`, `DELETE {id}`
  - Categories (`api/category`) – tree + CRUD
    - `POST search`, `GET {id}`, `POST create`, `POST update`, `DELETE {id}`
    - `GET hierarchy` pattern is supported via `CategoryService.GetCategoriesHierarchyAsync()` if exposed
  - Brands (`api/product-brand`) – CRUD
  - Colors (`api/color`) – batch create + CRUD
  - Sizes (`api/size`) – batch create + CRUD
  - Product Skus (`api/product-sku`) – CRUD
  - Images (`api/image`) – create/upload endpoints guarded by permissions

- Pricing and Orders
  - Price Adjustments (`api/price-adjustment`) – create/update and listing with spec params
  - Orders (`api/order`) – create/list/detail; integrates with payments
  - Payments (`api/payment`)
    - `POST {basketId}` (auth): create/update payment intent
    - `POST webhook` (anonymous): Stripe webhook (replace hard-coded secret for production)

- Basket (Redis-backed) (`api/basket`) – typically anonymous
  - `GET {id}`, `POST` (create/update), `DELETE {id}`

- Error Handling
  - Unified error responses and status codes via `ExceptionMiddleware` and `ResponseFactory`.

- Caching
  - Response cache service backed by Redis with `CachedAttribute` (can be applied to read endpoints as needed).


---

This document is intended to give another LLM a compact, accurate mental model of the repository, its architecture, security model, and operational requirements.

