using Microsoft.Extensions.Configuration;
using Nomnio.TableCopyLibrary;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
namespace ConsoleApp1
{
    class Program
    {
        

        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{SourceContext}] [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.RollingFile("log-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{SourceContext}] [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            
            Configuration = builder.Build();
            var copy = new DynamicTableCopy();
            Task complete;
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
                string sourceConnectionString = Configuration["sourceConnectionString"];
                string sourceTableName = Configuration["sourceTableName"];
                string targetConnectionString = Configuration["targetConnectionString"];
                string targetTableName = Configuration["targetTableName"];
                int batchSize= int.Parse(Configuration["batchSize"]);
                complete = copy.CopyAsync(sourceConnectionString, sourceTableName, targetConnectionString, targetTableName, batchSize);
            }
            Task.WaitAll(complete);
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
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