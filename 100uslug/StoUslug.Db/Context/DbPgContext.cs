using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoUslug.Db.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace StoUslug.Db.Context
{
    /// <summary>
    /// Postgresql context
    /// </summary>
    public class DbPgContext : DbContext
    {        
        /// <summary>
        /// settings set
        /// </summary>
        public DbSet<Settings> Settings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Table> Tables { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public DbPgContext(DbContextOptions<DbPgContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=sto_uslug;Username=postgres;Password=postgres");
        }

        /// <summary>
        /// create models
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var _logger = this.GetService<ILogger<DbPgContext>>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new EntityConfiguration<Settings>());

            foreach (var type in Assembly.GetAssembly(typeof(Entity)).GetTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var configType = typeof(EntityConfiguration<>).MakeGenericType(type);
                    var config = Activator.CreateInstance(configType);
                    GetType().GetMethod(nameof(ApplyConf), BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(type).Invoke(this, new object[] { modelBuilder, config });

                }
            }

            var tables = Tables;
            foreach(var table in tables)
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
                    applyMethod.Invoke(modelBuilder, new object[] { conf });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception while Apply config: {ex.Message} st: {ex.StackTrace}");
                }
            }
        }

        private void ApplyConf<T>(ModelBuilder modelBuilder, EntityConfiguration<T> config) where T : class, IEntity
        {
            modelBuilder.ApplyConfiguration(config);
        }        
    }

    public static class DynamicTypeBuilder
    {
        private static ModuleBuilder _moduleBuilder;

        public static Dictionary<string, Type> DynamicTypes { get; } = new Dictionary<string, Type>();

        private static void CreateModuleBuilder()
        {
            var an = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        }

        public static object CreateNewObject(string typeSignature, List<Field> fields)
        {
            var newType = CreateNewType(typeSignature, fields);
            var newObject = Activator.CreateInstance(newType);
            return newObject;
        }
        public static Type CreateNewType(string typeSignature, List<Field> fields)
        {
            TypeBuilder tb = GetTypeBuilder(typeSignature);
            tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            foreach (var field in fields)
                CreateProperty(tb, field.FieldName, field.FieldType);
            Type objectType = tb.CreateType();
            DynamicTypes.Add(typeSignature, objectType);
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string typeSignature)
        {
            if (_moduleBuilder == null) CreateModuleBuilder();
            TypeBuilder tb = _moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            System.Reflection.Emit.PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }

    public class Field
    {
        public string FieldName;
        public Type FieldType;
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
