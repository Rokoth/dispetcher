using StoUslug.Contract.Models;

namespace StoUslug.Client.Services
{
    public interface ISettingsDataService
    {
        Settings GetSettings();
    }
}