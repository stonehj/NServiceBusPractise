using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using static NServiceBus.Logging.LogManager;

namespace Billing
{ 
    //Billing subscribes to OrderPlaced so it can handle payment transaction
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static ILog _log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            _log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");
            return Task.CompletedTask;
        }
    }
}
