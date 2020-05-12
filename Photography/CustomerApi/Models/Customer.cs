using SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class Customer
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

        #nullable enable
        /// <summary>
        /// VATNUmber incase it is a company.
        /// </summary>
        public string? VATNumber { get; set; }
        #nullable disable
    }
}
