using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using qldb_credentials;

namespace select_s3etag
{

        //Global variable for S3 Etag
        static class S3
        {
            public static string eTag;
        } 

    class SelectS3Etag
    {
        private static IAmazonS3 client;
        private const string bucketName = "inndoxqldb";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        public static async Task ReadObjectDataAsync()
        {
            //Create proper credentials for Amazon QLDB
            CredentialsQLDB credentialsQLDB = new CredentialsQLDB();
            var tempCredentials = credentialsQLDB.GetCredentials();
            try
            {
                // Create a client by providing temporary security credentials.
                using (client = new AmazonS3Client(tempCredentials, bucketRegion))
                {
                    Amazon.S3.Model.GetObjectRequest request = new Amazon.S3.Model.GetObjectRequest();
                    {
                        request.BucketName  = "inndoxqldb";
                        request.Key = "FolderP001/D001.txt";
                        
                        //Other request key samples
                        //request.Key = "FolderP001/";
                        //request.Key = "FolderP001/D001.txt";
                        //request.Key = "FolderP001/D002.docx";
                        //request.Key = "FolderP001/D003.xlsx";
                        //request.Key = "FolderP001/D004.pptx";
                    };

                    using (GetObjectResponse response = await client.GetObjectAsync(request))
                
                    using (Stream responseStream = response.ResponseStream)
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string etag = response.ETag;
                        var S3Etag = etag;
                        S3.eTag = etag;
                        Console.WriteLine($"  S3 Etag found given the folowing parameters: Bucketname = {request.BucketName}, Key={request.Key}");
                        Console.WriteLine($"  S3 Etag: {S3Etag}");
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                var S3Etag = "";
                S3.eTag = null;
                Console.WriteLine($"S3 Etag: {S3Etag}");
                Console.WriteLine($"S3 Etag not found");
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }            
        }
    }
}