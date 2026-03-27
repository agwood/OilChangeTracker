using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OilChangeTracker.DataContexts;
using OilChangeTracker.Models;
using OilChangeTracker.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OilChangeTracker.Controllers
{
    public class OilChangesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OilChangesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OilChanges/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var vehicles = await _context.Vehicles
                .Where(u => u.OwnerId == userId)
                .ToListAsync();

            var viewModel = new OilChangeFormViewModel
            {
                Vehicles = vehicles,
                VehicleSelectedAlready = false
            };

            return View(viewModel);
        }

        // POST: OilChanges/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OilChangeFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Vehicles = await _context.Vehicles.ToListAsync();
                return View(viewModel);
            }

            var userId = _userManager.GetUserId(User);
            
            // Verify user owns this vehicle
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == viewModel.SelectedVehicleId && v.OwnerId == userId);
            
            if (vehicle == null)
            {
                return Unauthorized();
            }

            var oilChange = new OilChange
            {
                VehicleId = viewModel.SelectedVehicleId,
                Mileage = viewModel.Mileage,
                WorkDate = DateTime.Parse(viewModel.WorkDate),
                OilBrand = viewModel.OilBrand,
                OilFilterBrand = viewModel.OilFilterBrand,
                OilFilterModel = viewModel.OilFilterModel,
                OilViscosity = viewModel.OilViscosity
            };

            _context.OilChanges.Add(oilChange);
            await _context.SaveChangesAsync();

            return RedirectToAction("WorkDetails", "Vehicles", new { id = viewModel.SelectedVehicleId });
        }

        // GET: OilChanges/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);
            var oilChange = await _context.OilChanges
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Id == id && o.Vehicle.OwnerId == userId);

            if (oilChange == null)
            {
                return NotFound();
            }

            return View(oilChange);
        }

        // POST: OilChanges/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var oilChange = await _context.OilChanges
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Id == id && o.Vehicle.OwnerId == userId);

            if (oilChange == null)
            {
                return Unauthorized();
            }

            var vehicleId = oilChange.VehicleId;
            _context.OilChanges.Remove(oilChange);
            await _context.SaveChangesAsync();

            return RedirectToAction("WorkDetails", "Vehicles", new { id = vehicleId });
        }
    }
}
