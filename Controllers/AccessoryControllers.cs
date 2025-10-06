using Microsoft.AspNetCore.Mvc;
using AccessoryCreation.BusinessLogic;
using AccessoryCreation.Models;
using System;
using System.Collections.Generic;

namespace AccessoryCreation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessoryController : ControllerBase
    {
        private readonly AccessoryBusinessLogic _businessLogic;

        public AccessoryController(AccessoryBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        // -------------------------------
        // Catalog Endpoints
        // -------------------------------
        [HttpGet]
        public IActionResult GetAccessories()
        {
            var data = _businessLogic.GetAccessories();
            return Ok(new { Success = true, Data = data });
        }

        [HttpPost("AddNewAccessory")]
        public IActionResult AddNewAccessory([FromBody] string accessoryName)
        {
            try
            {
                var newId = _businessLogic.AddNewAccessory(accessoryName);
                return Ok(new { Message = "Accessory added successfully", AccessoryID = newId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // -------------------------------
        // Request Endpoints
        // -------------------------------
        [HttpPost("master")]
        public IActionResult InsertMaster([FromBody] int initiatorEmpId)
        {
            try
            {
                int masterId = _businessLogic.InsertRequestMaster(initiatorEmpId);
                return Ok(new { Success = true, MasterId = masterId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("InsertAccessoryRequest")]
        public IActionResult InsertAccessoryRequest([FromBody] AccessoryRequestDetailDto accessory)
        {
            try
            {
                _businessLogic.InsertAccessoryRequest(accessory);
                return Ok(new { Success = true, Message = "Accessory inserted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        [HttpPost("InsertOrUpdateAccessoryQuantity")]
        public IActionResult InsertOrUpdateAccessoryQuantity([FromBody] AccessoryQuantityDto accessory)
        {
            try
            {
                _businessLogic.InsertOrUpdateAccessoryQuantity(accessory);
                return Ok(new { Message = "Accessory quantity updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{requestId}")]
        public IActionResult GetAccessoryDetails(int requestId)
        {
            var data = _businessLogic.GetAccessoryDetails(requestId);
            if (data == null || data.Count == 0)
                return NotFound(new { Success = false, Message = "No accessories found for this request." });

            return Ok(new { Success = true, Data = data });
        }
    }
}
