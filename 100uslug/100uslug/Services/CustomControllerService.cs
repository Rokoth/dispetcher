using _100uslug.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Services
{
    public class CustomControllerService<T>
    {
        private readonly List<T> _data = new List<T>();

        public CustomControllerService()
        {
            for (int i = 1; i < 5; i++)
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(obj, "SomeValue");
                    }
                    if (prop.PropertyType == typeof(Guid))
                    {
                        prop.SetValue(obj, Guid.NewGuid());
                    }
                    if (prop.PropertyType == typeof(DateTimeOffset))
                    {
                        prop.SetValue(obj, DateTimeOffset.Now);
                    }
                    if (prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(obj, 11);
                    }
                }
                _data.Add(obj);
            }
        }

        public async Task<IActionResult> Get(HttpContext context)
        {
            await context.Response.WriteAsync(JObject.FromObject(_data).ToString());
            await context.Response.StartAsync();
            return new OkResult();
        }
    }
}
