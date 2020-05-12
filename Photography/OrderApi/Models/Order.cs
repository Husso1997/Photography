using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderProduct> Products { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public OrderProgress OrderProgress { get; set; }
    }

    public class OrderProduct
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
    }
    public enum OrderProgress { Verifiying, Cancelled, Accepted, Completed }
}
