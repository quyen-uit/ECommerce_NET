# Postman Collections for ECommerce NET API

This folder contains Postman collections and environments for testing the ECommerce NET API.

## Files

### Environment
- `ECommerce.postman_environment.json` - Environment variables for development

### Collections
- `ECommerce-Account-API.postman_collection.json` - Account/User authentication and management
- `ECommerce-Products.postman_collection.json` - Products, Brands, and Product SKUs
- `ECommerce-Categories.postman_collection.json` - Category management (hierarchical)
- `ECommerce-Attributes.postman_collection.json` - Colors and Sizes
- `ECommerce-Orders-Payments.postman_collection.json` - Orders and Stripe payments
- `ECommerce-Basket.postman_collection.json` - Shopping basket (Redis-backed)
- `ECommerce-Permissions.postman_collection.json` - RBAC permission management
- `ECommerce-PriceAdjustments.postman_collection.json` - Price adjustments and discounts
- `ECommerce-Images.postman_collection.json` - Image processing
- `ECommerce-Admin.postman_collection.json` - Admin operations (soft-delete, audit logs)

## Quick Start

### 1. Import into Postman

1. Open Postman
2. Click **Import** button (top left)
3. Drag and drop both JSON files or click **Choose Files**
4. Click **Import**

### 2. Select Environment

- In the top-right corner of Postman, select **"ECommerce NET Development"** from the environment dropdown

### 3. Configure SSL (Development Only)

Since you're using localhost with HTTPS:
- Go to **Settings** (gear icon, top right)
- Turn **OFF** "SSL certificate verification"
- This is safe for local development only

### 4. Enable Cookies

For endpoints that use refresh tokens:
- Go to **Cookies** (below the Send button)
- Make sure cookies are enabled for `localhost:7229`
- The `rt` (refresh token) cookie will be set automatically after login

## Testing Flow

### First Time Setup

1. **Register New User** - Creates a test account
2. **Login (Admin)** - Gets admin token (auto-saved to environment)
3. **Get Current User** - Verify authentication works

### Session Management Testing

1. **Login (Admin)** - Create session 1
2. **Get All Sessions** - View all active sessions (auto-saves first sessionId)
3. **Revoke Specific Session** - Test revoking a specific session
4. **Revoke Other Sessions** - Keep current session, revoke all others
5. **Logout All Sessions** - Clear all sessions
6. **Logout** - Logout current session

### Email Validation

- **Check Email Exists** - Verify if email is already registered

### Token Refresh

1. **Login** - Get initial tokens
2. **Refresh Token** - Get new access token (requires cookies and Origin header)

## Environment Variables

The environment includes these pre-configured variables:

| Variable | Default Value | Description |
|----------|--------------|-------------|
| `baseUrl` | https://localhost:7229 | API base URL |
| `spaOrigin` | http://localhost:4200 | SPA origin for CORS |
| `accessToken` | (auto-saved) | JWT access token |
| `adminEmail` | quyen@mail.com | Default admin email |
| `adminPassword` | Admin@123 | Default admin password |
| `testEmail` | testuser@example.com | Test user email |
| `testPassword` | Test@123 | Test user password |
| `sessionId` | (auto-saved) | Session ID for revoke tests |
| `productId` | (auto-saved) | Product ID |
| `categoryId` | (auto-saved) | Category ID |
| `brandId` | (auto-saved) | Brand ID |
| `colorId` | (auto-saved) | Color ID |
| `sizeId` | (auto-saved) | Size ID |
| `productSkuId` | (auto-saved) | Product SKU ID |
| `basketId` | (auto-saved) | Basket ID |
| `orderId` | (auto-saved) | Order ID |
| `permissionId` | (auto-saved) | Permission ID |
| `priceAdjustmentId` | (auto-saved) | Price Adjustment ID |
| `userId` | (auto-saved) | User ID for filtering |

## Features

✅ **Auto Token Management** - JWT tokens automatically saved to environment variables
✅ **Pre-configured Tests** - Response validation included in each request
✅ **Environment Variables** - Easy switching between dev/staging/prod
✅ **Origin Headers** - Automatically included for sensitive endpoints
✅ **Session Tracking** - Auto-saves session IDs for testing
✅ **Cookie Support** - Refresh token cookies managed automatically

## Collections Overview

### 1. Account API (Authentication & Users)
**File:** `ECommerce-Account-API.postman_collection.json`

- **Authentication:** Register, Login (Admin/Test User), Refresh Token, Logout, Logout All
- **User Profile:** Get Current User, Check Email Exists
- **Session Management:** Get All Sessions, Revoke Specific/Other Sessions

