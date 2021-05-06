using LocalBD.Model;
using System;
using System.Collections.Generic;

namespace LocalBD
{
    public static class Auth
    {
        private static readonly Dictionary<int, bool> _authUsers = new Dictionary<int, bool>();
        private static System.Data.SQLite.EF6.SQLiteProviderFactory _factory;

        public static AuthResponse AuthRequest(AuthRequest authRequest)
        {
            if (authRequest == null || authRequest.Login == null || authRequest.Password == null)
            {
                return new AuthResponse()
                {
                    IsAuth = false,
                    Error = "Wrong auth request",
                    ErrorCode = ((authRequest == null || authRequest.Login == null) ? ErrorCode.EmptyLogin : ErrorCode.EmptyPassword)
                };
            }
            else
            {
                AuthResponse response = new AuthResponse()
                {
                    Error = "",
                    ErrorCode = 0,
                    IsAuth = true,
                    User = new User()
                    {
                        Id = 0,
                        Login = "development",
                        Name = "development",
                        Password = "test",
                        Servers = new List<Server>()
                        {
                             new Server() {
                                 Id = 0, Password = "test", Login = "test", Name = "test", Url = new Uri("http://localhost")
                             }
                        }
                    }
                };
                _factory = new System.Data.SQLite.EF6.SQLiteProviderFactory();
                using System.Data.Common.DbConnection _connection = _factory.CreateConnection();
                _connection.ConnectionString = "";
                _connection.Open();
                using System.Data.Common.DbCommand _command = _connection.CreateCommand();
                System.Data.Common.DbParameter paramLogin = _command.CreateParameter();
                paramLogin.ParameterName = "login";
                paramLogin.Value = authRequest.Login;
                _command.Parameters.Add(paramLogin);
                System.Data.Common.DbParameter paramPass = _command.CreateParameter();
                paramPass.ParameterName = "password";
                paramPass.Value = authRequest.Password;
                _command.Parameters.Add(paramPass);

                _command.CommandText = $"select id, name from user where login = @login and password = @password;";


                return response;
            }
        }
    }
}
