using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class CustomerDTO
    {
        /// <summary>
        ///  ID of the product.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }
    
        #nullable enable
        /// <summary>
        /// VATNUmber incase it is a company.
        /// </summary>
        public string? VATNumber { get; set; }
        #nullable disable
    }
}
