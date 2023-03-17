using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDbExample.Models;
using MongoDbExample.Services;

namespace MongoDbExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ProductsService _productsService;

        public ProductsController(ProductsService productsService) =>
            _productsService = productsService;

        [HttpGet]
        public async Task<List<Products>> Get() =>
            await _productsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Products>> Get(string id)
        {
            var product = await _productsService.GetAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Products newProduct)
        {
            await _productsService.CreateAsync(newProduct);

            return CreatedAtAction(nameof(Get), new { id = newProduct.id }, newProduct);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Products updatedProduct)
        {
            var product = await _productsService.GetAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            updatedProduct.id = product.id;

            await _productsService.UpdateAsync(id, updatedProduct);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productsService.GetAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            await _productsService.RemoveAsync(id);

            return NoContent();
        }
    }
}
