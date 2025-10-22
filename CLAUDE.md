# Project Overview

ECommerce_NET is a layered .NET API for an e‑commerce domain. It provides product/catalog management, baskets, orders and payments, along with account authentication, role/permission checks, and response caching. The solution is split into API (presentation), Core (domain abstractions), and Infrastructure (data access and integrations). The API exposes CRUD endpoints for core catalog entities, order creation with delivery methods, Stripe payment intent handling, and sessioned JWT auth with secure refresh tokens.


# Tech Stack

- Runtime: .NET 9 (C#)
- Web: ASP.NET Core Web API, Swagger/OpenAPI
- Data: Entity Framework Core (Npgsql), ASP.NET Core Identity
- Cache/Message: Redis via StackExchange.Redis
- Auth: JWT bearer, cookie‑backed refresh tokens, dynamic permission policies
- Mapping: Mapster
- Payments: Stripe (payment intents + webhook)
- Patterns: Repository + Unit of Work, Specification pattern, middleware‑based error handling


# Architecture & Key Files

Layered solution with clear separations:

- API (presentation, DI composition, endpoints, middleware)
  - Entry: `src/API/Program.cs`
    - Registers Core/Infrastructure/API services, configures middleware, applies migrations and seed data at startup.
  - Composition: `src/API/Extensions/ConfigureServices.cs`
    - Adds controllers (kebab‑case routing), Swagger, CORS, JWT auth/authorization, DI registrations for services.
  - Middleware:
    - `src/API/Middlewares/ExceptionMiddleware.cs` – unifies errors into structured API responses.
    - `src/API/Middlewares/SpaOriginValidationMiddleware.cs` – origin/referer checks for sensitive cookie endpoints.
    - `src/API/Middlewares/RefreshRateLimitMiddleware.cs` – basic IP rate limiting for refresh endpoint.
  - Responses: `src/API/Commons/Response/*` – `ApiResponse`, `ApiSuccessResponse`, `ApiErrorResponse` and `ResponseFactory` for consistent envelopes.
  - Controllers (route base `api/[controller]`, kebab‑cased):
    - Accounts: `AccountController`
    - Basket: `BasketController`
    - Catalog: `ProductController`, `ProductSkuController`, `ProductBrandController`, `CategoryController`, `ColorController`, `SizeController`, `ImageController`
    - Orders/Payments: `OrderController`, `PaymentController`
    - Admin/ACL: `PermissionController`
    - Diagnostics: `ErrorController`, `BuggyController`
  - Mapping helpers: Mapster registration (`src/API/Helpers/MappingConfig.cs`).
  - AuthZ helpers: `src/API/Helpers/Authorizations/*` – dynamic policy provider and permission handler.
  - Config: `src/API/appsettings.json`, `appsettings.Development.json`, `Properties/launchSettings.json`.

- Core (domain model and abstractions)
  - Entities: catalog (Product, ProductBrand, Category, Color, Size, ProductSku, Image), orders (Order, OrderItem, DeliveryMethod), baskets (CustomerBasket), identity (AppUser, AppRole, RefreshToken, Permission, RolePermission), inventory/returns.
  - DTOs: request/response models for entities and workflows (Products, ProductSkus, Categories, Colors, Sizes, PriceAdjustments, Images, Orders, Accounts, Basket).
  - Interfaces: `IGenericRepository<>`, `IUnitOfWork`, service interfaces (e.g., `IProductService`, `IOrderService`, `IAccountService`, `IPermissionService`, etc.).
  - Specifications: strongly‑typed query objects for filtering/sorting/paging entities (e.g., Products, ProductSkus, Sizes, PriceAdjustments, Orders, Accounts).
  - Common: pagination, enums, messages, base entity contracts (auditing, soft‑delete).

- Infrastructure (data access and external integrations)
  - DbContext: `src/Infrastructure/Data/ApplicationDbContext.cs` – Identity + domain sets, soft delete filters, auditing, global identity value generators, configuration discovery.
  - EF Configurations: `src/Infrastructure/Data/Configurations/*` – entity mappings.
  - Repositories: `GenericRepository<>`, `BasketRepository`, `RolePermissionRepository`, plus `UnitOfWork`.
  - Specification evaluator: transforms specifications into EF Core queries.
  - Seeding: `ApplicationDbContextSeed` reads JSON seed files for initial data (brands, categories, sizes, colors, products, deliveries).
  - Composition: `src/Infrastructure/ConfigureServices.cs` – PostgreSQL (Npgsql), Redis connection multiplexer, Identity store, repo registrations.
  - Packages: EF Core, Npgsql, StackExchange.Redis, Stripe.net.

Services (API/business layer implementations):

- Account/Token: `AccountService`, `TokenService` – authentication, refresh token rotation, session management, JWT issuance.
- Catalog: `ProductService`, `ProductSkuService`, `ProductBrandService`, `CategoryService`, `ColorService`, `SizeService`, `ImageService`.
- Orders/Payments: `OrderService` (order creation, querying), `PaymentService` (Stripe intents, order status updates).
- Admin/ACL: `PermissionService`, `RoleService`, `ResponseCacheService` (Redis‑based response cache for `[Cached]` attribute).


# Setup & Run Instructions

Prerequisites

- .NET SDK 9.0
- PostgreSQL (local or container)
- Redis (docker compose provided)

Configuration

- Base settings are in `src/API/appsettings.json`. Override in `appsettings.Development.json` or environment variables for production.
- Important keys:
  - `ConnectionStrings:DefaultConnection` – PostgreSQL connection (Npgsql).
  - `ConnectionStrings:Redis` – Redis server (e.g., `localhost:6379`).
  - `Token:Key` – symmetric JWT signing key.
  - `Token:Issuer`, `Token:Audience`, `Token:AccessTokenExpiration`, `Token:RefreshTokenExpirationDays`, `Token:MaxSessionsPerUser`.
  - `Spa:Origin` – expected SPA origin for cookie‑guarded endpoints (refresh/logout). Default fallback: `https://localhost:4200`.
  - `StripeSettings:SecretKey`, `StripeSettings:PublishableKey` – Stripe credentials.
  - `Security:RefreshRateLimitPerMinute` – per‑IP throttle for refresh.

Local services

- Redis: `docker compose up -d` (uses `docker-compose.yml`; exposes Redis at `localhost:6379`, optional Redis Commander at `localhost:8081`).
- PostgreSQL: run locally or via container, e.g.:
  - `docker run -e POSTGRES_PASSWORD=postgres -e POSTGRES_USER=admin -e POSTGRES_DB=shopdb -p 5432:5432 -d postgres:16`
  - Update `ConnectionStrings:DefaultConnection` accordingly.

Run the API

1) Restore/build: `dotnet restore` then `dotnet build` (root or project).
2) Start API: `dotnet run --project src/API`.
   - The app applies EF migrations and seeds data on startup.
   - Swagger UI: `http://localhost:5229/swagger` (per `launchSettings.json`).

