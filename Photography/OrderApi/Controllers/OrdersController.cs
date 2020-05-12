using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using SharedModels;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IRepository<Order> _orderRepo;
        private readonly IConverter _converter;

        public OrdersController(IRepository<Order> orderRepo, IConverter converter)
        {
            _orderRepo = orderRepo;
            _converter = converter;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {
                var orders = await _orderRepo.GetAll();
                List<OrderDTO> orderDTOs = _converter.Convert<List<OrderDTO>>(orders);
                return new OkObjectResult(orderDTOs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error occured - Try again in a few seconds");
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Order order = await _orderRepo.GetById(id);
                if (order is null)
                {
                    return NotFound();
                }
                return new OkObjectResult(_converter.Convert<OrderDTO>(order));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error occured - Try again in a few seconds");
            }
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO is null || orderDTO.ID > 0)
                {
                    return BadRequest();
                }
                Order orderToCreate = _converter.Convert<Order>(orderDTO);
                var orderCreated = await _orderRepo.Create(orderToCreate);

                return new OkObjectResult(_converter.Convert<OrderDTO>(orderCreated));
            }
            catch (Exception)
            {
                return StatusCode(500, "Error occured - Try again in a few seconds");
            }
        }

        // PUT: api/Orders/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO is null || id != orderDTO.ID)
                {
                    return BadRequest();
                }
                Order orderToCancel = await _orderRepo.GetById(id);
                if(orderToCancel ==  null)
                {
                    return NotFound();
                }
                else
                {
                    orderToCancel.OrderProgress = Models.OrderProgress.Cancelled;
                }
                var orderCancelled = await _orderRepo.Update(orderToCancel);

                return Content($"Order with ID {orderCancelled.ID} has been successfully cancelled");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error occured - Try again in a few seconds");
            }
        }
    }
}
