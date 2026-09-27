//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace NWCodeFirstMVC.Domain
//{
//    public partial class northwindContext : DbContext
//    {
//        public northwindContext()
//        {
//        }

//        public northwindContext(DbContextOptions<northwindContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Product> Products { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-18OKP1O\\SQLEXPRESS;Database=northwind;Trusted_Connection=True;MultipleActiveResultSets=true;");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Product>(entity =>
//            {
//                entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

//                entity.HasIndex(e => e.CategoryId, "CategoryID");

//                entity.HasIndex(e => e.ProductName, "ProductName");

//                entity.HasIndex(e => e.SupplierId, "SupplierID");

//                entity.HasIndex(e => e.SupplierId, "SuppliersProducts");

//                entity.Property(e => e.ProductId).HasColumnName("ProductID");

//                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

//                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

//                entity.Property(e => e.ProductName).HasMaxLength(40);

//                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);

//                entity.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

//                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

//                entity.Property(e => e.UnitPrice)
//                    .HasColumnType("money")
//                    .HasDefaultValueSql("((0))");

//                entity.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

//                entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
