using Newtonsoft.Json;
using RestSharp;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotographyConsole.Infrastructure
{
    public class OrderServiceGateway
    {
        private readonly Uri _orderServiceBaseUrl;
        public OrderServiceGateway(Uri orderServiceBaseUrl)
        {
            _orderServiceBaseUrl = orderServiceBaseUrl;
        }

        public async Task<List<OrderDTO>> GetAll()
        {
            RestClient resClient = new RestClient
            {
                BaseUrl = _orderServiceBaseUrl
            };

            RestRequest restRequest = new RestRequest(Method.GET);

            IRestResponse response = await resClient.ExecuteAsync(restRequest);

            List<OrderDTO> ordersDTO = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Content);

            return ordersDTO;
        }

        public async Task<OrderDTO> CreateOrder(OrderDTO orderDTO)
        {
            RestClient resClient = new RestClient
            {
                BaseUrl = _orderServiceBaseUrl
            };

            RestRequest restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(orderDTO);

            IRestResponse<OrderDTO> response = await resClient.ExecuteAsync<OrderDTO>(restRequest);

            OrderDTO orderDTOCreated = response.Data;

            return orderDTOCreated;
        }
    }
}