Notes

- For production, do not commit secrets; prefer environment variables or secret stores. Avoid hardcoded Stripe webhook secrets.
- Ensure CORS and `Spa:Origin` match your frontend origin to pass origin/referer checks on cookie‑backed routes.


# API or Feature Summary

Conventions

- Base route: `api/[controller]` with kebab‑case transformer (e.g., `ProductSkuController` => `api/product-sku`). Some controllers override with explicit routes.
- Responses are wrapped in `ApiSuccessResponse<T>` or `ApiErrorResponse` with consistent `statusCode`, `message`, `succeeded` fields.
- Filtering/paging generally uses `SpecParams` objects (Specification pattern) and returns `Pagination<T>`.

Authentication & Sessions

- JWT Bearer for access tokens; refresh tokens are httpOnly, `SameSite=None`, `Secure` cookies (`rt`) scoped to `POST /api/account/refresh`.
- Middlewares:
  - SPA origin enforcement for `POST /api/account/refresh`, `POST /api/account/logout`, `GET/POST /api/account/sessions*`.
  - Basic per‑IP rate limiting for refresh.
- Selected endpoints:
  - `POST /api/account/login` – returns access token and sets `rt` cookie.
  - `POST /api/account/register` – creates user, returns tokens, sets `rt` cookie.
  - `GET /api/account` – current user (auth required).
  - `GET /api/account/email-exist?email=...` – availability check.
  - `POST /api/account/refresh` – rotates refresh token, returns new access token.
  - `POST /api/account/logout` – revokes current refresh token and clears cookie.
  - `POST /api/account/logout-all` – revoke all sessions for user (auth required).
  - `GET /api/account/sessions` – list sessions for user; revoke via:
    - `POST /api/account/sessions/{sessionId}/revoke`
    - `POST /api/account/sessions/revoke-others`

