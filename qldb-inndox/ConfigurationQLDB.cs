using Amazon.QLDBSession;

namespace qldb_configuration
{
    class ConfigurationQLDB
    {
        public AmazonQLDBSessionConfig GetConfiguration()
        {
            AmazonQLDBSessionConfig amazonQldbSessionConfig = new AmazonQLDBSessionConfig();
            amazonQldbSessionConfig.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName("ap-southeast-2");
            return amazonQldbSessionConfig;
        }
    }
}