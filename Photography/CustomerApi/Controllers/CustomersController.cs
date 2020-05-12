using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IConverter _converter;

        public CustomersController(IRepository<Customer> customerRepo, IConverter converter)
        {
            _customerRepo = customerRepo;
            _converter = converter;
        }


        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _customerRepo.GetAll();
                List<CustomerDTO> customersDTO = _converter.Convert<List<CustomerDTO>>(customers);
                return new OkObjectResult(customersDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // GET: api/Customers/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Customer customer = await _customerRepo.GetById(id);
                if (customer is null)
                {
                    return NotFound();
                }
                return new OkObjectResult(_converter.Convert<CustomerDTO>(customer));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customerDTO)
        {
            try
            {
                if (customerDTO is null || customerDTO.ID > 0)
                {
                    return BadRequest();
                }
                Customer customerToCreate = _converter.Convert<Customer>(customerDTO);
                var customerCreated = await _customerRepo.Create(customerToCreate);

                return new OkObjectResult(_converter.Convert<CustomerDTO>(customerCreated));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CustomerDTO customerDTO)
        {
            try
            {
                if (customerDTO is null || id != customerDTO.ID)
                {
                    return BadRequest();
                }

                Customer customerToUpdate = _converter.Convert<Customer>(customerDTO);
                var customerUpdated = await _customerRepo.Update(customerToUpdate);

                return new OkObjectResult(_converter.Convert<CustomerDTO>(customerUpdated));
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
                var customerToFind = _customerRepo.GetById(id);
                if (customerToFind is null)
                {
                    return NotFound();
                }

                var deletedCustomer = await _customerRepo.Delete(id);
                return new OkObjectResult(_converter.Convert<CustomerDTO>(deletedCustomer));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, try again");
            }
        }
    }
}
