using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessoryCreation.Models
{
    // Represents an Accessory record
    public class Accessory
    {
        public int AccessoryMID { get; set; }
        public int AccessoryWFMID { get; set; }

        [Required]
        public string AccessoryName { get; set; }

        public string AccessoryType { get; set; }
        public string Make { get; set; }
        public DateTime? AcqDate { get; set; }
        public int AccessoryCount { get; set; }
        public string ProductStatus { get; set; }

        [Required]
        public string Remarks { get; set; }
    }

    // For inserting new accessory in catalog
    public class AccessoryModel
    {
        [Required]
        public string AccessoryName { get; set; }

        [Required]
        public string Make { get; set; }

        public string AccessoryType { get; set; }
        public DateTime? AcqDate { get; set; }

        [Range(1, int.MaxValue)]
        public int AccessoryCount { get; set; }

        public string ProductStatus { get; set; }

        [Required]
        public string Remarks { get; set; }
    }

    // Master request (Initiator + List of Accessories)
    public class AccessoryRequestMaster
    {
        public int MasterId { get; set; }
        public int InitiatorEmpId { get; set; }
        public List<Accessory> Accessories { get; set; } = new List<Accessory>();
    }

    // Convenience wrapper for request details
    public class AccessoryRequest
    {
        public int RequestId { get; set; }
        public List<Accessory> Accessories { get; set; } = new List<Accessory>();
    }
}
