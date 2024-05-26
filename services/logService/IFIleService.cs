using RTS.services.logService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public interface IFIleService
    {
        public void CreateLogFile();
        public Task Write(string message);
    }
}
