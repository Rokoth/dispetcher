using StoUslug.Contract.Models;

namespace StoUslug.Client.Services
{
    public class SettingsDataService : ISettingsDataService
    {
        public SettingsDataService()
        {

        }

        public Settings GetSettings()
        {
            return new Settings();
        }
    }
}
