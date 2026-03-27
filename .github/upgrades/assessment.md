# .NET 10.0 Upgrade Assessment Report

**Solution:** OilChangeTracker  
**Current Framework:** .NET Framework 4.6.1  
**Target Framework:** .NET 10.0 (Long Term Support)  
**Date:** 2024  
**Status:** Assessment Complete ✅

---

## Executive Summary

This is a **major modernization effort** from .NET Framework 4.6.1 to .NET 10.0. The codebase uses classic ASP.NET MVC patterns that require substantial refactoring to work with ASP.NET Core.

### Key Metrics
- **Projects:** 2 (1 Web App, 1 Test Library)
- **Total Issues:** 856
  - **Mandatory (blocking):** 819
  - **Potential (warnings):** 36
  - **Optional:** 1
- **Affected Files:** 30
- **Primary Challenge:** ASP.NET Framework (System.Web) dependencies - 592 instances

### Complexity Level: **HIGH** 🔴

This is not a simple framework version bump. ASP.NET Framework and ASP.NET Core are fundamentally different architectures. You'll need to:
1. Convert project files from legacy XML format to SDK-style
2. Replace System.Web with ASP.NET Core equivalents
3. Migrate from OWIN to ASP.NET Core middleware
4. Rewrite routing, filters, and authentication
5. Update NuGet dependencies

---

## Project-by-Project Breakdown

### 1. OilChangeTracker.Web.csproj (Web Application)

**Status:** ⚠️ Major Refactoring Required

#### Issues by Category

| Category | Count | Severity |
|----------|-------|----------|
| API Binary Incompatibilities | 761 | 🔴 Mandatory |
| API Source Incompatibilities | 23 | 🟡 Potential |
| NuGet Incompatibilities | 15 | 🔴 Mandatory |
| NuGet Removals (included in framework) | 12 | 🔴 Mandatory |
| Feature Migrations | 6 | 🔴 Mandatory |
| NuGet Upgrades Needed | 10 | 🟡 Potential |

#### Critical Issues

**ASP.NET Framework Dependencies (761 instances)**
- **Impact:** Every reference to `System.Web.*` namespace is incompatible
- **Examples:** `System.Web.Mvc`, `System.Web.Routing`, `System.Web.Http`
- **Action:** Replace with ASP.NET Core equivalents

**Incompatible NuGet Packages**
```
Microsoft.AspNet.Identity.Core          2.2.4    ❌ Must remove/replace
Microsoft.AspNet.Identity.EntityFramework 2.2.4   ❌ Must remove/replace
Microsoft.AspNet.Identity.Owin          2.2.4    ❌ Must remove/replace
Microsoft.AspNet.Mvc                    5.3.0    ❌ Included in framework
Microsoft.AspNet.Razor                  3.3.0    ❌ Included in framework
Microsoft.AspNet.WebPages               3.3.0    ❌ Included in framework
Microsoft.AspNet.Web.Optimization       1.1.3    ❌ Not supported in Core
Microsoft.Owin                           4.2.3    ❌ Replace with native middleware
Microsoft.Owin.Host.SystemWeb           4.2.3    ❌ Not supported in Core
Microsoft.Owin.Security.*               4.2.3    ❌ Replace with Core auth
Owin                                    1.0      ❌ Not supported in Core
Antlr                                   3.5.0.2  ⚠️ Consider Antlr4 4.6.6
```

**Packages Needing Upgrades**
```
Microsoft.Bcl.AsyncInterfaces           10.0.0 → 10.0.5
Microsoft.Bcl.TimeProvider              8.0.1  → 10.0.5
Microsoft.CSharp                        4.5.0  → 4.7.0
Microsoft.Extensions.DependencyInjection.Abstractions  10.0.0 → 10.0.5
Microsoft.Extensions.Logging.Abstractions       10.0.0 → 10.0.5
Microsoft.Extensions.Options                    10.0.0 → 10.0.5
Microsoft.Extensions.Primitives                 10.0.0 → 10.0.5
System.Text.Encodings.Web                       8.0.0  → 10.0.5
System.Text.Json                                8.0.5  → 10.0.5
```

**Compatible Packages** ✅
- AutoMapper 16.1.1
- bootstrap 5.3.8
- EntityFramework 6.5.1
- jQuery 3.7.1
- jQuery.Validation 1.21.0
- Newtonsoft.Json 13.0.4
- WebGrease 1.6.0

#### Feature Migration Required

1. **System.Web.Optimization → Direct HTML References**
   - Current: Using bundling/minification (14 instances)
   - Action: Replace `@Scripts.Render()` with direct `<script>` tags
   - Impact: Manual updates to layout files

