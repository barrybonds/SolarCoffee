using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly InventoryService _inventoryService;
        //  private readonly ICustomerService _customerService;

        public InventoryController(ILogger<InventoryController> logger, InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }
        [HttpGet("/api/inventory")]
        public IActionResult GetCurrentInventory()
        {
            _logger.LogInformation("Getting all invesntoy...");
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(pi => new ProductInventoryModel { 
                  Id = pi.Id,
                  Product = ProductMapper.SerializeProductModel(pi.Product),
                  IdealQuantity = pi.IdealQuantity,
                  QuantityOnHand = pi.QuantityOnHand
                })
                .OrderBy(inv => inv.Product.Name) 
                .ToList();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        public IActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation($"Updating investory" + 
                $"for {shipment.ProductId} - " +
                $" Adjustment: {shipment.Adjustment}");
            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            var inventory = _inventoryService.UpdateunitAvailable(id, adjustment);
            return Ok(inventory);

        }
    }
}
