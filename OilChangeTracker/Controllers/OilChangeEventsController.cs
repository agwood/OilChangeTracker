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
            var viewModel = new OilChangeEventFormViewModel
            {
                Vehicles = _context.Vehicles.ToList()
            };

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

                foreach (ModelState state in ViewData.ModelState.Values.Where(x => x.Errors.Count > 0))
                {
                    state.ToString();
                }
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
                VehicleId = viewModel.Vehicle,
                WorkDate = DateTime.Parse(viewModel.WorkDate)

            };
            _context.OilChangeEvents.Add(oilChangeEvent);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}