using Microsoft.AspNet.Identity;
using OilChangeTracker.DataContexts;
using OilChangeTracker.Models;
using OilChangeTracker.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OilChangeTracker.Controllers
{
    public class OilChangesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OilChangesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: OilChanges
        [Authorize]
        public ActionResult Create()
        {
            int vehicleId;
            
            var userId = User.Identity.GetUserId();
            var viewModel = new OilChangeFormViewModel
            {
                Vehicles = _context.Vehicles.Where(u => u.OwnerId == userId).ToList(),
                VehicleSelectedAlready = false
                
            };
            if (Session["VehicleId"] != null)
            {
                vehicleId = Convert.ToInt32(Session["VehicleId"]);
                viewModel.SelectedVehicleId = vehicleId;
                var OilChanges = _context.OilChanges.Where(e => e.VehicleId == vehicleId).OrderByDescending(e => e.WorkDate).FirstOrDefault();
                int mileage;
                if (OilChanges != null)
                {
                    mileage = OilChanges.Mileage;
                }
                else
                {
                    mileage = 0;
                }
                viewModel.Mileage = mileage;
                viewModel.VehicleSelectedAlready = true;
                Session["VehicleId"] = null;
            }
            
            
            return View(viewModel);
        }

        // POST: OilChanges
        [Authorize]
        [HttpPost]
        public ActionResult Create (OilChangeFormViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                viewModel.Vehicles = _context.Vehicles.Where(u => u.OwnerId == userId).ToList();
                return View("Create", viewModel);
            }

            var OilChange = new OilChange
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
            _context.OilChanges.Add(OilChange);
            _context.SaveChanges();

            return RedirectToAction("Index", "Vehicles");
        }


        [HttpGet]
        public ActionResult UpdateMileage(int? Id)
        {

            //do the logic for taking the value for textbox

            int mileage;
            if (Id != null)
            {
                var OilChanges = _context.OilChanges.Where(e => e.VehicleId == Id).OrderByDescending(e => e.WorkDate).FirstOrDefault();
                if (OilChanges != null)
                {
                    mileage = OilChanges.Mileage;
                }
                else
                {
                    mileage = 0;
                }
            }
            else
            {
                mileage = 0;
            }
            
            return Json(mileage, JsonRequestBehavior.AllowGet);
        }

    }

}