using System;
using System.Threading;
using qldb_driver;

namespace create_table
{
    class CreateTable
    {
        public void AddTable()
        {

            //Create driver for QLDB
            DriverQLDB driverQLDB = new DriverQLDB();
            var driver  = driverQLDB.GetDriver();

			// Creates the table and the index in the same transaction
            Console.WriteLine("Creating the tables");
			string tableName = "Properties";
			string tableIndex = "PropertyId";
            driver.Execute(txn =>
            {
                txn.Execute($"CREATE TABLE {tableName}");
                txn.Execute($"CREATE INDEX ON {tableName}({tableIndex})");
            });
			
			Console.WriteLine($"Table '{tableName}' created");
			Console.WriteLine($"Table index '{tableIndex}' created");

            Thread.Sleep(2000);
            return;
        }
    }
}