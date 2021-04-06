﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.ViewModels;

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
        /// <summary>
        /// Adds a new product 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("/api/product")]
        public ActionResult AddProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Adding Product");
            var newProduct = ProductMapper.SerializeProductModel(product);
            var newProductResponse = _productService.CreateProduct(newProduct);

            return Ok(newProductResponse);
        }

        /// <summary>
        /// Returns all products
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Gettig all products");
            var products =  _productService.GetAllProducts();
            var productViewModels = products.Select(ProductMapper.SerializeProductModel);
            
            return Ok(products);
        }
        /// <summary>
        /// Archives an existing product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving Product");
            var archiveResult = _productService.ArchiveProduct(id);

            return Ok(archiveResult);
        }



    }
}
