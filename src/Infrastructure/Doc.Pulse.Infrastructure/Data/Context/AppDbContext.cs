using Doc.Pulse.Core.Entities;
using Doc.Pulse.Core.Entities._Kernel;
using Microsoft.EntityFrameworkCore;

namespace Doc.Pulse.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public virtual DbSet<CodeCategory> CodeCategories { get; set; }
    public virtual DbSet<ObjectCode> ObjectCodes { get; set; }
    public virtual DbSet<UserStub> UserStubs { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
