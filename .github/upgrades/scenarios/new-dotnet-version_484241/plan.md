# .NET 10.0 Upgrade Plan

**Solution:** OilChangeTracker  
**Current Framework:** .NET Framework 4.6.1  
**Target Framework:** .NET 10.0 (Long Term Support)  
**Status:** Planning Phase ✅  
**Estimated Duration:** 40-60 development hours

---

## Executive Summary

This document outlines the detailed upgrade plan for modernizing OilChangeTracker from .NET Framework 4.6.1 to .NET 10.0. This is a **major modernization** involving architectural changes from classic ASP.NET to ASP.NET Core.

### Key Milestones
1. **Phase 1:** Foundation (Project files, NuGet, startup)
2. **Phase 2:** Core Infrastructure (Routing, DI, Middleware)
3. **Phase 3:** Feature Migration (Auth, Bundling, Views)
4. **Phase 4:** Testing & Validation

### Risk: **MEDIUM-HIGH** 🟠
Multiple incompatible packages (11) and major architectural changes, but manageable scope.

---

## Phase 1: Foundation & Setup (Blocking)

### 1.1 Project File Conversion

**Goal:** Convert both projects from legacy .NET Framework format to SDK-style format

**Projects to Convert:**
1. `OilChangeTracker\OilChangeTracker.Web.csproj`
2. `OilChangeTracker.Tests\OilChangeTracker.Tests.csproj`

**Expected Changes:**
- Remove legacy `<ProjectTypeGuids>` elements
- Remove `<PropertyGroup>` web-specific elements (RootNamespace, etc. → implicit)
- Add `<Sdk>Microsoft.NET.Sdk.Web</Sdk>` for web project
- Add `<TargetFramework>net10.0</TargetFramework>`
- Convert `<ItemGroup>` references to implicit includes
- Remove AssemblyInfo.cs → move to project file properties
- Remove redundant .NET Framework references

**Outcome:**
- Both .csproj files are in modern SDK format
- Target framework set to net10.0
- Projects load successfully in Visual Studio

---

### 1.2 NuGet Package Management

**Goal:** Remove incompatible packages and update/add required ones

#### 1.2.1 Packages to REMOVE

These packages are either incompatible or included in .NET 10 framework:

**Incompatible Packages (must remove):**
- `Microsoft.AspNet.Identity.Core` (2.2.4)
- `Microsoft.AspNet.Identity.EntityFramework` (2.2.4)
- `Microsoft.AspNet.Identity.Owin` (2.2.4)
- `Microsoft.AspNet.Mvc` (5.3.0) - included in ASP.NET Core
- `Microsoft.AspNet.Razor` (3.3.0) - included in ASP.NET Core
- `Microsoft.AspNet.WebPages` (3.3.0) - included in ASP.NET Core
- `Microsoft.AspNet.Web.Optimization` (1.1.3) - not supported in Core
- `Microsoft.Owin` (4.2.3)
- `Microsoft.Owin.Host.SystemWeb` (4.2.3)
- `Microsoft.Owin.Security` (4.2.3)
- `Microsoft.Owin.Security.Cookies` (4.2.3)
- `Microsoft.Owin.Security.Facebook` (4.2.3)
- `Microsoft.Owin.Security.Google` (4.2.3)
- `Microsoft.Owin.Security.MicrosoftAccount` (4.2.3)
- `Microsoft.Owin.Security.OAuth` (4.2.3)
- `Microsoft.Owin.Security.Twitter` (4.2.3)
- `Owin` (1.0)
- `Antlr` (3.5.0.2) - replace with Antlr4
- `Microsoft.Web.Infrastructure` (2.0.0) - included in framework
- `System.Buffers` (4.6.1) - included in framework
- `System.ComponentModel.Annotations` (5.0.0) - included in framework
- `System.Memory` (4.6.3) - included in framework
- `System.Numerics.Vectors` (4.6.1) - included in framework
- `System.Reflection.Emit` (4.7.0) - included in framework
- `System.Security.Cryptography.Cng` (4.5.0) - included in framework
- `System.Threading.Tasks.Extensions` (4.6.3) - included in framework
- `System.ValueTuple` (4.6.2) - included in framework

