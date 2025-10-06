using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessoryCreation.Models
{
    // -------------------------------
    // Catalog Models
    // -------------------------------
    public class AccessoryCatalogDto
    {
        public int AccessoryWFMID { get; set; }
        public string AccessoryName { get; set; }
        public string AccessoryType { get; set; }
    }

    public class AccessoryCreateDto
    {
        [Required]
        public string AccessoryName { get; set; }

        public string? AccessoryType { get; set; }
        public string? Make { get; set; }
        public DateTime? AcqDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Accessory count must be greater than zero.")]
        public int AccessoryCount { get; set; }

        public string? ProductStatus { get; set; }
        public string? Remarks { get; set; }
    }

    // -------------------------------
    // Request Models
    // -------------------------------
    public class AccessoryRequestMasterDto
    {
        public int MasterId { get; set; }
        [Required]
        public int InitiatorEmpId { get; set; }
    }

    public class AccessoryRequestDetailDto
    {
        public long AccessoryWFMID { get; set; }
        public int AccessoryMID { get; set; }
        public string? Make { get; set; }
        public DateTime? AcqDate { get; set; }
        public int AccessoryCount { get; set; }
        public string? ProductStatus { get; set; }
        public string? Remarks { get; set; }
        public string? AccessoryType { get; set; }
    }

    // -------------------------------
    // Quantity Update Model
    // -------------------------------
    public class AccessoryQuantityDto
    {
        [Required]
        public int AccessoryMID { get; set; }
        public string? AccessoryType { get; set; }
        public string? AccessoryName { get; set; }
        public string? ProductStatus { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Accessory count must be greater than zero.")]
        public int AccessoryCount { get; set; }
    }

    // -------------------------------
    // Query Models
    // -------------------------------
    public class AccessoryRequestDetailResultDto
    {
        public int AccessoryWFMID { get; set; }
        public string? AccessoryName { get; set; }
        public int AccessoryCount { get; set; }
        public string? ProductStatus { get; set; }
        public string? Remarks { get; set; }
    }
}
