using System;
using System.IO;
using Amazon.QLDB.Driver;
using qldb_driver;
using Amazon.IonDotnet;
using Amazon.IonDotnet.Builders;
using Amazon.IonDotnet.Tree;

namespace select_table
{
    class SelectTable
    {
        public TextWriter GetDocument(string queryPropertyId)
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
                
                IResult result = txn.Execute("SELECT PropertyId, OwnerId, CompanyId, HandoverDate, DocumentId, DocumentFileName, DocumentUploadedBy, DocumentPath, DocumentMetadata  FROM Properties WHERE PropertyId = ?", ionSelectPropertyId);
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
                Console.WriteLine($"Property found {selectResult.GetField("PropertyId").StringValue}");
                
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