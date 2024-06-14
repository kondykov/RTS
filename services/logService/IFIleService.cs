using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public interface IFIleService
    {
        public Task WriteLog(string message);
        public Task ReadLog();
        public Task ClearLog();
    }
}