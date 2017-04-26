using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using Nomnio.TableCopyLibrary;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BenchmarkTableCopyConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var summary=BenchmarkRunner.Run<BenchmarkTableCopy>();

            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
    }
}