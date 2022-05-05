using Amazon.Runtime;

namespace qldb_credentials
{
    class CredentialsQLDB
    {
        public BasicAWSCredentials GetCredentials()
        {
            var awsCredentials = new BasicAWSCredentials("AKIAYC6PUR3YHIP44DYE","Ik1WIo9LFFbTYPe689GOIQDgXIXWljjPX2KPY2cN");
            return awsCredentials;
        }
    }
}