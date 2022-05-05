using System;
using System.Text;
using System.IO;
using Amazon.QLDB.Driver;
using qldb_driver;
using Newtonsoft.Json;
using Amazon.IonDotnet;
using Amazon.IonDotnet.Builders;
using Amazon.IonDotnet.Tree;
using properties_data;
using select_table;

namespace insert_table
{

    class InsertTable
    {
        private const string queryPropertyId = "P001";
        public void AddDocument()
        {
            //Create driver for QLDB
            DriverQLDB driverQLDB = new DriverQLDB();
            var driver  = driverQLDB.GetDriver();

            //Get a document in the table
            SelectTable selectTable = new SelectTable();
            var selectResult = selectTable.GetDocument(queryPropertyId);
            Console.WriteLine(selectResult);
            if (selectResult == null)
            {
                Console.WriteLine($"PropertyId not found!  {queryPropertyId}");
            
                // Inserting Properties Data 			
                string insertTableName = "Properties";
                var objsample = new PropertiesData() { PropertyId = "P001", OwnerId = "O001", CompanyId = "C001", HandoverDate = "2020-10-10T12:02:43.804Z", DocumentId = "D001", DocumentFileName = "D001", DocumentUploadedBy  = "UploaderOfDocument001", DocumentPath = "FolderP001/D001.txt", DocumentMetadata = "d5752f16bc6be903f3d9358a6bc2c0bf"};

                string jsonresult= JsonConvert.SerializeObject(objsample,Formatting.Indented);
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                using (JsonTextWriter jtwriter = new JsonTextWriter(sw))
                {
                    jtwriter.QuoteChar = '\'';

                    JsonSerializer ser = new JsonSerializer();
                    ser.Serialize(jtwriter, objsample);
                }

                Console.WriteLine(sb.ToString());

                Console.WriteLine("Inserting a document");

                var text = $"INSERT INTO {insertTableName} VALUE" + sb;
                Console.WriteLine(text);

                IIonValue insertResult = driver.Execute(txn =>
                {
                    IResult result = txn.Execute($"INSERT INTO {insertTableName} VALUE "+sb);
                    foreach (var row in result)
                    {
                        return row;
                    }
                    return null;
                });


                // Printing the id of the inserted document
                using TextWriter twInsert = new StringWriter();
                using IIonWriter writerInsert = IonTextWriterBuilder.Build(twInsert, new IonTextOptions { PrettyPrint = true });
                using IIonReader readerInsert = IonReaderBuilder.Build(insertResult);
                writerInsert.WriteValues(readerInsert);
                writerInsert.Finish();
                Console.WriteLine($"Document inserted {twInsert.ToString()}");
            }

            else
            {
                Console.WriteLine($"PropertyId found!  {queryPropertyId}");
                Console.WriteLine("Document cannot be inserted");
            }
            return;
        }
    }
}