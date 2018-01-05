using System;
using System.Collections.Generic;

namespace Ambition.WebApi.Contract
{
    public class OrderResourceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<OrderLineResourceModel> OrderLines { get; set; }
    }

    public class OrderLineResourceModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}