#### 1.2.2 Packages to ADD

**ASP.NET Core Essentials:**
- `Microsoft.AspNetCore.Identity` (latest)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (latest)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (10.0.x)
- `Microsoft.AspNetCore.Authentication.Cookies` (10.0.x)

**Bundling & Minification (optional, but recommended):**
- `WebOptimizer.Core` (or alternative) - replace System.Web.Optimization

#### 1.2.3 Packages to UPGRADE

From assessment findings:
- `Microsoft.Bcl.AsyncInterfaces`: 10.0.0 → 10.0.5
- `Microsoft.Bcl.TimeProvider`: 8.0.1 → 10.0.5
- `Microsoft.CSharp`: 4.5.0 → 4.7.0
- `Microsoft.Extensions.DependencyInjection.Abstractions`: 10.0.0 → 10.0.5
- `Microsoft.Extensions.Logging.Abstractions`: 10.0.0 → 10.0.5
- `Microsoft.Extensions.Options`: 10.0.0 → 10.0.5
- `Microsoft.Extensions.Primitives`: 10.0.0 → 10.0.5
- `System.Text.Encodings.Web`: 8.0.0 → 10.0.5
- `System.Text.Json`: 8.0.5 → 10.0.5

#### 1.2.4 Packages that STAY

These are compatible with .NET 10.0:
- `EntityFramework` (6.5.1) ✅
- `AutoMapper` (16.1.1) ✅
- `bootstrap` (5.3.8) ✅
- `jQuery` (3.7.1) ✅
- `jQuery.Validation` (1.21.0) ✅
- `Newtonsoft.Json` (13.0.4) ✅
- `WebGrease` (1.6.0) ✅
- `Microsoft.IdentityModel.*` (8.14.0) ✅
- `System.Diagnostics.DiagnosticSource` (10.0.5) ✅
- `System.Runtime.CompilerServices.Unsafe` (6.1.2) ✅

**Outcome:**
- All incompatible packages removed
- .NET 10.0 framework references added
- Required ASP.NET Core packages added
- Package.config removed (SDK-style uses .csproj only)

---

### 1.3 Application Startup (Program.cs)

**Goal:** Create new ASP.NET Core application entry point

**Current State:**
- `Global.asax.cs` handles application startup
- `RouteConfig.cs` defines routes
- `FilterConfig.cs` defines global filters
- `BundleConfig.cs` defines bundling

**New State:**
- Create `Program.cs` with WebApplication builder pattern
- Create `appsettings.json` for configuration
- Create `appsettings.Development.json` for dev settings
- Migrate all startup logic to Program.cs

**Program.cs Structure:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

**Outcome:**
- Program.cs created and configured
- appsettings.json created with connection strings
- Global.asax.cs can be safely deleted
- Application starts with ASP.NET Core host

---

### 1.4 Web Configuration (appsettings.json)

**Goal:** Replace web.config with appsettings.json

**Current Configuration in web.config:**
- Connection strings
- AppSettings
- Authentication settings
- Assembly bindings

**New Configuration:**
Create `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OilChangeTracker;Trusted_Connection=true;"
  },
  "AllowedHosts": "*"
}
```

Create `appsettings.Development.json` for development overrides.

**Outcome:**
- appsettings.json created with all connection strings and settings
- web.config can be removed after startup verification
- Environment-specific configurations ready

---

## Phase 2: Core Infrastructure

### 2.1 Routing System

**Goal:** Convert from ASP.NET MVC RouteTable to ASP.NET Core endpoint routing

**Current Implementation (RouteConfig.cs):**
```csharp
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
    }
}
```

