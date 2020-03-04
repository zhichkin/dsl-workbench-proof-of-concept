namespace OneCSharp.WebAgent
{
    public interface IDeployService
    {
        string Deploy();
    }
    public class DeployService : IDeployService
    {
        public string Deploy()
        {
            return nameof(IDeployService);
        }
    }
}