Catalog

- Products (`api/product`)
  - `POST /get-all-by-id` – paged products by filters (`categoryId`, `productBrandId`, `name`, flags).
  - `POST /get-all` – paged filtered by name.
  - `GET /{id}` – single product.
  - `POST /create`, `POST /update` – upsert via `CreateProductDto`.
  - `DELETE /{id}` – delete.

- Product SKUs (`api/product-sku`)
  - `POST /get-all` – paged by `productId`.
  - `GET /{id}` – sku detail.
  - `POST /create`, `POST /update`, `DELETE /{id}`.

- Brands (`api/brand`)
  - `GET /{id}` – brand detail.
  - `POST /get-all` – paged list.
  - `POST /create`, `POST /update`, `POST /create-many`, `DELETE /{id}`, `DELETE /delete-many`.

- Categories (`api/category`)
  - `GET /{id}` – category detail.
  - `POST /get-all` – paged list.
  - `GET /hierarchy` – nested category tree.
  - `POST /create`, `POST /update`, `POST /create-many`, `DELETE /{id}`.

- Colors (`api/color`)
  - `GET /{id}`, `POST /get-all`, `POST /create`, `POST /update`, `POST /create-many`, `DELETE /{id}`.

- Sizes (`api/size`)
  - `GET /{id}`, `POST /get-all` (supports name, type, sort range), `POST /create`, `POST /update`, `POST /create-many`, `DELETE /{id}`, `DELETE /delete-many`.

- Images (`api/image`)
  - `POST /process` – bulk image processing (`CreateListImageDto`).

Basket (`api/basket`)

- `GET /?id={basketId}` – fetch basket.
- `POST /` – upsert basket.
- `DELETE /?id={basketId}` – delete basket.

Orders (`api/order`, auth required)

- `POST /` – create/replace order for basket and delivery method.
- `GET /` – user’s orders.
- `GET /{id}` – user’s order by id.
- `GET /delivery-methods` – available methods.

Payments (`api/payment`)

- `POST /{basketId}` – create/update Stripe PaymentIntent using current basket.
- `POST /webhook` – Stripe webhook: updates order status to `PaymentReceived` or `PaymentFailed`.

Permissions/ACL (`api/permission`)

- `GET /{id}`, `POST /get-all`, `POST /create`, `POST /update`, `DELETE /{id}`.
- Authorization policies can be declared dynamically as `Policy="Permission:SomeName"` and are enforced through `PermissionAuthorizationHandler`.

Error Handling

- All unhandled exceptions are converted to `ApiErrorResponse` via `ExceptionMiddleware`.
- Status code pages are routed to `GET /errors/{code}` for consistent formatting.

Caching

- `ResponseCacheService` stores serialized responses in Redis keyed by request. The `[Cached(seconds)]` attribute can short‑circuit controller execution when cache hits; apply selectively to idempotent GETs.

Seeding & Migrations

- On startup the app runs `context.Database.MigrateAsync()` and seeds categories, brands, sizes, colors, products, and delivery methods when empty. Admin user and roles are also created if none exist.
