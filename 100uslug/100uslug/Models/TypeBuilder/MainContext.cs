using _100uslug.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace _100uslug.Models
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var _logger = this.GetService<ILogger<MainContext>>();
            var _baseContext = this.GetService<BaseContext>();
            var tables = _baseContext.Set<Table>().ToList();
            foreach (var table in tables)
            {
                try
                {
                    var tableType = DynamicTypeBuilder.CreateNewType(table.ModelName, table.Columns.Select(s => new Field()
                    {
                        FieldName = s.ModelName,
                        FieldType = Type.GetType(s.Type)
                    }).ToList());
                    var confType = typeof(CustomConfiguration<>).MakeGenericType(tableType);
                    var conf = Activator.CreateInstance(confType, table);

                    var applyMethod = typeof(ModelBuilder).GetMethod("ApplyConfiguration");
                    applyMethod.Invoke(builder, new object[] { conf });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception while Apply config: {ex.Message} st: {ex.StackTrace}");
                }
            }

        }
    }

    public class CustomConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        private readonly Table _table;

        public CustomConfiguration(Table table)
        {
            _table = table;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(_table.Name);
            var keyColumn = _table.Columns.FirstOrDefault(s => s.IsPKey);
            if (keyColumn != null)
            {
                builder.HasKey(keyColumn.Name);
            }
            foreach (var column in _table.Columns)
            {
                builder.Property(column.ModelName)
                    .HasColumnName(column.Name)
                    .IsRequired(!column.IsNullable);
            }
        }
    }
}