**New Implementation (Program.cs):**
```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

**Also needed:**
- Update `[Route]` attributes on controllers if using custom routes
- Remove `RouteConfig.cs`
- Remove route registration from `Global.asax.cs`

**Controllers Affected:**
- HomeController
- AccountController
- [Other controllers - TBD]

**Outcome:**
- Routing converted to ASP.NET Core endpoint routing
- All controllers map correctly
- RouteConfig.cs deleted

---

### 2.2 Dependency Injection Container

**Goal:** Set up built-in ASP.NET Core DI container

**Current State:**
- May or may not have DI configured
- Likely manual instantiation in controllers

**New State:**
- All services registered in `Program.cs` using `builder.Services`
- Controllers use constructor injection
- DbContext registered with AddDbContext()
- Identity services registered with AddIdentity()

**Services to Register:**
```csharp
// Controllers and Views
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Custom services (if any)
// builder.Services.AddScoped<IMyService, MyService>();
```

**Outcome:**
- DI container fully configured
- No manual service instantiation
- Constructor injection works in all controllers

---

### 2.3 Middleware Pipeline

**Goal:** Replace OWIN middleware with ASP.NET Core native middleware

**Current OWIN Middleware (from Startup.cs/Global.asax):**
- OWIN app configuration
- Potential external authentication (Facebook, Google, Twitter)
- Cookie authentication
- CORS configuration

**New ASP.NET Core Middleware:**
```csharp
// Exception handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS and static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing
app.UseRouting();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Custom middleware (if any)
app.Use(async (context, next) => { /* middleware code */ });

// Endpoints
app.MapControllerRoute(...);
```

**Outcome:**
- OWIN references removed
- Middleware pipeline configured
- External authentication (if used) migrated to Core providers
- Request processing pipeline ready

---

## Phase 3: Feature Migration

### 3.1 Authentication & ASP.NET Identity

**Goal:** Migrate from ASP.NET Identity 2.x to ASP.NET Core Identity

**Current Implementation:**
- Microsoft.AspNet.Identity.Core
- Microsoft.AspNet.Identity.EntityFramework
- Microsoft.AspNet.Identity.Owin
- Custom User model (likely inheriting from IdentityUser)
- ApplicationDbContext with DbSet<IdentityUser>

**Migration Steps:**
1. Install `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
2. Update User model to inherit from `IdentityUser`
3. Update ApplicationDbContext to inherit from `IdentityDbContext<IdentityUser>`
4. Update account controller actions (Login, Register, Logout)
5. Update login views
6. Run database migration (separate step)

**Key Changes:**
- `SignInManager<TUser>` (same interface, different namespace)
- `UserManager<TUser>` (same interface, different namespace)
- Password reset flow simplified
- Email confirmation flow simplified

**Files to Update:**
- `Models/ApplicationDbContext.cs`
- `Models/ApplicationUser.cs` (if custom)
- `Controllers/AccountController.cs`
- `Views/Account/*.cshtml` (login, register, manage)

**Outcome:**
- Identity fully migrated to Core version
- User authentication works with new system
- Database schema updated (may require data migration)

---

### 3.2 Bundling & Minification

**Goal:** Replace System.Web.Optimization with alternative solution

**Current Implementation:**
- Uses `System.Web.Optimization` NuGet package
- `BundleConfig.cs` defines script/style bundles
- Views use `@Scripts.Render()` and `@Styles.Render()`

**Two Approaches:**

**Option A: Direct HTML References (Simplest)**
- Remove bundling entirely
- Use direct `<script>` and `<link>` tags
- CSS/JS files served as-is
- Good for development, requires minification tool for production

**Option B: WebOptimizer.Core**
- Install `WebOptimizer.Core` NuGet package
- Configure in Program.cs
- Update views to use `href="~/bundle/..."` or similar
- Handles bundling and minification at runtime

**Recommended:** Option B (WebOptimizer) for production-ready setup

**Views to Update:**
- `Views/Shared/_Layout.cshtml`
- Other layout files with bundle references

**Files to Delete/Update:**
- `App_Start/BundleConfig.cs` → Delete
- Update `_Layout.cshtml` to remove `@Scripts.Render()`, `@Styles.Render()`