2. **RouteCollection → Route Mapping (2 instances)**
   - Current: `RouteTable.Routes.MapRoute()`
   - Action: Use `app.MapControllerRoute()` in Program.cs or route attributes
   - Impact: Routing configuration rewrite

3. **GlobalFilterCollection → Middleware (2 instances)**
   - Current: `GlobalFilters.Filters.Add()`
   - Action: Register in `services.AddControllers()` or middleware pipeline
   - Impact: Filter registration rewrite

4. **OWIN → ASP.NET Core Middleware (2 instances)**
   - Current: Using OWIN app builder pattern
   - Action: Use ASP.NET Core's native middleware system
   - Impact: Complete middleware rewrite

5. **Global.asax.cs → Program.cs (1 instance)**
   - Current: Application_Start, Application_End in Global.asax
   - Action: Move to Program.cs startup logic
   - Impact: Complete application startup rewrite

6. **ASP.NET Identity → ASP.NET Core Identity (1 instance)**
   - Current: Microsoft.AspNet.Identity 2.x
   - Action: Migrate to Microsoft.AspNetCore.Identity
   - Impact: User management rewrite, database schema changes

#### Behavioral Changes (2 instances)

These may work but behave differently in .NET 10:
- Review default behaviors in ASP.NET Core
- Test thoroughly for runtime differences

---

### 2. OilChangeTracker.Tests.csproj (Test Library)

**Status:** ⚠️ Moderate Updates Required

#### Issues by Category

| Category | Count | Severity |
|----------|-------|----------|
| API Binary Incompatibilities | 1 | 🔴 Mandatory |
| NuGet Incompatibilities | 3 | 🔴 Mandatory |
| NuGet Removals | 4 | 🔴 Mandatory |
| Project Format | 1 | 🔴 Mandatory |
| Framework Target | 1 | 🔴 Mandatory |

#### Specific Issues

**Test Framework Compatibility**
- Current: MSTest (Microsoft.VisualStudio.TestTools.UnitTesting)
- Status: ✅ Fully compatible with .NET 10
- Action: No changes needed

**Incompatible Dependencies**
- System.Web references will need removal
- Test project won't actually use System.Web, but references may exist

---

## Detailed Upgrade Tasks

### Phase 1: Foundation (Blocking)
1. **Project File Conversion**
   - Convert `OilChangeTracker.Web.csproj` from XML to SDK-style format
   - Convert `OilChangeTracker.Tests.csproj` from XML to SDK-style format
   - Update to `<TargetFramework>net10.0</TargetFramework>`

2. **NuGet Package Cleanup**
   - Remove: Antlr 3.5.0.2 (replace with Antlr4)
   - Remove: Microsoft.AspNet.Identity.* (migrate to Core Identity)
   - Remove: Microsoft.AspNet.Mvc, Razor, WebPages, Web.Optimization
   - Remove: Microsoft.Owin.* and Owin packages
   - Upgrade: System.Text.Json, Extensions packages

3. **Application Startup**
   - Create Program.cs with host configuration
   - Migrate Global.asax.cs logic to Program.cs
   - Delete Global.asax.cs

### Phase 2: Core Infrastructure (High Priority)
1. **Routing System**
   - Convert RouteConfig.cs to endpoint routing
   - Implement in Program.cs or MapControllers()

2. **Dependency Injection**
   - Set up IServiceCollection in Program.cs
   - Register services (DbContext, Identity, etc.)

3. **Middleware Pipeline**
   - Replace OWIN middleware with ASP.NET Core equivalents
   - Implement exception handling middleware
   - Set up authentication/authorization middleware

### Phase 3: Feature Migration (Medium Priority)
1. **Authentication & Identity**
   - Install Microsoft.AspNetCore.Identity
   - Create identity models and DbContext
   - Migrate user data schema
   - Update login/register pages

2. **Bundling & Minification**
   - Replace @Scripts.Render() and @Styles.Render() with direct tags
   - Optionally use WebOptimizer or other bundling solutions
   - Update _Layout.cshtml and other views

3. **Filters & Attributes**
   - Convert [Authorize] attributes (mostly compatible)
   - Migrate custom filters to Core format
   - Update filter registration in ConfigureServices

4. **Views & Razor**
   - Update view syntax (mostly compatible)
   - Migrate view imports
   - Update _ViewImports.cshtml

### Phase 4: Testing & Verification (Ongoing)
1. Run all unit tests after each phase
2. Compile and fix remaining errors
3. Integration testing
4. Performance testing

---

## Dependency Analysis

### Direct Dependencies by Type

**ASP.NET Framework (incompatible)**
- System.Web*
- System.Web.Mvc
- System.Web.Routing
- System.Web.Http
- System.Web.Optimization

