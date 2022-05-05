using System;
using create_table;
using insert_table;
using update_table;
using select_s3path;
using select_qldbetag;
using select_s3etag;
using Amazon;
using Amazon.S3;

namespace qldb_inndox
{
    
    class Program
    {

        private const string queryPropertyId = "P001";
        private const string queryDocumentId = "D001";
        private const string queryDocumentFileName = "D001";
        private const string queryDocumentUploadedBy = "UploaderOfDocument001";
        //Global variable for QLDB Etag
        static class QLDB
        {
            public static string eTag;
        } 
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello inndox QLDB!");
            
            //Create table and its index
            CreateTable createTable = new CreateTable();
            createTable.AddTable();

            //Add a document in the table
            InsertTable insertTable = new InsertTable();
            insertTable.AddDocument();

            //Update a document in the table
            UpdateTable updateTable = new UpdateTable();
            updateTable.UpdateDocument();
			
			//Get S3 Path of document in QLDB
			//Parameters: PropertyId, DocumentId, DocumentFileName, DocumentUploadedBy
            SelectS3Path selectS3Path = new SelectS3Path();
            var selectS3PathResult = selectS3Path.GetS3Path(queryPropertyId, queryDocumentId, queryDocumentFileName, queryDocumentUploadedBy);

            Console.WriteLine("\n");
            Console.WriteLine("Selecting S3 Path in QLDB");
            Console.WriteLine("=========================");
            if (selectS3PathResult != null)
            {
                Console.WriteLine($"S3 Document Path found given the folowing parameters: PropertyId = {queryPropertyId}, DocumentId = {queryDocumentId}, DocumentFileName = {queryDocumentFileName}, DocumentUploadedBy = {queryDocumentUploadedBy}");
                Console.WriteLine(selectS3PathResult);
                Console.WriteLine("\n");

            }
            else
            {
                Console.WriteLine($"S3 Document Path NOT found given the folowing parameters: PropertyId = {queryPropertyId}, DocumentId = {queryDocumentId}, DocumentFileName = {queryDocumentFileName}, DocumentUploadedBy = {queryDocumentUploadedBy}");
                Console.WriteLine("\n");

            }
			
			//Get Etag of document in QLDB
			//Parameter: PropertyId
            SelectQLDBEtag selectQLDBEtag = new SelectQLDBEtag();
            var selectQLDBEtagResult = selectQLDBEtag.GetQLDBEtag(queryPropertyId);

            if (selectQLDBEtagResult != null)
            {
                Console.WriteLine($"QLDB Etag found given the folowing parameter: PropertyId = {queryPropertyId}");
                var QLDBEtag = selectQLDBEtagResult;

                //format QLDB etag to be enclosed in double quotes
                QLDB.eTag = '"' + QLDBEtag + '"';

                Console.WriteLine($"QLDB Etag: {QLDB.eTag}");
            }
            else
            {
                Console.WriteLine($"QLDB Etag NOT found given the folowing parameter: PropertyId = {queryPropertyId}");
            }

            //Read all objects of S3 Buckets
            SelectS3Etag.ReadObjectDataAsync().Wait();

            //Process Etags of QLDB and S3
            Console.WriteLine("\n");
            Console.WriteLine("Processing of QLDB Etag and S3 Etag");
            Console.WriteLine("===================================");
            Console.WriteLine($"QLDB Etag:{QLDB.eTag}");
            Console.WriteLine($"  S3 Etag:{S3.eTag}");

            if (S3.eTag == null)
                Console.WriteLine("Etag not found in S3");
            else
            {
                if (QLDB.eTag == S3.eTag)
                    Console.WriteLine("Etag is valid");
                else
                    Console.WriteLine("Etag is invalid");
            }
            
            return;
        }
    }
}