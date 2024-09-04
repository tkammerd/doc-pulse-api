//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Doc.Pulse.Core.Entities.ApiInformationAggregate;

//namespace Doc.Pulse.Infrastructure.Data.Config.ApiInformationAggregate;

//internal class ApiInformationConfig
//{
//    internal class CodeConfig : IEntityTypeConfiguration<ApiInformation>
//    {
//        public void Configure(EntityTypeBuilder<ApiInformation> builder)
//        {
//            builder.ToTable("ApiInformation", "info").HasKey(o => o.Id);

//            builder.Property(p => p.Id).HasColumnName("Id");

//            builder.Property(p => p.Name).HasColumnName("Name").HasColumnType("varchar(100)");
//            builder.Property(p => p.Description).HasColumnName("Description").HasColumnType("varchar(500)");

//            builder.OwnsOne(o => o.Effective).Property(p => p.Effective).HasColumnName("Effective").IsRequired();

//            builder.OwnsOne(o => o.Maintainer).Property(p => p.Agency).HasColumnName("MaintainerAgency").HasColumnType("varchar(100)").IsRequired();
//            builder.OwnsOne(o => o.Maintainer).Property(p => p.Section).HasColumnName("MaintainerSection").HasColumnType("varchar(100)").IsRequired();
//            builder.OwnsOne(o => o.Maintainer).Property(p => p.Name).HasColumnName("MaintainerName").HasColumnType("varchar(100)").IsRequired();
//            builder.OwnsOne(o => o.Maintainer).Property(p => p.Attn).HasColumnName("MaintainerAttn").HasColumnType("varchar(200)").IsRequired();
//            builder.OwnsOne(o => o.Maintainer).Property(p => p.Email).HasColumnName("MaintainerEmail").HasColumnType("varchar(300)").IsRequired();
//            builder.OwnsOne(o => o.Maintainer).Property(p => p.Phone).HasColumnName("MaintainerPhone").HasColumnType("varchar(20)").IsRequired();

//            builder.OwnsOne(o => o.Owner).Property(p => p.Agency).HasColumnName("OwnerAgency").HasColumnType("varchar(100)").IsRequired();
//            builder.OwnsOne(o => o.Owner).Property(p => p.Section).HasColumnName("OwnerSection").HasColumnType("varchar(100)").IsRequired();
//            builder.OwnsOne(o => o.Owner).Property(p => p.Name).HasColumnName("OwnerName").HasColumnType("varchar(100)").IsRequired();
//            builder.OwnsOne(o => o.Owner).Property(p => p.Attn).HasColumnName("OwnerAttn").HasColumnType("varchar(200)").IsRequired();
//            builder.OwnsOne(o => o.Owner).Property(p => p.Email).HasColumnName("OwnerEmail").HasColumnType("varchar(300)").IsRequired();
//            builder.OwnsOne(o => o.Owner).Property(p => p.Phone).HasColumnName("OwnerPhone").HasColumnType("varchar(20)").IsRequired();


//        }
//    }
//}