**Outcome:**
- CSS and JavaScript properly loaded
- Bundling configured (if using WebOptimizer)
- No System.Web.Optimization references

---

### 3.3 Filters & Action Filters

**Goal:** Migrate from GlobalFilterCollection to ASP.NET Core filters

**Current Implementation:**
- `FilterConfig.cs` registers global filters
- Custom action filters (if any)
- Likely filters for authorization, exception handling

**ASP.NET Core Approach:**
- Register filters in `Program.cs`: `services.AddControllersWithViews(options => options.Filters.Add(...))`
- Or use attributes: `[ValidateAntiForgeryToken]`, `[Authorize]`, etc.
- Create custom filter classes if needed

**Migration Example:**
```csharp
// Old code (Global.asax / FilterConfig.cs)
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
    }
}

// New code (Program.cs)
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(ExceptionHandlingFilter));
});
```

**Files to Update/Delete:**
- `App_Start/FilterConfig.cs` → Delete (migrate content to Program.cs)
- Global filter registration in `Global.asax` → Delete
- Custom filter classes → Update namespace imports

**Outcome:**
- All filters functional in ASP.NET Core
- Exception handling works
- Authorization filters operational

---

### 3.4 Views & Razor Pages

**Goal:** Update views to work with ASP.NET Core Razor engine

**Current Implementation:**
- Views in `Views/` folder
- Layout in `Views/Shared/_Layout.cshtml`
- Uses `@{...}` syntax (compatible)
- Uses `Html.BeginForm()`, `Html.ActionLink()` helpers

**Required Changes:**
1. Create `Views/_ViewImports.cshtml` with standard imports
2. Create `Views/_ViewStart.cshtml` if needed
3. Update view imports to include Core namespaces
4. Update HTML helpers (mostly compatible)
5. Update bundling references (as per 3.2)

**_ViewImports.cshtml (Required):**
```csharp
@using OilChangeTracker
@using OilChangeTracker.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

**_ViewStart.cshtml (Optional but recommended):**
```csharp
@{
    Layout = "_Layout";
}
```

**Files to Create/Update:**
- `Views/_ViewImports.cshtml` → Create
- `Views/_ViewStart.cshtml` → Create (optional)
- `Views/Shared/_Layout.cshtml` → Update bundle references
- All other views → Mostly compatible, minor updates

**Outcome:**
- All views render correctly
- Tag helpers work
- Layout system functional

---

## Phase 4: Testing & Validation

### 4.1 Unit Test Updates

**Goal:** Ensure test project works with .NET 10.0

**Current Implementation:**
- MSTest framework (compatible!)
- Tests likely reference controllers, services
- May reference System.Web (needs cleanup)

**Required Changes:**
1. Convert test project to SDK-style (Phase 1.1)
2. Remove System.Web references from tests
3. Update controller instantiation to use DI if needed
4. Update mock setup for DI container

**Test Structure Example:**
```csharp
// Old approach - manual instantiation
var controller = new HomeController();

// New approach - inject dependencies
var mockService = new Mock<IMyService>();
var controller = new HomeController(mockService.Object);
```

**Files to Update:**
- `OilChangeTracker.Tests/OilChangeTracker.Tests.csproj` → Converted to SDK
- All test files → Remove System.Web references, update controller setup

**Outcome:**
- All unit tests compile
- Tests run successfully against .NET 10.0
- Test coverage maintained

---

### 4.2 Build & Compilation

**Goal:** Ensure solution builds cleanly without errors

**Verification Steps:**
1. Solution builds successfully (0 errors, warnings acceptable)
2. No NuGet restore errors
3. No missing assembly references
4. No unresolved namespace imports
5. IntelliSense works in Visual Studio

**Key Checks:**
- All projects target net10.0
- No duplicate NuGet package versions
- No assembly binding redirects needed
- All using statements valid

**Outcome:**
- Clean build completed
- No compilation errors
- All projects load in IDE

---

### 4.3 Integration Testing

**Goal:** Verify application runs and key features work

**Pre-requisites:**
- Database created/migrated
- .NET 10 runtime installed
- IIS Express configured (if using)

**Test Scenarios:**
1. Application starts without errors
2. Home page loads and renders
3. Navigation works (if implemented)
4. Authentication workflow functional (if implemented)
5. Database queries work (EF Core)
6. Static files served correctly (CSS, JS, images)

**Outcome:**
- Application runs without errors
- Core features functional
- Ready for deployment

---

## Execution Order & Dependencies

### Dependency Graph

```
Phase 1.1: Convert Projects
    ↓
