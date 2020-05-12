using OrderApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public interface IDbSeeding
    {
        public void SeedWithData(OrderApiContext orderApiContext);

    }
}
