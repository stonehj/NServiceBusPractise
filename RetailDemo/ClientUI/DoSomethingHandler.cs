using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    //to process a message, create a Handler, a class that implements IHandleMessages<T> where T is a message type
    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }

        //Instead of explicitly retuning a Task you can add async keyword
        /*public async Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            //Do something with the message here
        }*/
    }
}
