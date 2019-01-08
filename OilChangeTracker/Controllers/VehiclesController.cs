﻿using Microsoft.AspNet.Identity;
using OilChangeTracker.DataContexts;
using OilChangeTracker.Models;
using OilChangeTracker.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace OilChangeTracker.Controllers
{

    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Vehicles
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new VehicleFormViewModel();
            
            return View(viewModel);
        }

        // POST: Vehicles
        [Authorize]
        [HttpPost]
        public ActionResult Create(VehicleFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
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
                OwnerId = User.Identity.GetUserId()
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return RedirectToAction("Index", "Vehicles");
        }

        // GET: All Vehicles
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var vehicles = _context.Vehicles.Where(u => u.OwnerId == userId);

            var viewModel = new VehicleViewModel()
            {
                Vehicles = vehicles
            };

            return View("Index", viewModel);

        }

        [Authorize]
        public ActionResult CreateOilChangeEvent(int VehicleId)
        {
            Session["VehicleId"] = VehicleId;

            return RedirectToAction("Create", "OilChangeEvents");
        }

        // GET: Vehicle oil changes for Vehicle
        [Authorize]
        public ActionResult WorkDetails(int Id)
        {
            //Get Oil changes for this vehicle Id
            var oilChangeEvents = _context.OilChangeEvents.Where(e => e.VehicleId == Id).OrderByDescending(e => e.WorkDate);

            var viewModel = new OilChangesViewModel()

            {
                OilChangeEvents = oilChangeEvents
            };

            return View(viewModel);

        }
    }


}