### 2. Products
**File:** `ECommerce-Products.postman_collection.json`

- **Products:** Search, Get by ID, Create, Update, Delete
- **Brands:** Search, Get, Create, Create Many, Update, Delete, Delete Many
- **Product SKUs:** Search, Get by ID, Create, Update, Delete

### 3. Categories
**File:** `ECommerce-Categories.postman_collection.json`

- Get by ID, Search, Get Hierarchy (tree structure)
- Create, Create Many, Update, Delete

### 4. Attributes (Colors & Sizes)
**File:** `ECommerce-Attributes.postman_collection.json`

- **Colors:** Get, Search, Create, Create Many, Update, Delete
- **Sizes:** Get, Search, Create, Create Many, Update, Delete, Delete Many

### 5. Orders & Payments
**File:** `ECommerce-Orders-Payments.postman_collection.json`

- **Orders:** Create Order, Get All Orders, Get Order by ID, Get Delivery Methods
- **Payments:** Create/Update Payment Intent (Stripe), Stripe Webhook

### 6. Basket
**File:** `ECommerce-Basket.postman_collection.json`

- Get Basket by ID, Create/Update Basket, Delete Basket
- Note: All endpoints allow anonymous access

### 7. Permissions
**File:** `ECommerce-Permissions.postman_collection.json`

- Get by ID, Search, Create, Update, Delete
- Used for RBAC (Role-Based Access Control)

### 8. Price Adjustments
**File:** `ECommerce-PriceAdjustments.postman_collection.json`

- Get by ID, Search, Create, Update, Delete
- Manage product discounts and price changes

### 9. Images
**File:** `ECommerce-Images.postman_collection.json`

- Process Images (upload and associate with products/entities)

### 10. Admin Operations
**File:** `ECommerce-Admin.postman_collection.json`

- **Soft Deleted Items:** Get soft-deleted Products, Categories, Brands
- **Restore Items:** Restore soft-deleted Products, Categories, Brands
- **Permanent Delete:** Permanently delete soft-deleted items
- **Audit Logs:** Get audit logs with filtering by user/action

## Tips

- **Run in Order**: For first-time testing, run requests from top to bottom
- **Check Console**: View saved environment variables in Postman Console (bottom left)
- **View Tests**: Click **Tests** tab to see automatic validations
- **Auto-save Tokens**: Tokens are automatically saved after login
- **Cookie Management**: Make sure cookies are enabled for refresh/logout endpoints

## Default Credentials

**Admin Account:**
- Email: `quyen@mail.com`
- Password: `Admin@123`

⚠️ **Important**: Change these credentials immediately in production environments!

## Troubleshooting

### SSL Certificate Errors
- Disable SSL verification in Postman Settings (development only)

### Cookie Not Working
- Enable cookies in Postman for `localhost:7229`
- Check that Origin header is set correctly for refresh/logout endpoints

### 401 Unauthorized
- Run **Login (Admin)** to get a fresh token
- Token is automatically saved and used in subsequent requests

### 403 Forbidden
- User may not have required permissions
- Login with Admin account for full access

## Testing Workflow Examples

### Complete Product Setup
1. Login (Admin) - Get token
2. Create Brand - Auto-saves `brandId`
3. Create Category - Auto-saves `categoryId`
4. Create Color - Auto-saves `colorId`
5. Create Size - Auto-saves `sizeId`
6. Create Product - Uses saved `brandId` and `categoryId`
7. Create Product SKU - Uses saved `productId`, `colorId`, `sizeId`

### Shopping Flow
1. Create/Update Basket - Add items, auto-saves `basketId`
2. Create Order - Uses `basketId`
3. Create Payment Intent - Uses `basketId`
4. Get Order by ID - View order details

### Admin Management
1. Delete Product - Soft delete
2. Get Soft Deleted Products - View deleted items
3. Restore Product - Undo soft delete
4. Permanently Delete Product - Hard delete (cannot undo)
5. Get Audit Logs - View system activity

## Permission Requirements

Most endpoints require specific permissions. Login with the admin account (`quyen@mail.com` / `Admin@123`) for full access:

- **Product.*** - Product management
- **Category.*** - Category management
- **Brand.*** - Brand management
- **Color.*** - Color management
- **Size.*** - Size management
- **ProductSku.*** - Product SKU management
- **PriceAdjustment.*** - Price adjustment management
- **Image.*** - Image management
- **Permission.*** - Permission management
- **Module.Manage** - Soft-delete and restore operations