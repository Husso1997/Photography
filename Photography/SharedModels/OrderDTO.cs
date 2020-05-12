using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class OrderDTO
    {
        public int ID { get; set; }
        public List<OrderProductDTO> Products { get; set; }
        public DateTime CreatedDate { get; set; }
        public OrderProgress OrderProgress { get; set; }
        public int CustomerID { get; set; }
    }

    public class OrderProductDTO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
    }

    public enum OrderProgress { Verifiying, Cancelled, Accepted, Completed }
}
