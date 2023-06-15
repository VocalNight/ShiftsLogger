using Microsoft.EntityFrameworkCore;

namespace ShiftsLogger.Model
{
    public class ShiftLoggerContext : DbContext
    {
        public ShiftLoggerContext(DbContextOptions<ShiftLoggerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<ShiftItem>().ToTable("ShiftItem");
        }

        public DbSet<ShiftItem> ShiftItems { get; set; }
    }
}
