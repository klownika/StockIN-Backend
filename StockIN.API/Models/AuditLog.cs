using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int EmployeeId { get; set; }

    public string ActionType { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public int RecordId { get; set; }

    public DateTime LogDate { get; set; }

    public string Details { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
