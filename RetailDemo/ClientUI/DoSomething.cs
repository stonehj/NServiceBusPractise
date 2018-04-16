using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    public class DoSomething : ICommand //lets NServiceBus know that class is a Comment
    {
        public string SomeProperty { get; set; }

    }
}
