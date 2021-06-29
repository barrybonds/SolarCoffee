using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;
        public InventoryService(SolarDbContext dbContext, ILogger<InventoryService> logger) {
            _db = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Create a snapshot record using the provided Productinventory instance
        /// </summary>
        /// <param name="inventory"></param>
        public void CreateSnapshot()
        {
            var now = DateTime.UtcNow;
            var inventories = _db.ProductInventories
                .Include(inv => inv.Product)
                .ToList();
            foreach (var inventory in inventories)
            {
                var snapShot = new ProductInventorySnapshot
                {
                    SnapshotTime = now,
                    Product = inventory.Product,
                    QunatityOnHand = inventory.QuantityOnHand
                };

                _db.Add(snapShot);
            }
        }
        /// <summary>
        /// Gets a Productinventory instance by product ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories
            .Include(pi => pi.Product)
            .FirstOrDefault(pi => pi.Product.Id == productId);

        }
        /// <summary>
        /// Returns all current inventory from the database
        /// </summary>
        /// <returns></returns>
        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
            .Include(pi => pi.Product)
            .Where(pi => !pi.Product.IsArchived)
            .ToList();
        }
        /// <summary>
        /// Return Snapshot history for the previous 6 hours
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ProductInventorySnapshot> GetSnapshotsHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(2);

            return _db.GetProductInventorySnapshots
            .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchived).ToList();
           
        }
        /// <summary>
        /// Updates number of units available of the provided product id
        /// Adjusts QuantityOnHand by adjustment value
        /// <param name="id">productId</param>
        /// <param name="adjustment">productId</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// </summary>

        public ServiceResponse<ProductInventory> UpdateunitAvailable(int id, int adjustment)
        {
            var now = DateTime.UtcNow;

            try
            {
                var inventory = _db.ProductInventories
                .Include(inv => inv.Product)
                .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error creating Inventory Snapshot");
                    _logger.LogError(e.StackTrace);
                   
                }
                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = inventory,
                    Message = $"Product {id} inventory adjusted",
                    Time = now
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = $"Error updating productInventory QuantityOnHand",
                    Time = now
                };
            }
        }

        //private void CreateSnapshot()
        //{
        //    var now = DateTime.UtcNow;
        //    var inventories = _db.ProductInventories.Include(inv => inv.Product).ToList();
        //    foreach (var inventory in inventories) {
        //        var snapshot = new ProductInventorySnapshot
        //        {
        //            SnapshotTime = now,
        //            Product = inventory.Product,
        //            QunatityOnHand = inventory.QuantityOnHand

        //        };
        //        _db.Add(snapshot);
        //    }
        //}
    }
}
