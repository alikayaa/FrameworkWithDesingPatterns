using DAI.Rule.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAI.Rule.Validation.Definition
{
    [DomainObjectName("Customer")]
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}