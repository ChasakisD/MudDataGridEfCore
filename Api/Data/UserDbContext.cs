using Microsoft.EntityFrameworkCore;
namespace MudDataGridEfCore.Data;

public class UserEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class UserDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserEntity>().ToTable("Users");
        modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<UserEntity>().Property(x => x.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<UserEntity>().Property(x => x.FirstName).IsRequired();
        modelBuilder.Entity<UserEntity>().Property(x => x.LastName).IsRequired();
    }
}
