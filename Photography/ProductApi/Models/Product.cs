using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description of the offer/product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// How many times this product has been bought.
        /// </summary>
        public int AmountOfTimesBought { get; set; }
    }
}
