using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class InventoryStock
{
    public int StockId { get; set; }

    public int WarehouseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int MinStockLevel { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
