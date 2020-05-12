﻿using PhotographyConsole.Infrastructure;
using RestSharp;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
{
    public class ProductServiceGateway : IServiceGateway<ProductDTO>
    {
        private readonly Uri _productServiceBaseUrl;
        public ProductServiceGateway(Uri productServiceBaseUrl)
        {
            _productServiceBaseUrl = productServiceBaseUrl;
        }

        public async Task<List<ProductDTO>> GetAll()
        {
            RestClient resClient = new RestClient
            {
                BaseUrl = _productServiceBaseUrl
            };

            RestRequest restRequest = new RestRequest(Method.GET);

            IRestResponse<List<ProductDTO>> response = await resClient.ExecuteAsync<List<ProductDTO>>(restRequest);

            List<ProductDTO> productsDTO = response.Data;

            return productsDTO;
        }
    }
}
