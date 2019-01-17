using OilChangeTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace OilChangeTracker.ViewModels
{
    public class VehicleViewModel
    {

        public int Id { get; set; }

        [Display(Name = "VIN")]
        [StringLength(20)]
        public string Vin { get; set; }

        [Required]
        [Display(Name = "Vehicle Nickname")]
        [StringLength(255)]
        public string Nickname { get; set; }

        [Required]
        [Display(Name = "Make")]
        [StringLength(255)]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Model")]
        [StringLength(255)]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Year")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit Year")]
        [StringLength(4)]
        public string Year { get; set; }

        [StringLength(255)]
        public string Color { get; set; }

        public ApplicationUser Owner { get; set; }

        public string LatestMileage { get; private set; }
    }
}