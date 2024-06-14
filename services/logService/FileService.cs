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
        public const string DEFAULT_PATH = "C:\\Godot\\Log.txt";

        async public Task WriteLog(string message)
        {
            using (StreamWriter tw = new(DEFAULT_PATH, true, Encoding.Default))
            {
                await tw.WriteLineAsync(message);
            }
        }

        async public Task ReadLog()
        {
            using (StreamReader sr = new StreamReader(FileService.DEFAULT_PATH))
            {
                Console.WriteLine(await sr.ReadLineAsync());
            }
        }

        public Task ClearLog()
        {
            throw new NotImplementedException();
        }
    }
}