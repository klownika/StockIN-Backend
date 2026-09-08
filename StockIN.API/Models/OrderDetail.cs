using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitCost { get; set; }

    public short Quantity { get; set; }

    public decimal Discount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
