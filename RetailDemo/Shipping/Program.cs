using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Shipping
{
    class Program
    {
        static async Task Main()
        {
                Console.Title = "Shipping";

                //important for the two endpoints to exactly match otherwise the endpoints will not be able to understand each other
                //the only difference is the name "Sales" in the console title and Endpoint Configuartion constructor
                //this means the Sales endpoint will create its own queue where it will listen for messages
                //two processes with their own queues but can now send messages between them
                var endpointConfiguration = new EndpointConfiguration("Billing");

                var transport = endpointConfiguration.UseTransport<MsmqTransport>();
                endpointConfiguration.UsePersistence<InMemoryPersistence>();
                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.EnableInstallers();

                var routing = transport.Routing();
                routing.RegisterPublisher(typeof(OrderPlaced), "Sales");
                routing.RegisterPublisher(typeof(OrderBilled), "Billing");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                    .ConfigureAwait(false);

                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();

                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }
