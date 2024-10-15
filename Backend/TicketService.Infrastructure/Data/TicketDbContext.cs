using Microsoft.EntityFrameworkCore;
using TicketService.Domain.Models;

namespace TicketService.Infrastructure.Data
{
    public class TicketDbContext : DbContext, ITicketDbContext
    {
        public TicketDbContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(options =>
            {
                options.Property(x => x.Status).HasConversion<string>();
            });
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default
        )
        {
            var entities = ChangeTracker
                .Entries()
                .Where(x =>
                    x.Entity is Ticket
                    && (x.State == EntityState.Added || x.State == EntityState.Modified)
                );
            foreach (var entityEntry in entities)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((Ticket)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }

                ((Ticket)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
