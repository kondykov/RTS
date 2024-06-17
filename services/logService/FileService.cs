using LoggerService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class FileService : IFIleService
    {
        private const string DefaultPath = "Log.txt";

        public async Task WriteLog(string message)
        {
            await using StreamWriter tw = new(DefaultPath, true, Encoding.Default);
            await tw.WriteLineAsync(message);
        }

        public async Task ReadLog()
        {
            using StreamReader sr = new StreamReader(FileService.DefaultPath);
            Console.WriteLine(await sr.ReadLineAsync());
        }

        public Task ClearLog() => throw new NotImplementedException();
    }
}