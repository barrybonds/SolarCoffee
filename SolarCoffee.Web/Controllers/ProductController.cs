﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Gettig all products");
            var products =  _productService.GetAllProducts();
            var productViewModels = products.Select(ProductMapper.SerializeProductModel);

            return Ok(products);
        }

        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving Product");
            var archiveResult = _productService.ArchiveProduct(id);

            return Ok(archiveResult);
        }



    }
}
