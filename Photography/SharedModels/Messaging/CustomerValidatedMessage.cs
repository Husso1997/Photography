using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Messaging
{
    public class CustomerValidatedMessage
    {
        public int OrderID { get; set; }
        public List<int> ProductIDs { get; set; }
        public bool CustomerExists { get; set; }
    }
}
