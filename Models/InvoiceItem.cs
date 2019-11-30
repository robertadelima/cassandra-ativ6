using System;

namespace cassandra_ativ6.Models
{
    public partial class InvoiceItem
    {
        public int InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int InvoiceItemId { get; set; }
        public string ServiceDescription { get; set; }
        public int InvoiceItemQuantity { get; set; }
        public double InvoiceItemUnitValue { get; set; }
        public string ResourceName { get; set; }
        public string QualificationName { get; set; }
        public double? InvoiceItemTaxValue { get; set; }
        public double? InvoiceItemDiscountValue { get; set; }
        public double InvoiceItemSubtotal { get; set; }
        public double? InvoiceTotal { get; set; }
    }
}