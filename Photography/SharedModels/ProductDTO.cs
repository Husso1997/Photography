using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class ProductDTO
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
