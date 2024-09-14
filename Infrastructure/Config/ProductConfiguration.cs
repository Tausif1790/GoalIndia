using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;  // Importing the namespace for configuring entity types

namespace Infrastructure.Config;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)"); // Configuring the Price property with a specific SQL data type
        builder.Property(x => x.Name).IsRequired();                    // Configuring the Name property to be required (non-nullable)
    }
}

