using _100uslug.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Models
{
    public class CustomClasses
    {
        public Dictionary<string, Type> CustomTypes { get; private set; }

        public CustomClasses(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BaseContext>();
            var tables = context.Set<Table>().Include(s => s.Columns).Include(s => s.ForeignKeys).Include(s => s.Indexes);
            foreach (var table in tables)
            {
                CustomTypes.Add(table.Name, DynamicTypeBuilder.CreateNewType(table.Name, table.Columns.Select(s => new Field()
                {
                    FieldName = s.Name,
                    FieldType = Type.GetType(s.Type)
                }).ToList()));
            }
        }

        public void AddNewType(string name, Type type)
        {
            if (!CustomTypes.ContainsKey(name))
            {
                CustomTypes.Add(name, type);
            }
        }

        public void UpdateType(string name, Type type)
        {
            if (CustomTypes.ContainsKey(name))
            {
                CustomTypes[name] = type;
            }
            else
            {
                AddNewType(name, type);
            }
        }
    }
}
