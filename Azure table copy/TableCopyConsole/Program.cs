using System;
using System.Threading.Tasks;
using TableCopyLibrary;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var copy = new TableCopy();
            Task<bool> complete;
            if (args.Length == 5)
            {
                int batchSize;
                if (!int.TryParse(args[4],out batchSize))
                {
                    batchSize = GetInt("Please enter batch size");
                }
                complete = copy.CopyAsync(args[0], args[1], args[2], args[3], batchSize);
            }
            else
            {
                string sourceConnectionString = GetString("Please enter source connection string");
                string sourceTableName = GetString("Please enter source table name");
                string targetConnectionString = GetString("Please enter target connection string");
                string targetTableName = GetString("Please enter target table name");
                int batchSize=GetInt("Please enter batch size");
                complete = copy.CopyAsync(sourceConnectionString, sourceTableName, targetConnectionString, targetTableName, batchSize);
            }
            Task.WaitAll(complete);
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        static string GetString(string promptMessage)
        {
                Console.WriteLine(promptMessage);
                return Console.ReadLine();
        }

        static int GetInt(string promptMessage)
        {
            int testInt;
            while (true)
            {
                Console.WriteLine(promptMessage);
                string intTest = Console.ReadLine();

                if (int.TryParse(intTest, out testInt))
                {
                    break;
                }
            }
            return testInt;
        }
    }
}