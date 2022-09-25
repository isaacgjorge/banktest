
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public class ApplicationContext: DbContext
{
    public DbSet<Account>? Account { get; set; }
    public DbSet<User>? User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "BankTestDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(_ =>new { _.Email }).IsUnique();
    }


   
}

