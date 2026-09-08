using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int ManagerEmployeeId { get; set; }

    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual Employee ManagerEmployee { get; set; } = null!;
}
