using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilChangeTracker.Models
{
    public class VehicleImages
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Vehicle")]
        public Vehicle VehicleId { get; set; }



    }
}