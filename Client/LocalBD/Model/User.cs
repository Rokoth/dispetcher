using System;
using System.Collections.Generic;

namespace LocalBD.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual IEnumerable<Server> Servers { get; set; }
    }

    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class AddServerRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public User User { get; set; }
        public bool IsAuth { get; set; }
        public string Error { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }

    public enum ErrorCode
    {
        OK = 0,
        WrongPassword = 1,
        WrongLogin = 2,
        EmptyPassword = 3,
        EmptyLogin = 4,
        TooManyTries = 5
    }
}
