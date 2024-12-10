using StoUslug.Contract.Models;

namespace StoUslug.Client.Services
{
    public class AuthDataService
    {
        public AuthDataService()
        {

        }

        public ClientIdentityResponse Auth(UserIdentity identity)
        {
            return new ClientIdentityResponse();
        }
    }
}
