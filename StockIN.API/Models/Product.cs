using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public string? QuantityPerUnit { get; set; }

    public decimal UnitPrice { get; set; }

    public short UnitsInStock { get; set; }

    public short UnitsOnOrder { get; set; }

    public short ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual Supplier? Supplier { get; set; }
}
