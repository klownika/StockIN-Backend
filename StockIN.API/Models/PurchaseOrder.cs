using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int SupplierId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
