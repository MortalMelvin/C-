using System;
using System.IO;
using Amazon.QLDB.Driver;
using qldb_driver;
using Amazon.IonDotnet;
using Amazon.IonDotnet.Builders;
using Amazon.IonDotnet.Tree;

namespace select_s3path
{

    class SelectS3Path
    {
        public TextWriter GetS3Path(string queryPropertyId, string queryDocumentId, string queryDocumentFileName, string queryDocumentUploadedBy)

        {
            if (queryPropertyId is null)
            {
                throw new ArgumentNullException(nameof(queryPropertyId));
            }
            //Create driver for QLDB
            DriverQLDB driverQLDB = new DriverQLDB();
            var driver  = driverQLDB.GetDriver();
            Console.WriteLine("Querying a table");
            var count=0;
            IIonValue selectResult = driver.Execute(txn =>
            {
                // Selecting a document
                IIonValue ionSelectPropertyId = IonLoader.Default.Load(queryPropertyId);
                IIonValue ionSelectDocumentId = IonLoader.Default.Load(queryDocumentId);
				IIonValue ionSelectDocumentFileName = IonLoader.Default.Load(queryDocumentFileName);
                IIonValue ionSelectDocumentUploadedBy = IonLoader.Default.Load(queryDocumentUploadedBy);

                
                IResult result = txn.Execute("SELECT DocumentPath  FROM Properties WHERE PropertyId = ? AND DocumentId = ? AND DocumentFileName = ? AND DocumentUploadedBy  = ?", ionSelectPropertyId, ionSelectDocumentId, ionSelectDocumentFileName, ionSelectDocumentUploadedBy);
                foreach (var row in result)
                {
                    count ++;
                    return row;
                }
                return null;
            });
            Console.WriteLine($"count: {count}");
            
            if (count > 0)
            {
                Console.WriteLine($"S3 Document Path found: {selectResult.GetField("DocumentPath").StringValue}");
                
                // Printing the id of the selected document
                using TextWriter twSelect = new StringWriter();
                using IIonWriter writerSelect = IonTextWriterBuilder.Build(twSelect, new IonTextOptions { PrettyPrint = true });
                using IIonReader readerSelect = IonReaderBuilder.Build(selectResult);
                writerSelect.WriteValues(readerSelect);
                writerSelect.Finish();
                Console.WriteLine($"Document selected {twSelect.ToString()}");
                
                return twSelect;
            }
            else
            {
                return null;
            }
        }
    }
}