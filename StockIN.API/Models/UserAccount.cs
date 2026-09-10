using System;
using System.Collections.Generic;

namespace StockIN.API.Models;

public partial class UserAccount
{
    public int UserAccountId { get; set; }

    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
