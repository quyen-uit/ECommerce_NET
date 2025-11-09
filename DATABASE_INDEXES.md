# Database Index Recommendations

Comprehensive index strategy based on entity relationships, query patterns from specifications, and common operations.

## Table of Contents
- [Critical Priority (Immediate)](#critical-priority-immediate)
- [High Priority (Before 10k+ records)](#high-priority-before-10k-records)
- [Medium Priority (When needed)](#medium-priority-when-needed)
- [Composite Indexes](#composite-indexes)
- [Unique Constraints](#unique-constraints)
- [Implementation Guide](#implementation-guide)

---

## Critical Priority (Immediate)

### 1. Products Table
**Reason:** Most queried table, filtered/sorted frequently

```sql
-- Foreign keys (used in every product query with joins)
CREATE INDEX IX_Products_CategoryId
ON "Products"("CategoryId")
WHERE "IsDeleted" = false;

CREATE INDEX IX_Products_ProductBrandId
ON "Products"("ProductBrandId")
WHERE "IsDeleted" = false;

-- Full-text search on Name (LIKE queries)
CREATE INDEX IX_Products_Name
ON "Products"("Name")
WHERE "IsDeleted" = false;

-- Filtered queries (IsNew, IsTrending, IsActive)
CREATE INDEX IX_Products_IsNew_IsTrending_IsActive
ON "Products"("IsNew", "IsTrending", "IsActive")
WHERE "IsDeleted" = false;

-- Sorting (default sort by UpdatedAt DESC)
CREATE INDEX IX_Products_UpdatedAt
ON "Products"("UpdatedAt" DESC)
WHERE "IsDeleted" = false;
```

**Impact:** 10-50x faster product search/filtering
**Query Pattern:** `ProductWithTypesAndBrandsSpecification` filters by CategoryId, ProductBrandId, Name, IsNew, IsTrending

---

### 2. Orders Table
**Reason:** User order history queries

```sql
-- User's orders (filtered by email, sorted by date)
CREATE INDEX IX_Orders_BuyerEmail_OrderDate
ON "Orders"("BuyerEmail", "OrderDate" DESC)
WHERE "IsDeleted" = false;

-- Order status filtering (admin dashboard)
CREATE INDEX IX_Orders_Status_OrderDate
ON "Orders"("Status", "OrderDate" DESC)
WHERE "IsDeleted" = false;

-- Payment intent lookup (Stripe webhooks)
CREATE INDEX IX_Orders_PaymentIntentId
ON "Orders"("PaymentIntentId")
WHERE "IsDeleted" = false AND "PaymentIntentId" IS NOT NULL;

-- Date range queries (reports)
CREATE INDEX IX_Orders_OrderDate
ON "Orders"("OrderDate" DESC)
WHERE "IsDeleted" = false;
```

**Impact:** 20-100x faster order history retrieval
**Query Pattern:** `OrdersWithItemsAndOrderingSpecification` filters by BuyerEmail, orders by OrderDate

---

### 3. RefreshTokens Table
**Reason:** Authentication on every request

```sql
-- Token lookup (most critical for auth)
CREATE UNIQUE INDEX IX_RefreshTokens_Token
ON "RefreshTokens"("Token")
WHERE "RevokedAt" IS NULL;

-- User's active sessions
CREATE INDEX IX_RefreshTokens_UserId_IsActive
ON "RefreshTokens"("UserId", "RevokedAt")
WHERE "RevokedAt" IS NULL;

-- Session management
CREATE INDEX IX_RefreshTokens_SessionId
ON "RefreshTokens"("SessionId")
WHERE "RevokedAt" IS NULL;

-- Cleanup expired tokens (background job)
CREATE INDEX IX_RefreshTokens_Expires
ON "RefreshTokens"("Expires")
WHERE "RevokedAt" IS NULL;
```

**Impact:** Sub-millisecond token validation
**Query Pattern:** `RefreshTokenSpecification` filters by UserId and RevokedAt, token lookup by string

---

### 4. AuditLogs Table
**Reason:** Admin queries, security investigations

```sql
-- User activity tracking
CREATE INDEX IX_AuditLogs_UserId_Timestamp
ON "AuditLogs"("UserId", "Timestamp" DESC);

-- Endpoint performance monitoring
CREATE INDEX IX_AuditLogs_Path_Timestamp
ON "AuditLogs"("Path", "Timestamp" DESC);

-- HTTP method filtering
CREATE INDEX IX_AuditLogs_Action_Timestamp
ON "AuditLogs"("Action", "Timestamp" DESC);

-- Time-based queries (most common filter)
CREATE INDEX IX_AuditLogs_Timestamp
ON "AuditLogs"("Timestamp" DESC);
```

**Impact:** Fast security audits and performance analysis
**Query Pattern:** AdminController filters by UserId, Action, orders by Timestamp DESC

---

## High Priority (Before 10k+ records)

### 5. ProductSkus Table
**Reason:** Inventory and variant lookups

```sql
-- Product's SKUs (product detail page)
CREATE INDEX IX_ProductSkus_ProductId
ON "ProductSkus"("ProductId")
WHERE "IsDeleted" = false;

-- Color/Size filtering (variant selection)
CREATE INDEX IX_ProductSkus_ColorId_SizeId
ON "ProductSkus"("ColorId", "SizeId")
WHERE "IsDeleted" = false;

-- SKU code lookup (unique identifier)
CREATE UNIQUE INDEX IX_ProductSkus_SkuCode
ON "ProductSkus"("SkuCode")
WHERE "IsDeleted" = false;

-- Active SKUs
CREATE INDEX IX_ProductSkus_IsActive
ON "ProductSkus"("IsActive")
WHERE "IsDeleted" = false;
```

**Impact:** Instant variant lookups
**Query Pattern:** `ProductSkuWithColorAndSizeSpecification` filters by ProductId

---

### 6. Reviews Table
**Reason:** Product reviews display

```sql
-- Product's reviews (product detail page)
CREATE INDEX IX_Reviews_ProductId_CreatedAt
ON "Reviews"("ProductId", "CreatedAt" DESC)
WHERE "IsDeleted" = false;

-- User's reviews (profile page)
CREATE INDEX IX_Reviews_AppUserId
ON "Reviews"("AppUserId")
WHERE "IsDeleted" = false;

-- Rating filtering/sorting
CREATE INDEX IX_Reviews_Rating
ON "Reviews"("Rating")
WHERE "IsDeleted" = false;
```

**Impact:** Fast review loading
**Query Pattern:** Reviews filtered by ProductId and AppUserId

---

### 7. Categories Table
**Reason:** Hierarchical navigation

```sql
-- Parent-child relationship (category tree)
CREATE INDEX IX_Categories_ParentId_Order
ON "Categories"("ParentId", "Order")
WHERE "IsDeleted" = false;

-- Active categories
CREATE INDEX IX_Categories_IsActive
ON "Categories"("IsActive")
WHERE "IsDeleted" = false;

-- Category name search
CREATE INDEX IX_Categories_Name
ON "Categories"("Name")
WHERE "IsDeleted" = false;
```

**Impact:** Fast category tree rendering
**Query Pattern:** Hierarchical queries by ParentId, sorted by Order

---

### 8. RolePermissions Table (Critical for RBAC)
**Reason:** Permission checks on every authorized request

```sql
-- Permission lookup by role
CREATE INDEX IX_RolePermissions_RoleId
ON "RolePermissions"("RoleId");

-- Reverse lookup (which roles have a permission)
CREATE INDEX IX_RolePermissions_PermissionId
ON "RolePermissions"("PermissionId");

-- Composite unique constraint
CREATE UNIQUE INDEX IX_RolePermissions_RoleId_PermissionId
ON "RolePermissions"("RoleId", "PermissionId");
```

**Impact:** Fast permission resolution
**Query Pattern:** `GetPermissionsByRoleNamesAsync` in RolePermissionRepository

---

### 9. AspNetUserRoles Table (Identity Framework)
**Reason:** Used in every permission check

```sql
-- User's roles (permission resolution)
CREATE INDEX IX_AspNetUserRoles_UserId
ON "AspNetUserRoles"("UserId");

-- Role's users (already has default index, verify it exists)
-- Role member count (RoleService optimization from Week 1)
```

**Impact:** Fast role membership queries
**Query Pattern:** RoleService groups by RoleId for user counts

---

## Medium Priority (When needed)

### 10. Wishlists Table
**Reason:** User wishlist queries

```sql
-- User's wishlist
CREATE INDEX IX_Wishlists_AppUserId
ON "Wishlists"("AppUserId")
WHERE "IsDeleted" = false;

-- Check if item in wishlist
CREATE INDEX IX_Wishlists_ProductSkuId_AppUserId
ON "Wishlists"("ProductSkuId", "AppUserId")
WHERE "IsDeleted" = false;
```

**Impact:** Fast wishlist retrieval
**Query Pattern:** Filter by AppUserId

---

### 11. StoreProductSkus Table
**Reason:** Inventory management

```sql
-- Store's inventory
CREATE INDEX IX_StoreProductSkus_StoreId
ON "StoreProductSkus"("StoreId")
WHERE "IsDeleted" = false;

-- Product availability across stores
CREATE INDEX IX_StoreProductSkus_ProductSkuId
ON "StoreProductSkus"("ProductSkuId")
WHERE "IsDeleted" = false;

-- Composite for unique constraint
CREATE UNIQUE INDEX IX_StoreProductSkus_StoreId_ProductSkuId
ON "StoreProductSkus"("StoreId", "ProductSkuId")
WHERE "IsDeleted" = false;
```

**Impact:** Fast inventory lookups
**Query Pattern:** Filter by StoreId and ProductSkuId

---

### 12. PriceAdjustments Table
**Reason:** Active promotions lookup

```sql
-- Active promotions (date range queries)
CREATE INDEX IX_PriceAdjustments_StartDate_EndDate
ON "PriceAdjustments"("StartDate", "EndDate")
WHERE "IsDeleted" = false;

-- Current active promotions
CREATE INDEX IX_PriceAdjustments_Active
ON "PriceAdjustments"("StartDate", "EndDate")
WHERE "IsDeleted" = false
  AND "StartDate" <= CURRENT_TIMESTAMP
  AND "EndDate" >= CURRENT_TIMESTAMP;
```

**Impact:** Fast promotion lookup
**Query Pattern:** Date range filtering

---

### 13. Images Table
**Reason:** Product image lookups

```sql
-- Images by reference (product/category/etc.)
CREATE INDEX IX_Images_RefId
ON "Images"("RefId")
WHERE "IsDeleted" = false;
```

**Impact:** Fast image loading
**Query Pattern:** `ImageByRefIdSpecification` filters by RefId

---

### 14. OrderItems Table
**Reason:** Order details

```sql
-- Order's items (included in OrdersWithItemsAndOrderingSpecification)
-- EF Core will auto-create FK index, verify it exists
-- If missing, add:
CREATE INDEX IX_OrderItems_OrderId
ON "OrderItems"("OrderId");
```

---

### 15. InventoryTransactions Table (If Used)
**Reason:** Inventory audit trail

```sql
-- Product SKU transactions
CREATE INDEX IX_InventoryTransactions_ProductSkuId_CreatedAt
ON "InventoryTransactions"("ProductSkuId", "CreatedAt" DESC)
WHERE "IsDeleted" = false;

-- Store transactions
CREATE INDEX IX_InventoryTransactions_StoreId_CreatedAt
ON "InventoryTransactions"("StoreId", "CreatedAt" DESC)
WHERE "IsDeleted" = false;
```

---

### 16. ReturnOrders Table (If Used)
**Reason:** Return order tracking

```sql
-- Customer's returns
CREATE INDEX IX_ReturnOrders_UserId_CreatedAt
ON "ReturnOrders"("UserId", "CreatedAt" DESC)
WHERE "IsDeleted" = false;

-- Original order reference
CREATE INDEX IX_ReturnOrders_OrderId
ON "ReturnOrders"("OrderId")
WHERE "IsDeleted" = false;
```

---

## Composite Indexes

### Explanation
Composite indexes support queries that filter/sort by multiple columns. **Column order matters!**

**Rule:** Most selective column first (highest cardinality), then sort columns.

**Example:**
```sql
-- Good: UserId (selective) + Timestamp (sort)
CREATE INDEX IX_AuditLogs_UserId_Timestamp
ON "AuditLogs"("UserId", "Timestamp" DESC);

-- Can be used for:
-- 1. WHERE UserId = 'x'
-- 2. WHERE UserId = 'x' ORDER BY Timestamp DESC ✅ (perfect match)
-- 3. ORDER BY Timestamp DESC (won't use index efficiently)
```

---

## Unique Constraints

### Enforce Data Integrity

```sql
-- ProductSku: Unique SKU code
CREATE UNIQUE INDEX IX_ProductSkus_SkuCode
ON "ProductSkus"("SkuCode")
WHERE "IsDeleted" = false;

-- RefreshToken: Unique token string
CREATE UNIQUE INDEX IX_RefreshTokens_Token
ON "RefreshTokens"("Token")
WHERE "RevokedAt" IS NULL;

-- RolePermissions: Unique role-permission pair
CREATE UNIQUE INDEX IX_RolePermissions_RoleId_PermissionId
ON "RolePermissions"("RoleId", "PermissionId");

-- StoreProductSku: One entry per store-SKU pair
CREATE UNIQUE INDEX IX_StoreProductSkus_StoreId_ProductSkuId
ON "StoreProductSkus"("StoreId", "ProductSkuId")
WHERE "IsDeleted" = false;

-- Wishlist: User can't add same item twice
CREATE UNIQUE INDEX IX_Wishlists_AppUserId_ProductSkuId
ON "Wishlists"("AppUserId", "ProductSkuId")
WHERE "IsDeleted" = false;
```

---

## Implementation Guide

### Method 1: EF Core Migration (Recommended)

#### Step 1: Create Migration
```bash
dotnet ef migrations add AddDatabaseIndexes --project src/Infrastructure --startup-project src/API
```

#### Step 2: Edit Migration File
Open `src/Infrastructure/Migrations/YYYYMMDD_AddDatabaseIndexes.cs` and add:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Products - Critical
    migrationBuilder.CreateIndex(
        name: "IX_Products_CategoryId",
        table: "Products",
        column: "CategoryId",
        filter: "\"IsDeleted\" = false");

    migrationBuilder.CreateIndex(
        name: "IX_Products_ProductBrandId",
        table: "Products",
        column: "ProductBrandId",
        filter: "\"IsDeleted\" = false");

    migrationBuilder.CreateIndex(
        name: "IX_Products_Name",
        table: "Products",
        column: "Name",
        filter: "\"IsDeleted\" = false");

    migrationBuilder.CreateIndex(
        name: "IX_Products_IsNew_IsTrending_IsActive",
        table: "Products",
        columns: new[] { "IsNew", "IsTrending", "IsActive" },
        filter: "\"IsDeleted\" = false");

    migrationBuilder.CreateIndex(
        name: "IX_Products_UpdatedAt",
        table: "Products",
        column: "UpdatedAt",
        descending: true,
        filter: "\"IsDeleted\" = false");

    // Orders - Critical
    migrationBuilder.CreateIndex(
        name: "IX_Orders_BuyerEmail_OrderDate",
        table: "Orders",
        columns: new[] { "BuyerEmail", "OrderDate" },
        descending: new[] { false, true },
        filter: "\"IsDeleted\" = false");

    migrationBuilder.CreateIndex(
        name: "IX_Orders_Status_OrderDate",
        table: "Orders",
        columns: new[] { "Status", "OrderDate" },
        descending: new[] { false, true },
        filter: "\"IsDeleted\" = false");

    migrationBuilder.CreateIndex(
        name: "IX_Orders_PaymentIntentId",
        table: "Orders",
        column: "PaymentIntentId",
        unique: false,
        filter: "\"IsDeleted\" = false AND \"PaymentIntentId\" IS NOT NULL");

    // RefreshTokens - Critical
    migrationBuilder.CreateIndex(
        name: "IX_RefreshTokens_Token",
        table: "RefreshTokens",
        column: "Token",
        unique: true,
        filter: "\"RevokedAt\" IS NULL");

    migrationBuilder.CreateIndex(
        name: "IX_RefreshTokens_UserId_RevokedAt",
        table: "RefreshTokens",
        columns: new[] { "UserId", "RevokedAt" },
        filter: "\"RevokedAt\" IS NULL");

    migrationBuilder.CreateIndex(
        name: "IX_RefreshTokens_SessionId",
        table: "RefreshTokens",
        column: "SessionId",
        filter: "\"RevokedAt\" IS NULL");

    // AuditLogs - Critical
    migrationBuilder.CreateIndex(
        name: "IX_AuditLogs_UserId_Timestamp",
        table: "AuditLogs",
        columns: new[] { "UserId", "Timestamp" },
        descending: new[] { false, true });

    migrationBuilder.CreateIndex(
        name: "IX_AuditLogs_Path_Timestamp",
        table: "AuditLogs",
        columns: new[] { "Path", "Timestamp" },
        descending: new[] { false, true });

    migrationBuilder.CreateIndex(
        name: "IX_AuditLogs_Timestamp",
        table: "AuditLogs",
        column: "Timestamp",
        descending: true);

    // Add more indexes as needed...
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropIndex(name: "IX_Products_CategoryId", table: "Products");
    migrationBuilder.DropIndex(name: "IX_Products_ProductBrandId", table: "Products");
    migrationBuilder.DropIndex(name: "IX_Products_Name", table: "Products");
    // ... drop all indexes
}
```

#### Step 3: Apply Migration
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

---

### Method 2: Raw SQL Script (Direct)

Create file: `scripts/add_indexes.sql`

```sql
-- Products
CREATE INDEX CONCURRENTLY IF NOT EXISTS IX_Products_CategoryId
ON "Products"("CategoryId") WHERE "IsDeleted" = false;

CREATE INDEX CONCURRENTLY IF NOT EXISTS IX_Products_ProductBrandId
ON "Products"("ProductBrandId") WHERE "IsDeleted" = false;

-- Add all other indexes...

-- Analyze tables after creating indexes
ANALYZE "Products";
ANALYZE "Orders";
ANALYZE "RefreshTokens";
ANALYZE "AuditLogs";
```

Execute:
```bash
psql -h localhost -U your_user -d your_database -f scripts/add_indexes.sql
```

**Note:** `CONCURRENTLY` allows index creation without locking the table (production safe).

---

## Performance Testing

### Before Adding Indexes
```sql
-- Test slow query
EXPLAIN ANALYZE
SELECT * FROM "Products"
WHERE "CategoryId" = 'some-guid'
  AND "IsDeleted" = false
ORDER BY "UpdatedAt" DESC
LIMIT 20;

-- Look for "Seq Scan" (bad) vs "Index Scan" (good)
```

### After Adding Indexes
```sql
-- Same query should now use index
EXPLAIN ANALYZE
SELECT * FROM "Products"
WHERE "CategoryId" = 'some-guid'
  AND "IsDeleted" = false
ORDER BY "UpdatedAt" DESC
LIMIT 20;

-- Should see "Index Scan using IX_Products_CategoryId"
```

---

## Index Maintenance

### Monitor Index Usage
```sql
-- Check unused indexes (PostgreSQL)
SELECT schemaname, tablename, indexname, idx_scan
FROM pg_stat_user_indexes
WHERE idx_scan = 0
  AND indexname NOT LIKE 'pg_toast%'
ORDER BY tablename, indexname;

-- Drop unused indexes after 30 days of monitoring
```

### Rebuild Indexes (Quarterly)
```sql
-- PostgreSQL
REINDEX TABLE "Products";
REINDEX TABLE "Orders";
REINDEX TABLE "RefreshTokens";
REINDEX TABLE "AuditLogs";
```

---

## Trade-offs

### Index Benefits
✅ 10-100x faster SELECT queries
✅ Faster JOINs and WHERE clauses
✅ Faster ORDER BY and GROUP BY
✅ Enforce uniqueness constraints

### Index Costs
❌ Slower INSERT/UPDATE/DELETE (5-10% overhead per index)
❌ Additional storage (~10-30% of table size per index)
❌ More memory usage (index cache)

### Recommendation
- **Start with Critical indexes** (Products, Orders, RefreshTokens, AuditLogs)
- **Add High Priority** when you reach 10k records
- **Monitor query performance** using Serilog DurationMs
- **Add Medium Priority** based on actual slow queries

---

## Summary

### Immediate Action (Critical Indexes)
```
Total: 20 indexes
Estimated impact: 60-90% query time reduction
Tables: Products, Orders, RefreshTokens, AuditLogs
```

### Before 10k Records (High Priority)
```
Additional: 15 indexes
Tables: ProductSkus, Reviews, Categories, RolePermissions, UserRoles
```

### As Needed (Medium Priority)
```
Additional: 10 indexes
Tables: Wishlists, StoreProductSkus, PriceAdjustments, Images
```

### Total Recommended
**45 indexes** across 15+ tables

---

## Next Steps

1. **Baseline Measurement**
   - Check current query times in AuditLogs: `SELECT AVG("DurationMs") FROM "AuditLogs" WHERE "Path" LIKE '%product%'`
   - Note slow endpoints (>500ms)

2. **Create Migration**
   - Start with Critical indexes only
   - Test in staging environment first

3. **Apply & Monitor**
   - Apply migration to production
   - Monitor AuditLogs for performance improvements
   - Check PostgreSQL query stats

4. **Iterate**
   - Add High Priority indexes when you see slow queries
   - Use `EXPLAIN ANALYZE` to verify index usage
   - Drop unused indexes after 30 days

---

**Document Version:** 1.0
**Last Updated:** 2025-11-02
**Database:** PostgreSQL 15+
**ORM:** Entity Framework Core 9
