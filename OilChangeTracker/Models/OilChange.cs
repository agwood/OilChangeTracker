﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OilChangeTracker.Models
{
    public class OilChange
    {
        [Key]
        public int Id { get; set; }

        public Vehicle Vehicle{ get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime WorkDate { get; set; }

        [Required]
        public int Mileage { get; set; }

        [StringLength(50)]
        public string OilFilterBrand { get; set; }

        [StringLength(50)]
        public string OilFilterModel { get; set; }

        [StringLength(50)]
        public string OilBrand { get; set; }

        [StringLength(50)]
        public string OilViscosity { get; set; }

        [StringLength(500)]
        public string OtherNotes { get; set; }
    }
}