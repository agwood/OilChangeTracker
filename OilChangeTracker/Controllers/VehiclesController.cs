using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OilChangeTracker.DataContexts;
using OilChangeTracker.Models;
using OilChangeTracker.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OilChangeTracker.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehiclesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Vehicles/Create
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new VehicleFormViewModel();
            return View(viewModel);
        }

        // POST: Vehicles/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
                return View("Create", viewModel);
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "User is not authenticated");
                return View("Create", viewModel);
            }

            var vehicle = new Vehicle
            {
                Nickname = viewModel.Nickname,
                Vin = viewModel.Vin,
                Make = viewModel.Make,
                Model = viewModel.Model,
                Year = viewModel.Year,
                Color = viewModel.Color,
                OwnerId = userId
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Vehicles");
        }

        // GET: Vehicles
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var vehicles = await _context.Vehicles
                .Where(u => u.OwnerId == userId)
                .AsNoTracking()
                .ToListAsync();

            // Map to ViewModel using manual mapping
            var viewModels = vehicles.Select(v => new VehicleViewModel
            {
                Id = v.Id,
                Nickname = v.Nickname,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = v.Color,
                Vin = v.Vin
            }).ToList();

            return View("Index", viewModels);
        }

        // GET: Vehicles/WorkDetails/5
        [Authorize]
        public async Task<ActionResult> WorkDetails(int id)
        {
            var oilChanges = _context.OilChanges
                .Where(e => e.VehicleId == id)
                .OrderByDescending(e => e.WorkDate);
            
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(e => e.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var viewModel = new OilChangesViewModel
            {
                OilChanges = oilChanges,
                Vehicle = vehicle
            };

            return View(viewModel);
        }

        // GET: Vehicles/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);
            var vehicle = await _context.Vehicles
                .Where(e => e.Id == id)
                .Where(u => u.OwnerId == userId)
                .FirstOrDefaultAsync();
            
            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleEditFormViewModel = new VehicleEditFormViewModel
            {
                Nickname = vehicle.Nickname,
                Vin = vehicle.Vin,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color,
                Id = vehicle.Id
            };

            return View(vehicleEditFormViewModel);
        }

        // POST: Vehicles/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleEditFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userId = _userManager.GetUserId(User);
            var vehicle = new Vehicle
            {
                Nickname = viewModel.Nickname,
                Vin = viewModel.Vin,
                Make = viewModel.Make,
                Model = viewModel.Model,
                Year = viewModel.Year,
                Color = viewModel.Color,
                OwnerId = userId,
                Id = viewModel.Id
            };

            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Vehicles");
        }
    }
}
