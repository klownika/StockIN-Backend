using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StockIN.API.Models;

namespace StockIN.API.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<InventoryStock> InventoryStocks { get; set; }

    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in", "like", "ilike", "is", "match", "imatch", "isdistinct" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("audit_logs_pkey");

            entity.ToTable("audit_logs");

            entity.HasIndex(e => e.EmployeeId, "audit_logs_employee_id_idx");

            entity.HasIndex(e => new { e.TableName, e.RecordId }, "audit_logs_table_record_idx");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(20)
                .HasColumnName("action_type");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.LogDate).HasColumnName("log_date");
            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .HasColumnName("table_name");

            entity.HasOne(d => d.Employee).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_audit_logs_employees");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "uq_categories_category_name").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnName("category_name");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.HasIndex(e => e.City, "customers_city_idx");

            entity.HasIndex(e => e.CompanyName, "customers_company_name_idx");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnName("city");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnName("company_name");
            entity.Property(e => e.ContactName)
                .HasMaxLength(30)
                .HasColumnName("contact_name");
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnName("country");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.HasIndex(e => e.LastName, "employees_last_name_idx");

            entity.HasIndex(e => e.ReportsTo, "employees_reports_to_idx");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnName("country");
            entity.Property(e => e.Extension)
                .HasMaxLength(4)
                .HasColumnName("extension");
            entity.Property(e => e.FirstName)
                .HasMaxLength(10)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.HomePhone)
                .HasMaxLength(24)
                .HasColumnName("home_phone");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.PhotoPath)
                .HasMaxLength(255)
                .HasColumnName("photo_path");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnName("region");
            entity.Property(e => e.ReportsTo).HasColumnName("reports_to");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");
            entity.Property(e => e.TitleOfCourtesy)
                .HasMaxLength(25)
                .HasColumnName("title_of_courtesy");

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation)
                .HasForeignKey(d => d.ReportsTo)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_employees_reports_to");
        });

        modelBuilder.Entity<InventoryStock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("inventory_stock_pkey");

            entity.ToTable("inventory_stock");

            entity.HasIndex(e => e.ProductId, "inventory_stock_product_id_idx");

            entity.HasIndex(e => new { e.WarehouseId, e.ProductId }, "uq_inventory_stock_warehouse_product").IsUnique();

            entity.Property(e => e.StockId).HasColumnName("stock_id");
            entity.Property(e => e.MinStockLevel).HasColumnName("min_stock_level");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryStocks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_inventory_stock_products");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryStocks)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("fk_inventory_stock_warehouses");
        });

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("inventory_transactions_pkey");

            entity.ToTable("inventory_transactions");

            entity.HasIndex(e => e.TransactionDate, "inventory_transactions_date_idx");

            entity.HasIndex(e => e.EmployeeId, "inventory_transactions_employee_id_idx");

            entity.HasIndex(e => e.ProductId, "inventory_transactions_product_id_idx");

            entity.HasIndex(e => e.WarehouseId, "inventory_transactions_warehouse_id_idx");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_inventory_transactions_employees");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_inventory_transactions_products");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_inventory_transactions_warehouses");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.HasIndex(e => e.CustomerId, "orders_customer_id_idx");

            entity.HasIndex(e => e.EmployeeId, "orders_employee_id_idx");

            entity.HasIndex(e => e.OrderDate, "orders_order_date_idx");

            entity.HasIndex(e => e.ShipVia, "orders_ship_via_idx");

            entity.HasIndex(e => e.ShippedDate, "orders_shipped_date_idx");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("customer_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Freight)
                .HasPrecision(19, 4)
                .HasColumnName("freight");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.RequiredDate).HasColumnName("required_date");
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(60)
                .HasColumnName("ship_address");
            entity.Property(e => e.ShipCity)
                .HasMaxLength(15)
                .HasColumnName("ship_city");
            entity.Property(e => e.ShipCountry)
                .HasMaxLength(15)
                .HasColumnName("ship_country");
            entity.Property(e => e.ShipName)
                .HasMaxLength(40)
                .HasColumnName("ship_name");
            entity.Property(e => e.ShipPostalCode)
                .HasMaxLength(10)
                .HasColumnName("ship_postal_code");
            entity.Property(e => e.ShipRegion)
                .HasMaxLength(15)
                .HasColumnName("ship_region");
            entity.Property(e => e.ShipVia).HasColumnName("ship_via");
            entity.Property(e => e.ShippedDate).HasColumnName("shipped_date");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_orders_customers");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_orders_employees");

            entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipVia)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_orders_shippers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("pk_order_details");

            entity.ToTable("order_details");

            entity.HasIndex(e => e.ProductId, "order_details_product_id_idx");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Discount)
                .HasPrecision(4, 3)
                .HasColumnName("discount");
            entity.Property(e => e.Quantity)
                .HasDefaultValue((short)1)
                .HasColumnName("quantity");
            entity.Property(e => e.UnitCost)
                .HasPrecision(19, 4)
                .HasColumnName("unit_cost");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(19, 4)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_details_orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_order_details_products");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.HasIndex(e => e.OrderId, "payments_order_id_idx");

            entity.HasIndex(e => e.PurchaseOrderId, "payments_purchase_order_id_idx");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasPrecision(19, 4)
                .HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(30)
                .HasColumnName("payment_method");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_payments_orders");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_payments_purchase_orders");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.CategoryId, "products_category_id_idx");

            entity.HasIndex(e => e.ProductName, "products_product_name_idx");

            entity.HasIndex(e => e.SupplierId, "products_supplier_id_idx");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Discontinued).HasColumnName("discontinued");
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnName("product_name");
            entity.Property(e => e.QuantityPerUnit)
                .HasMaxLength(20)
                .HasColumnName("quantity_per_unit");
            entity.Property(e => e.ReorderLevel).HasColumnName("reorder_level");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(19, 4)
                .HasColumnName("unit_price");
            entity.Property(e => e.UnitsInStock).HasColumnName("units_in_stock");
            entity.Property(e => e.UnitsOnOrder).HasColumnName("units_on_order");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_products_categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_products_suppliers");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("purchase_orders_pkey");

            entity.ToTable("purchase_orders");

            entity.HasIndex(e => e.EmployeeId, "purchase_orders_employee_id_idx");

            entity.HasIndex(e => e.OrderDate, "purchase_orders_order_date_idx");

            entity.HasIndex(e => e.SupplierId, "purchase_orders_supplier_id_idx");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.RequiredDate).HasColumnName("required_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_purchase_orders_employees");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_purchase_orders_suppliers");
        });

        modelBuilder.Entity<PurchaseOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseOrderId, e.ProductId }).HasName("pk_purchase_order_details");

            entity.ToTable("purchase_order_details");

            entity.HasIndex(e => e.ProductId, "purchase_order_details_product_id_idx");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Discount)
                .HasPrecision(4, 3)
                .HasColumnName("discount");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(19, 4)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pod_products");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(d => d.PurchaseOrderId)
                .HasConstraintName("fk_pod_purchase_orders");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.HasKey(e => e.ShipperId).HasName("shippers_pkey");

            entity.ToTable("shippers");

            entity.Property(e => e.ShipperId).HasColumnName("shipper_id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnName("company_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.City, "suppliers_city_idx");

            entity.HasIndex(e => e.CompanyName, "suppliers_company_name_idx");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnName("city");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnName("company_name");
            entity.Property(e => e.ContactName)
                .HasMaxLength(30)
                .HasColumnName("contact_name");
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnName("country");
            entity.Property(e => e.HomePage).HasColumnName("home_page");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("warehouses_pkey");

            entity.ToTable("warehouses");

            entity.HasIndex(e => e.WarehouseName, "uq_warehouses_name").IsUnique();

            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.Location)
                .HasMaxLength(60)
                .HasColumnName("location");
            entity.Property(e => e.ManagerEmployeeId).HasColumnName("manager_employee_id");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(40)
                .HasColumnName("warehouse_name");

            entity.HasOne(d => d.ManagerEmployee).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.ManagerEmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_warehouses_manager");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
