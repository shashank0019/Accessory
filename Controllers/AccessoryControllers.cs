using Microsoft.AspNetCore.Mvc;
using AccessoryCreation.BusinessLogic;
using AccessoryCreation.Models;
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

        #region Catalog Endpoints
        [HttpGet]
        public IActionResult GetAccessories()
        {
            var data = _businessLogic.GetAccessories();
            return Ok(new { Success = true, Data = data });
        }

        [HttpPost("add")]
        public IActionResult AddAccessory([FromBody] AccessoryModel model)
        {
            try
            {
                int newId = _businessLogic.AddAccessory(model);
                return Ok(new { Success = true, AccessoryId = newId });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Request Endpoints
        [HttpPost("master")]
        public IActionResult InsertMaster([FromBody] int initiatorEmpId)
        {
            try
            {
                int masterId = _businessLogic.InsertRequestMaster(initiatorEmpId);
                return Ok(new { Success = true, MasterId = masterId });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("details/{masterId}")]
        public IActionResult InsertDetails(int masterId, [FromBody] List<Accessory> accessories)
        {
            try
            {
                _businessLogic.InsertRequestDetails(masterId, accessories);
                return Ok(new { Success = true, Message = "Accessory details inserted successfully." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("approve/{masterId}")]
        public IActionResult ApproveAccessoryRequest(int masterId, [FromBody] List<Accessory> accessories)
        {
            try
            {
                _businessLogic.InsertIntoInventory(masterId, accessories);
                return Ok(new { Success = true, Message = "Accessories added to inventory successfully." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
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
        #endregion
    }
}
