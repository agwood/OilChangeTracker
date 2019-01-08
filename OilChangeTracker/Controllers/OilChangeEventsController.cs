using Microsoft.AspNet.Identity;
using OilChangeTracker.DataContexts;
using OilChangeTracker.Models;
using OilChangeTracker.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OilChangeTracker.Controllers
{
    public class OilChangeEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OilChangeEventsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: OilChangeEvents
        [Authorize]
        public ActionResult Create()
        {
            int vehicleId;
            
            var userId = User.Identity.GetUserId();
            var viewModel = new OilChangeEventFormViewModel
            {
                Vehicles = _context.Vehicles.Where(u => u.OwnerId == userId).ToList()
            };
            if (Session["VehicleId"] != null)
            {
                vehicleId = Convert.ToInt32(Session["VehicleId"]);
                viewModel.SelectedVehicleId = vehicleId;
                Session["VehicleId"] = null;
            }
            
            
            return View(viewModel);
        }

        // POST: OilChangeEvents
        [Authorize]
        [HttpPost]
        public ActionResult Create (OilChangeEventFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Vehicles = _context.Vehicles.ToList();
                return View("Create", viewModel);
            }

            var oilChangeEvent = new OilChangeEvent
            {
                Mileage = viewModel.Mileage,
                OtherNotes = viewModel.OtherNotes,
                OilBrand = viewModel.OilBrand,
                OilViscosity = viewModel.OilViscosity,
                OilFilterBrand = viewModel.OilFilterBrand,
                OilFilterModel = viewModel.OilFilterModel,
                VehicleId = viewModel.SelectedVehicleId,
                WorkDate = DateTime.Parse(viewModel.WorkDate)

            };
            _context.OilChangeEvents.Add(oilChangeEvent);
            _context.SaveChanges();

            return RedirectToAction("Index", "Vehicles");
        }

    }

}