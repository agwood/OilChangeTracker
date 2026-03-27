# OilChangeTracker .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the OilChangeTracker upgrade from .NET Framework 4.6.1 to .NET 10.0. Work proceeds from prerequisites → foundation conversion (projects/packages/startup) → infrastructure → feature migration → testing and verification.

**Progress**: 1/6 tasks complete (17%) ![0%](https://progress-bar.xyz/17)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-26 23:27)*
**References**: Plan §Phase 4.3 Pre-requisites, Plan §Phase 1

- [✓] (1) Verify .NET 10 SDK/runtime is installed and `dotnet` CLI available per Plan §Phase 4.3
- [✓] (2) Runtime/SDK meets minimum requirements (**Verify**)
- [✓] (3) Verify required tooling available: `dotnet-ef`, optional `upgrade-assistant`, and any other tools referenced in Plan §Resource Links
- [✓] (4) Required tools installed/available (**Verify**)
- [✓] (5) Check configuration file compatibility (presence of `web.config`, `packages.config`, global.json if present) per Plan §Phase 1.4 and Plan §Phase 1.2
- [✓] (6) Configuration files identified and noted for migration (compatible or flagged) (**Verify**)

### [▶] TASK-002: Convert projects, update packages, and create startup configuration
**References**: Plan §Phase 1.1, Plan §Phase 1.2, Plan §Phase 1.3, Plan §Phase 1.4

- [▶] (1) Convert `OilChangeTracker\OilChangeTracker.Web.csproj` and `OilChangeTracker.Tests\OilChangeTracker.Tests.csproj` to SDK-style and set `<TargetFramework>net10.0</TargetFramework>` per Plan §Phase 1.1
- [ ] (2) Project files converted to SDK-style and target net10.0 (**Verify**)
- [ ] (3) Remove incompatible packages and `packages.config`, add required ASP.NET Core packages, and apply package upgrades per Plan §Phase 1.2 (see Plan for full lists)
- [ ] (4) Package references updated and `dotnet restore` completes successfully (**Verify**)
- [ ] (5) Create `Program.cs` and `appsettings.json` / `appsettings.Development.json` with startup configuration per Plan §Phase 1.3 and §Phase 1.4
- [ ] (6) Build solution and fix compilation issues introduced by conversion (follow Plan §Phase 1 breaking changes)
- [ ] (7) Solution builds with 0 errors (**Verify**)
- [ ] (8) Commit changes with message: "TASK-002: Project conversion, package updates, and foundation setup"

### [ ] TASK-003: Configure routing, dependency injection, and middleware pipeline
**References**: Plan §2.1, Plan §2.2, Plan §2.3

- [ ] (1) Migrate routing to ASP.NET Core endpoint routing and remove `RouteConfig.cs`/legacy route registrations per Plan §2.1
- [ ] (2) Register services in `Program.cs` (AddControllersWithViews, AddDbContext, AddIdentity, AddAuthentication, custom services) per Plan §2.2
- [ ] (3) Implement middleware pipeline (UseExceptionHandler/UseHsts, UseHttpsRedirection, UseStaticFiles, UseRouting, UseAuthentication, UseAuthorization, custom middleware) per Plan §2.3
- [ ] (4) Build solution and fix compilation/configuration issues related to DI and middleware
- [ ] (5) Solution builds with 0 errors (**Verify**)
- [ ] (6) Commit changes with message: "TASK-003: Configure routing, DI, and middleware pipeline"

### [ ] TASK-004: Migrate authentication to ASP.NET Core Identity
**References**: Plan §3.1

- [ ] (1) Install and configure `Microsoft.AspNetCore.Identity` and `Microsoft.AspNetCore.Identity.EntityFrameworkCore` and update `ApplicationUser`/`ApplicationDbContext` per Plan §3.1
- [ ] (2) Update `AccountController` and related views to use `UserManager`/`SignInManager` namespaces and Core patterns per Plan §3.1
- [ ] (3) Apply automated database migrations (use `dotnet ef` tooling if configured) per Plan §3.1
- [ ] (4) Build solution and resolve compilation or migration issues
- [ ] (5) Solution builds with 0 errors and migrations applied successfully (**Verify**)
- [ ] (6) Commit changes with message: "TASK-004: Migrate to ASP.NET Core Identity"

### [ ] TASK-005: Replace bundling, migrate filters, and update views
**References**: Plan §3.2, Plan §3.3, Plan §3.4

- [ ] (1) Replace `System.Web.Optimization` with `WebOptimizer.Core` (or configured alternative) and update layout bundle references per Plan §3.2
- [ ] (2) Migrate global filters to ASP.NET Core filter registration and remove `FilterConfig.cs` per Plan §3.3
- [ ] (3) Create `Views/_ViewImports.cshtml` and optional `Views/_ViewStart.cshtml`, update `_Layout.cshtml` and other views for Core tag helpers per Plan §3.4
- [ ] (4) Build solution and fix compilation/rendering-related issues arising from view changes
- [ ] (5) Solution builds with 0 errors (**Verify**)
- [ ] (6) Commit changes with message: "TASK-005: Bundling, filters, and views migration"

### [ ] TASK-006: Run automated tests, finalize build verification, and commit fixes
**References**: Plan §4.1, Plan §4.2, Plan §4.3

- [ ] (1) Run unit test project `OilChangeTracker.Tests` and any other automated test suites per Plan §4.1
- [ ] (2) Fix any test failures (reference Plan §Phase 3 breaking changes where applicable)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures and full solution builds with 0 errors (**Verify**)
- [ ] (5) Commit test and build fixes with message: "TASK-006: Complete testing and build verification"


