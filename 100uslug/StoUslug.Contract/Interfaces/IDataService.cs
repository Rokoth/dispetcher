using StoUslug.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoUslug.Contract.Interfaces
{
    public interface IDataService<T> where T: Entity
    {
        Task<T> GetItem(Guid id);
        Task<IEnumerable<T>> GetList(IFilter<T> filter);
    }

    public interface IDataService
    {
        public Task<T> GetItem<T>(Guid id);
    }
}
