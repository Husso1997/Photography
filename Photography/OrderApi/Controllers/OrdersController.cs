using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Infrastructure;
using OrderApi.Models;
using PhotographyConsole.Infrastructure;
using SharedModels;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IRepository<Order> _orderRepo;
        private readonly IConverter _converter;
        private readonly IMessagePublisher _messagePublisher;

        public OrdersController(IRepository<Order> orderRepo, IMessagePublisher messagePublisher, IConverter converter)
        {
            _orderRepo = orderRepo;
            _messagePublisher = messagePublisher;
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
                if (orderDTO is null || orderDTO.ID > 0 || orderDTO.Products is null)
                {
                    return BadRequest();
                }

                Order orderToCreate = _converter.Convert<Order>(orderDTO);
                Order orderCreated = await _orderRepo.Create(orderToCreate);
                OrderDTO orderCreatedDTO = _converter.Convert<OrderDTO>(orderCreated);
                await _messagePublisher.PublishOrderCreatedMessage(orderCreatedDTO);

                return new OkObjectResult(orderCreatedDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error occured - Try again in a few seconds");
            }
        }

        // PUT: api/Orders/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] OrderDTO orderDTO)
        {
            return await CancelOrCompleteOrder(id, orderDTO, true);
        }

        // PUT: api/Orders/5/complete
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id, [FromBody] OrderDTO orderDTO)
        {
            return await CancelOrCompleteOrder(id, orderDTO, false);
        }

        private async Task<IActionResult> CancelOrCompleteOrder(int id, OrderDTO orderDTO, bool cancel)
        {
            try
            {
                if (orderDTO is null || id != orderDTO.ID)
                {
                    return BadRequest();
                }
                Order orderToUpdateProgress = await _orderRepo.GetById(id);
                if (orderToUpdateProgress is null)
                {
                    return NotFound();
                }
                else
                {
                    orderToUpdateProgress.OrderProgress = cancel ? Models.OrderProgress.Cancelled : Models.OrderProgress.Completed;
                }
                var orderUpdatedProgress = await _orderRepo.Update(orderToUpdateProgress);
                string contentMsg = $"Order with ID {orderUpdatedProgress.ID} has been successfully " + (cancel ? "cancelled" : "completed");
                return Content(contentMsg);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error occured - Try again in a few seconds");
            }
        }
    }
}
