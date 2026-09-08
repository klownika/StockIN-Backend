using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? PurchaseOrderId { get; set; }

    public int? OrderId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual PurchaseOrder? PurchaseOrder { get; set; }
}
