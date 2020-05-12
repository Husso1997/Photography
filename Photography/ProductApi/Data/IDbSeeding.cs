using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Data
{
    public interface IDbSeeding
    {
        public void SeedWithData(ProductApiContext productApiContext);

    }
}
