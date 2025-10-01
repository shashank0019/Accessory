using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessoryCreation.Models
{
    // -------------------------------
    // Catalog Models
    // -------------------------------

    // Output of ITHW_AMT_AccessoryCreation_GetAccessories
    public class AccessoryCatalogDto
    {
        public int AccessoryWFMID { get; set; }
        public string AccessoryName { get; set; }
        public string AccessoryType { get; set; }
    }

    // Input for IJTM_AML_AccessoryCreation_AddNewAccessory
    public class AccessoryCreateDto
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

    // -------------------------------
    // Request Models
    // -------------------------------

    // Insert into Master: ITHW_AMT_AccessoryCreation_Master_Insert
    public class AccessoryRequestMasterDto
    {
        public int MasterId { get; set; }

        [Required]
        public int InitiatorEmpId { get; set; }
    }

    // Insert details: ITHW_AMT_AccessoryCreation_Insert
    public class AccessoryRequestDetailDto
    {
        public int AccessoryMID { get; set; }
        public string AccessoryType { get; set; }
        public string AccessoryName { get; set; }
        public string Make { get; set; }
        public DateTime? AcqDate { get; set; }
        public int AccessoryCount { get; set; }
        public string ProductStatus { get; set; }
        public string Remarks { get; set; }
    }

    // Update inventory: ITHW_AMT_AccessoryQuantity_InsertUpdate
    public class AccessoryInventoryDto
    {
        public int AccessoryMID { get; set; }
        public string AccessoryType { get; set; }
        public string AccessoryName { get; set; }
        public int AccessoryCount { get; set; }
        public string ProductStatus { get; set; }
    }

    // -------------------------------
    // Query Models
    // -------------------------------

    // Output of ITHW_AMT_AccessoryCreation_GetRequestedAccessoryDetails
    public class AccessoryRequestDetailResultDto
    {
        public int AccessoryWFMID { get; set; }
        public string AccessoryName { get; set; }
        public int AccessoryCount { get; set; }
        public string ProductStatus { get; set; }
        public string Remarks { get; set; }
    }
}