Phase 1.2: Update NuGet Packages
    ↓
Phase 1.3: Create Program.cs & appsettings.json
    ↓
[Phase 1.4: Web Config] (parallel)
    ↓
Phase 2.1: Routing System
    ↓
Phase 2.2: Dependency Injection
    ↓
Phase 2.3: Middleware Pipeline
    ↓
[Phase 3.1: Auth & Identity] (starts here, parallel to 3.2-3.4)
[Phase 3.2: Bundling] (parallel)
[Phase 3.3: Filters] (parallel)
[Phase 3.4: Views] (parallel)
    ↓
Phase 4.1: Unit Tests
Phase 4.2: Build & Compilation
Phase 4.3: Integration Testing
```

### Critical Path
1. Projects must be converted first (blocks everything)
2. NuGet packages must be resolved (blocks startup)
3. Program.cs must exist (blocks running)
4. Routing must work (required for app to function)
5. Views must be updated (required for UI)

### Optional/Parallel Tasks
- Authentication migration (can start early)
- Bundling replacement (can do anytime)
- Filter migration (after program.cs)

---

## Rollback Strategy

### Save Points
1. After Phase 1 (project files updated) - **checkpoint-phase1**
2. After Phase 2 (infrastructure ready) - **checkpoint-phase2**
3. After Phase 3 (features migrated) - **checkpoint-phase3**

### If Major Issues
- Revert to previous branch: `git checkout master`
- Or reset to checkpoint: `git reset --hard checkpoint-phase1`
- Update tasks.md with failures and assessment findings

---

## Success Criteria

### Phase 1 Complete ✅
- [ ] Both projects in SDK-style format
- [ ] Target framework set to net10.0
- [ ] NuGet packages resolved (no errors)
- [ ] Program.cs created
- [ ] appsettings.json configured
- [ ] Solution builds (0 errors)

### Phase 2 Complete ✅
- [ ] Routing configured and working
- [ ] DI container populated
- [ ] Middleware pipeline configured
- [ ] Application starts (F5 or `dotnet run`)

### Phase 3 Complete ✅
- [ ] Authentication system working
- [ ] Views render without errors
- [ ] CSS/JS loading correctly
- [ ] Filters operational

### Phase 4 Complete ✅
- [ ] All unit tests pass
- [ ] Clean build (0 errors)
- [ ] Integration tests pass
- [ ] Application fully functional

---

## Resource Links

### Official Documentation
- [.NET 10.0 Release Notes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [Migrate from ASP.NET Framework to ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/migration/proper-to-core)
- [Breaking Changes in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0)
- [ASP.NET Core Fundamentals](https://learn.microsoft.com/en-us/aspnet/core/fundamentals)

### Migration Guides
- [System.Web to ASP.NET Core Migration](https://learn.microsoft.com/en-us/aspnet/core/migration/framework)
- [Identity V1 to AspNetCore.Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [OWIN to ASP.NET Core Middleware](https://learn.microsoft.com/en-us/aspnet/core/migration/owin)

### Tools & Utilities
- [ASP.NET Core Upgrade Assistant](https://github.com/dotnet/upgrade-assistant)
- [WebOptimizer](https://github.com/madskristensen/WebOptimizer)
- [.NET Portability Analyzer](https://github.com/Microsoft/dotnet-apiport)

---

**Plan Complete** ✅

Ready to generate execution tasks and begin Phase 1?
