using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace cassandra_ativ6.Models
{
    public partial class InvoicesDbContext : DbContext
    {
        public InvoicesDbContext()
        {
        }

        public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<InvoiceItem> InvoiceItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;database=invoices_system;uid=root;pwd=061191");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
    
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Invoice");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("InvoiceNumber");
                
                entity.Property(e => e.CustomerName)
                    .HasColumnType("varchar(55)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci")
                    .HasColumnName("CustomerName");

                entity.Property(e => e.CustomerAddress)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci")
                    .HasColumnName("CustomerAddress");

                entity.Property(e => e.InvoiceItemId)
                    .HasColumnName("InvoiceItemId")
                    .HasColumnType("int(11)")
                    .HasColumnName("InvoiceItemID");

                entity.Property(e => e.ServiceDescription)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci")
                    .HasColumnName("ServiceDescription");

                entity.Property(e => e.InvoiceItemQuantity)
                    .HasColumnType("int(11)")
                    .HasColumnName("InvoiceItemQuantity");

                entity.Property(e => e.InvoiceItemUnitValue)
                    .HasColumnName("InvoiceItemUnitValue");

                entity.Property(e => e.ResourceName)
                    .IsRequired()
                    .HasColumnType("varchar(65)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci")
                    .HasColumnName("ResourceName");

                entity.Property(e => e.QualificationName)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci")
                    .HasColumnName("QualificationName");

                entity.Property(e => e.InvoiceItemTaxValue)
                    .HasColumnName("InvoiceItemTaxValue");

                 entity.Property(e => e.InvoiceItemDiscountValue)
                    .HasColumnName("InvoiceItemDiscountValue");

                entity.Property(e => e.InvoiceItemSubtotal)
                    .HasColumnName("InvoiceItemSubtotal");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
