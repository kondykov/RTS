using LoggerService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.services.logService
{
    public class FileService : IFIleService
    {
        public const string DEFAULT_PATH = "C:\\Godot\\Log.txt";
        public void CreateLogFile()
        {

        }
        async public Task Write(string message)
        {
            await using (FileStream f = File.OpenWrite(DEFAULT_PATH))
            {

            }
        }
    }
}
