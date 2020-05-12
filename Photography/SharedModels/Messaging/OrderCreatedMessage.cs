using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Messaging
{
    public class OrderCreatedMessage
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }

    }
}
