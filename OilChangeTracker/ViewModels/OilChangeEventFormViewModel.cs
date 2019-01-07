using OilChangeTracker.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OilChangeTracker.ViewModels
{

    public class OilChangeEventFormViewModel
    {
        [Required]
        [Display(Name = "Vehicle Nickname")]
        public byte Vehicle { get; set; }

        public IEnumerable<Vehicle> Vehicles { get; set; }


        [Required]
        [Display(Name = "Date Work Completed")]
        public string WorkDate { get; set; }

        [Required]
        [Display(Name = "Mileage")]
        public int Mileage { get; set; }

        [StringLength(50)]
        [Display(Name = "Oil Filter Brand")]
        public string OilFilterBrand { get; set; }

        [StringLength(50)]
        [Display(Name = "Oil Filter Model")]
        public string OilFilterModel { get; set; }

        [StringLength(50)]
        [Display(Name = "Oil Brand")]
        public string OilBrand { get; set; }

        [StringLength(50)]
        [Display(Name = "Oil Viscosity")]
        public string OilViscosity { get; set; }

        [StringLength(500)]
        [Display(Name = "Miscellaneous Notes")]
        public string OtherNotes { get; set; }
    }
}