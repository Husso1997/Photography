using SharedModels;
using SharedModels.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
{
    public interface IMessagePublisher
    {
        Task PublishCustomerValidatedMessage(CustomerValidatedMessage csm);
    }
}
