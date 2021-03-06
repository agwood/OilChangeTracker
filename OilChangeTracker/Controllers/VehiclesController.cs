﻿using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Identity;
using OilChangeTracker.DataContexts;
using OilChangeTracker.Models;
using OilChangeTracker.ViewModels;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace OilChangeTracker.Controllers
{
    [Authorize]
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
            var vehicles = _context.Vehicles
                                    .Where(u => u.OwnerId == userId)
                                    .ProjectTo<VehicleViewModel>()
                                    .ToList();


            return View("Index", vehicles);

        }

        [Authorize]
        public ActionResult CreateOilChange(int VehicleId)
        {
            Session["VehicleId"] = VehicleId;

            return RedirectToAction("Create", "OilChanges"); ;
        }

        // GET: Vehicle oil changes for Vehicle
        [Authorize]
        public ActionResult WorkDetails(int Id)
        {
            //Get Oil changes for this vehicle Id
            var OilChanges = _context.OilChanges.Where(e => e.VehicleId == Id).OrderByDescending(e => e.WorkDate);
            var vehicle = _context.Vehicles.Where(e => e.Id == Id).FirstOrDefault();

            var viewModel = new OilChangesViewModel()

            {
                OilChanges = OilChanges,
                Vehicle = vehicle
            };

            return View(viewModel);

        }

        // GET: /Vehicles/Edit/1
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var vehicle = _context.Vehicles
                                        .Where(e => e.Id == Id)
                                        .Where(u => u.OwnerId == userId)
                                        .FirstOrDefault();
            if (vehicle == null)
            {
                return HttpNotFound();
            }

            //var vehicle = new VehicleFormViewModel();

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

        // POST: /Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleEditFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var vehicle = new Vehicle
                {
                    Nickname = viewModel.Nickname,
                    Vin = viewModel.Vin,
                    Make = viewModel.Make,
                    Model = viewModel.Model,
                    Year = viewModel.Year,
                    Color = viewModel.Color,
                    OwnerId = User.Identity.GetUserId(),
                    Id = viewModel.Id
                };

                _context.Entry(vehicle).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", "Vehicles");
            }

            return View(viewModel);
        }
    }


}