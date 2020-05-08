using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class Product
    {
        /// <summary>
        ///  ID of the product.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }

    }
}
