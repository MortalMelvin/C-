using System;
using Amazon.QLDB.Driver;
using qldb_configuration;
using qldb_credentials;

namespace qldb_driver
{
    class DriverQLDB
    {
        public IQldbDriver GetDriver()
        {

            //Create proper credentials for Amazon QLDB
            CredentialsQLDB credentialsQLDB = new CredentialsQLDB();
            var awsCredentials = credentialsQLDB.GetCredentials();

            //Create proper configuration for Amazon QLDB
            ConfigurationQLDB configurationQLDB = new ConfigurationQLDB();
            var amazonQldbSessionConfig = configurationQLDB.GetConfiguration();

            //Specify the ledger name inside Amazon QLDB
            var ledgerName = "qldb-inndox";

            //Create driver for Amazon QLDB
            Console.WriteLine("Create the inndox QLDB Driver");

            IQldbDriver driver = QldbDriver.Builder()
                .WithAWSCredentials(awsCredentials)
                .WithQLDBSessionConfig(amazonQldbSessionConfig)
                .WithLedger(ledgerName)
                .Build();
            return driver;
        }
    }
}