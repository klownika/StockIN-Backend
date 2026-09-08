using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class InventoryTransaction
{
    public int TransactionId { get; set; }

    public int WarehouseId { get; set; }

    public int ProductId { get; set; }

    public int EmployeeId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime TransactionDate { get; set; }

    public int ReferenceId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
