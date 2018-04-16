using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NServiceBus;
using ICommand = NServiceBus.ICommand;

namespace Messages
{
    public class PlaceOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}
