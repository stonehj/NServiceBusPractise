using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus.AcceptanceTesting.Support;
using NServiceBus;
using NServiceBus.Logging;
using EndpointConfiguration = NServiceBus.EndpointConfiguration;

namespace ClientUI
{
    internal class Program
    {
        private static ILog _log = LogManager.GetLogger<Program>();

        //create messaging endpoint
        private static async Task Main()
        {
            Console.Title = "ClientUI";

            //EndpointConfiguration class defintes all settings which determines how endpoint operates
            //string ClientUI is endpoint name
            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            //            //transport is a setting which NServiceBus uses to send and receives messages
            //            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            
            //MSMQ does not support natively Publish/Subscribe
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

            //message subscription info stored in memory instead
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            //error queue specified
            endpointConfiguration.SendFailedMessagesTo("error");

            //creates message queues required by endpoints
            endpointConfiguration.EnableInstallers();

            //establises commands of type PlaceOrder should be sent to the Sales endpoint
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            //starts endpoint
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            //endpoint runs until user presses enter and then stops
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        private static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                _log.Info("Press 'P' to place and order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        //Instantiate the command
                        //creating a PlaceOrder object
                        var command = new PlaceOrder
                        {
                            //supply unique value for OrderId
                            OrderId = Guid.NewGuid().ToString()
                        };

                        //Send the command to the local endpoint
                        //SendLocal is method on the IEndpointInstance interface
                        //Local means that command is handled by the same endpoint that sent it
                        //It is not sent to an external endpoint
                        _log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");

                        //Send returns Task so we need an await
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        _log.Info("Unknown input. Please try again.");
                        break;
                        
                }
            }
        }
    }

}
