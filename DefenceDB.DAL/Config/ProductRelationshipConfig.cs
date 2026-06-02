using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DefenceDB.EL.Models;

namespace DefenceDB.DAL.Config;

public class ProductRelationshipConfig : IEntityTypeConfiguration<ProductRelationship>
{
    public void Configure(EntityTypeBuilder<ProductRelationship> builder)
    {
        builder.HasKey(pr => pr.Id);

        builder.HasOne(pr => pr.SourceProduct)
            .WithMany(p => p.SourceRelationships)
            .HasForeignKey(pr => pr.SourceProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pr => pr.TargetProduct)
            .WithMany(p => p.TargetRelationships)
            .HasForeignKey(pr => pr.TargetProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data buraya eklenebilir
        // builder.HasData(...);
    }
}
