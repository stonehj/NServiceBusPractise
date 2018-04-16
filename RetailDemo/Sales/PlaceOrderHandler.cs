using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        //logger allows you to use same logging system used by NServiceBus
        //entries writtinbg with the logger appears in log file as well as in constole
        private static ILog _log = LogManager.GetLogger<PlaceOrderHandler>();
        static Random random = new Random();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            //logger records receipt of the PlaceOrder message including value of OrderId
            _log.Info($"Received PlaceOrder, OrderID = {message.OrderId}");

            if (random.Next(0,5) == 0)
            {
                throw new Exception("OOPS");
            }
            

            //PlaceOrder is a command which is a command to do something
            //OrderPlaced is an Event which is an announcement that something has taken place 
            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }
    }
}
