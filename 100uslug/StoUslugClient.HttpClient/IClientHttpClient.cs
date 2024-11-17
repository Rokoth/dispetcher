using StoUslug.Contract.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoUslugClient.ClientHttpClient
{
    public interface IClientHttpClient
    {
        bool IsConnected { get; }

        event EventHandler OnConnect;

        Task<bool> Auth(UserIdentity identity);
        Task<(int, IEnumerable<T>)> Get<T>(string param, Type apiType = null) where T : class;
        Task<T> Get<T>(Guid id) where T : class;

        Task<T> PostAsync<T>(T request) where T : class;

        void ConnectToServer(string server, Action<bool, bool, string> onResult);
        void Dispose();
       
    }
}
