using _100uslug.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _100uslug.Models
{
    public class BaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=sto_uslug;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TableConfiguration());
            builder.ApplyConfiguration(new ColumnConfiguration());
            builder.ApplyConfiguration(new ForeignConfiguration());
            builder.ApplyConfiguration(new IndexConfiguration());
        }
    }

    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("table").HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasMaxLength(30);
            builder.Property(p => p.Id).HasColumnName("id").IsRequired();
            builder.Property(p => p.ModelName).HasColumnName("model_name").IsRequired().HasMaxLength(30);

            builder.HasMany(s => s.Columns).WithOne().HasForeignKey(s => s.TableId).HasPrincipalKey(s => s.Id);
            builder.HasMany(s => s.ForeignKeys).WithOne().HasForeignKey(s => s.TableId).HasPrincipalKey(s => s.Id);
            builder.HasMany(s => s.Indexes).WithOne().HasForeignKey(s => s.TableId).HasPrincipalKey(s => s.Id);
        }
    }

    public class ColumnConfiguration : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.ToTable("column").HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasMaxLength(30);
            builder.Property(p => p.Id).HasColumnName("id").IsRequired();
            builder.Property(p => p.ModelName).HasColumnName("model_name").IsRequired().HasMaxLength(30);
            builder.Property(p => p.IsNullable).HasColumnName("is_nullable").IsRequired();
            builder.Property(p => p.IsPKey).HasColumnName("is_pkey").IsRequired();
            builder.Property(p => p.AutoIncrement).HasColumnName("autoincrement").IsRequired();
            builder.Property(p => p.DefaultValue).HasColumnName("default_value").IsRequired();
            builder.Property(p => p.TableId).HasColumnName("table_id").IsRequired();
            builder.Property(p => p.Type).HasColumnName("type").IsRequired();
        }
    }

    public class ForeignConfiguration : IEntityTypeConfiguration<ForeignKey>
    {
        public void Configure(EntityTypeBuilder<ForeignKey> builder)
        {
            builder.ToTable("foreign_key").HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasMaxLength(30);
            builder.Property(p => p.Id).HasColumnName("id").IsRequired();
            builder.Property(p => p.TableId).HasColumnName("table_id").IsRequired();

            builder.Property(p => p.ForeignField).HasColumnName("foreign_field").IsRequired().HasMaxLength(30);
            builder.Property(p => p.ForeignTable).HasColumnName("foreign_table").IsRequired();
            builder.Property(p => p.KeyField).HasColumnName("key_field").IsRequired();
        }
    }

    public class IndexConfiguration : IEntityTypeConfiguration<TableIndex>
    {
        public void Configure(EntityTypeBuilder<TableIndex> builder)
        {
            builder.ToTable("foreign_key").HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasMaxLength(30);
            builder.Property(p => p.Id).HasColumnName("id").IsRequired();
            builder.Property(p => p.TableId).HasColumnName("table_id").IsRequired();

            builder.Property(p => p.IsUnique).HasColumnName("is_unique").IsRequired();
            builder.Property(p => p.Field).HasColumnName("field").IsRequired();
        }
    }
}
