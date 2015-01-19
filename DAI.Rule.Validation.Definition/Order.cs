using DAI.Rule.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAI.Rule.Validation.Definition
{
    [DomainObjectName("Order")]
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}