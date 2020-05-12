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
        /// Description of the offer/product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// How many times this product has been bought.
        /// </summary>
        public int AmountOfTimesBought { get; set; }
    }
}
