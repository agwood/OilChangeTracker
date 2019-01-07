
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OilChangeTracker.Entities
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "VIN")]
        public string Vin { get; set; }
        [Required]
        [Display(Name = "Vehicle Nickname")]
        public string Nickname { get; set; }
        [Required]
        [Display(Name = "Make")]
        public string Make { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Required]
        [Display(Name = "Year")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit Year")]
        public string Year { get; set; }
        public string Color { get; set; }
        public Guid UserId { get; set; }

    }
}