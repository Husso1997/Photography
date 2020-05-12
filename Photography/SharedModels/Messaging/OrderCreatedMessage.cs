using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Messaging
{
    public class OrderCreatedMessage
    {
        public OrderDTO OrderDTO { get; set; }
    }
}
