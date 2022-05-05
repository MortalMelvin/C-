using System;
using System.IO;
using Amazon.QLDB.Driver;
using qldb_driver;
using Amazon.IonDotnet;
using Amazon.IonDotnet.Builders;
using Amazon.IonDotnet.Tree;
using select_table;

namespace update_table
{
    class UpdateTable
    {
        private const string queryPropertyId = "P001";
        public void UpdateDocument()
        {
            //Create driver for QLDB
            DriverQLDB driverQLDB = new DriverQLDB();
            var driver  = driverQLDB.GetDriver();

            //Get a document in the table
            SelectTable selectTable = new SelectTable();
            var selectResult = selectTable.GetDocument(queryPropertyId);
            Console.WriteLine(selectResult);
            if (selectResult != null)
            {
                Console.WriteLine($"PropertyId found!  {queryPropertyId}");

                // Updating a document
                string updateTableName = "Properties";
                IIonValue ionUpdatePropertyId = IonLoader.Default.Load("P001");
                IIonValue ionUpdateOwnerId = IonLoader.Default.Load("O002");
                Console.WriteLine("Updating a document");
                IIonValue updateResult = driver.Execute(txn =>
                {
                    IResult result = txn.Execute($"UPDATE {updateTableName} SET OwnerId = ? WHERE PropertyId = ?", ionUpdateOwnerId, ionUpdatePropertyId);
                    foreach (var row in result)
                    {
                        return row;
                    }
                    return null;
                });

                // Printing the id of the updated document
                using TextWriter twUpdate = new StringWriter();
                using IIonWriter writerUpdate = IonTextWriterBuilder.Build(twUpdate, new IonTextOptions { PrettyPrint = true });
                using IIonReader readerUpdate = IonReaderBuilder.Build(updateResult);
                writerUpdate.WriteValues(readerUpdate);
                writerUpdate.Finish();
                Console.WriteLine($"Document updated {twUpdate.ToString()}");   
            }

            else
            {
                Console.WriteLine($"PropertyId not found!  {queryPropertyId}");
                Console.WriteLine("Document cannot be updated");
            }

            return;
        }
    }
}