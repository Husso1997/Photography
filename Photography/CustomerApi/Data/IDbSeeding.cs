﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public interface IDbSeeding
    {
        public void SeedWithData(CustomerApiContext orderApiContext);

    }
}
