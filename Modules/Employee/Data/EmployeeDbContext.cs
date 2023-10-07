using Microsoft.EntityFrameworkCore;

namespace Hera.Modules.Employee.Data;

public class EmployeeDbContext : DbContext {
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

    public DbSet<FamilyMember> FamilyMembers { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // Use your own connection string here
    //     optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PersonDb;Trusted_Connection=True;");
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<FamilyMember>().ToTable("FamilyMember");
    }
}