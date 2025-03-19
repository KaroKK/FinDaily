using FinBackend.Api.Models;
using FinBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FinBackend.Data
{
    public class FinContext : DbContext
    {
        public FinContext(DbContextOptions<FinContext> options) : base(options) { }

        // Tables in database
        public DbSet<User> Users { get; set; }
        public DbSet<Categ> Categ { get; set; }
        public DbSet<PayWay> PayWays { get; set; }
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<PlanBud> PlanBuds { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categ>()
                .HasOne(c => c.TheUser)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}