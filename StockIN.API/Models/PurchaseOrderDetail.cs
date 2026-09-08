using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class PurchaseOrderDetail
{
    public int PurchaseOrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public short Quantity { get; set; }

    public decimal Discount { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
