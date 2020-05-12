﻿using PhotographyConsole.Infrastructure;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotographyConsole
{
    class Program
    {
        private static readonly ProductServiceGateway _productServiceGateway
        = new ProductServiceGateway(new Uri("https://localhost:44397/api/products"));

        private static readonly OrderServiceGateway _orderServiceGateway
        = new OrderServiceGateway(new Uri("https://localhost:44362/api/orders"));

        private static List<OrderDTO> _orders;
        private static List<ProductDTO> _products;
        static async Task Main(string[] args)
        {
            string option;
            while ((option = Console.ReadLine().ToLower()) != "quit")
            {
                MessageWithOptions();
                await OptionSelector(option);
            }
        }

        private static async Task OptionSelector(string option)
        {
            if (int.TryParse(option, out int canParse))
            {
                switch (canParse)
                {
                    case 1:
                        await GetProducts();
                        break;
                    case 2:
                        await GetOrders();
                        break;
                    default:
                        Console.WriteLine("Number out of range");
                        break;

                }
            }
            else
            {
                Console.WriteLine("Please type a numeric value");
            }
        }

        private static async Task GetProducts()
        {
            _products = await _productServiceGateway.GetAll();
            _products.ForEach(x => Console.WriteLine($"ID: {x.ID} Name: {x.Name} Price: {x.Price}"));
        }

        private static async Task GetOrders()
        {
            _orders = await _orderServiceGateway.GetAll();

            foreach (OrderDTO order in _orders)
            {
                string productIds = "";
                order.Products.ForEach(x => productIds += $"({x.ProductID})");
                Console.WriteLine($"ID: {order.ID} CustomerID: {order.CustomerID} ProductIDs: {productIds} OrderProgress: {order.OrderProgress}");
            }
        }

        private static void MessageWithOptions()
        {
            StringBuilder stringMessage = new StringBuilder();
            stringMessage.AppendLine("Welcome to Photography Solution");
            stringMessage.AppendLine("Type '1' To show products");
            stringMessage.AppendLine("Type '2' To show orders");
            Console.WriteLine(stringMessage.ToString());
        }
    }
}
