using Doc.Pulse.Core.Entities;
using Doc.Pulse.Core.Entities._Kernel;
using Microsoft.EntityFrameworkCore;

namespace Doc.Pulse.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public virtual DbSet<AccountOrganization> AccountOrganizations { get; set; }
    public virtual DbSet<Agency> Agencies { get; set; }
    public virtual DbSet<Appropriation> Appropriations { get; set; }
    public virtual DbSet<CodeCategory> CodeCategories { get; set; }
    public virtual DbSet<ObjectCode> ObjectCodes { get; set; }
    public virtual DbSet<Program> Programs { get; set; }
    public virtual DbSet<Receipt> Receipts { get; set; }
    public virtual DbSet<RFP> Rfps { get; set; }
    public virtual DbSet<Vendor> Vendors { get; set; }
    public virtual DbSet<UserStub> UserStubs { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
