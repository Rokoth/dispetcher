using System.Threading.Tasks;

namespace StoUslug.DeployerService
{
    public interface IDeployService
    {
        Task Deploy(int? num = null);
    }
}
