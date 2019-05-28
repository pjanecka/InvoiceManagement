using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Models
{
    public class InvoiceContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }

        public InvoiceContext(DbContextOptions<InvoiceContext> options): base(options)
        { }
    }
}