**Authentication (requires replacement)**
- Microsoft.AspNet.Identity.*
- Microsoft.Owin.*
- Owin

**Data Access (compatible/upgradable)**
- EntityFramework 6.5.1 ✅
- System.Data.* ✅

**Serialization (compatible with upgrades)**
- Newtonsoft.Json 13.0.4 ✅
- System.Text.Json 8.0.5 → 10.0.5

**UI Framework (compatible)**
- bootstrap 5.3.8 ✅
- jQuery 3.7.1 ✅
- jQuery.Validation 1.21.0 ✅

---

## Breaking Changes Summary

### High Impact
1. **Complete HTTP request pipeline redesign** - OWIN → ASP.NET Core middleware
2. **Routing system changed** - AttributeRouting becomes primary pattern
3. **Authentication system replaced** - Entire identity subsystem different
4. **Configuration model changed** - appsettings.json instead of web.config
5. **View engine improvements** - Some Razor syntax changes

### Medium Impact
1. **NuGet package replacements** - 11 packages need updating
2. **Bundling/minification removed** - Manual or alternative solutions needed
3. **Dependency injection mandatory** - No more static helpers (like WebApi.Dispatcher)
4. **Web.config obsolete** - Use appsettings.json and environment variables

### Low Impact
1. **C# language features** - C# 7.3 → C# 12.0 available (optional improvements)
2. **Async patterns** - Generally already in use
3. **Nullable reference types** - Can be enabled gradually

---

## Estimated Effort

### Development Time: **40-60 hours**

Breaking down by task:
- **Project setup & migration:** 4 hours
- **NuGet updates & cleanup:** 2 hours
- **Program.cs & startup:** 4 hours
- **Routing conversion:** 3 hours
- **Authentication/Identity:** 8 hours
- **OWIN → Middleware:** 6 hours
- **Bundling/Views:** 4 hours
- **Error fixes & compilation:** 8 hours
- **Testing & validation:** 8 hours
- **Documentation & cleanup:** 3 hours

### Risk Level: **MEDIUM-HIGH** 🟠

**Mitigating Factors:**
- Project size is moderate (130 files)
- No exotic dependencies
- Entity Framework is compatible
- Tests use standard MSTest framework

**Risk Factors:**
- Many incompatible packages to replace
- Significant architectural changes (OWIN → Middleware)
- Authentication system complete rewrite
- Learning curve for ASP.NET Core patterns

---

## Recommendations

### ✅ RECOMMENDED APPROACH

**Incremental Conversion Strategy:**
1. Start with test project (simpler, fewer dependencies)
2. Create new SDK-style projects alongside old ones
3. Gradually migrate code from old projects to new projects
4. Use project references to maintain interdependencies
5. Test thoroughly at each stage

### Alternative: Clean Rewrite
If the codebase is relatively small and you want modern patterns, consider:
- Starting fresh ASP.NET Core project
- Copying business logic incrementally
- Rewriting UI/controllers with new patterns
- Can be faster for small to medium projects

### NOT RECOMMENDED: Minimal Changes
- Simply updating target framework won't work
- System.Web is fundamentally incompatible
- Would result in 1000+ compilation errors
- Not a viable approach for this project

---

## Next Steps

1. **Review this assessment** - Understand the scope
2. **Choose upgrade strategy** - Incremental vs. Rewrite
3. **Set up .NET 10 SDK** - Ensure SDK is installed locally
4. **Create detailed plan** - Phase-by-phase breakdown
5. **Begin Phase 1** - Project file conversion

---

## Appendix: Detailed Issue List

### Binary Incompatible APIs (Top 10)
1. `System.Web.Mvc.Controller` - Rewrite using ASP.NET Core `ControllerBase`
2. `System.Web.Mvc.ViewResult` - Use Core `ViewResult` (compatible interface)
3. `System.Web.HttpContext` - Use ASP.NET Core `HttpContext`
4. `System.Web.Routing.RouteTable` - Use endpoint routing in Core
5. `System.Web.HttpRequest` - Use ASP.NET Core `HttpRequest`
6. `System.Web.HttpResponse` - Use ASP.NET Core `HttpResponse`
7. `System.Web.Mvc.ViewEngineResult` - Rewrite view references
8. `System.Web.Mvc.FilterInfo` - Use Core filter attributes
9. `System.Web.Security.FormsAuthentication` - Use Core authentication
10. `System.Web.HttpApplication` - Move logic to Program.cs/Middleware

---

**Assessment Complete** ✅  
**Ready to proceed to Planning Phase**

Shall we continue to create the detailed upgrade plan?
