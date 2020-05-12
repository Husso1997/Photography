using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using ProductApi.Models;
using SharedModels;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IConverter _converter;

        public ProductsController(IRepository<Product> productRepo, IConverter converter)
        {
            _productRepo = productRepo;
            _converter = converter;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productRepo.GetAll();
                List<ProductDTO> productsDTO = _converter.Convert<List<ProductDTO>>(products);
                return new OkObjectResult(productsDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Product product = await _productRepo.GetById(id);
                if (product is null)
                {
                    return NotFound();
                }
                return new OkObjectResult(_converter.Convert<ProductDTO>(product));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO is null || productDTO.ID > 0)
                {
                    return BadRequest();
                }
                Product productToCreate = _converter.Convert<Product>(productDTO);
                var productCreated = await _productRepo.Create(productToCreate);

                return new OkObjectResult(_converter.Convert<ProductDTO>(productCreated));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO is null || id != productDTO.ID)
                {
                    return BadRequest();
                }

                Product productToUpdate = _converter.Convert<Product>(productDTO);
                var productUpdated = await _productRepo.Update(productToUpdate);

                return new OkObjectResult(_converter.Convert<ProductDTO>(productUpdated));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productToFind = _productRepo.GetById(id);
                if (productToFind is null)
                {
                    return NotFound();
                }

                var deletedProd = await _productRepo.Delete(id);
                return new OkObjectResult(_converter.Convert<ProductDTO>(deletedProd));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }
    }
}
