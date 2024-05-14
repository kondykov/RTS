using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Debug
{
    enum Types
    {
        None,
        Error
    }
    public static class RTSMessageWriter
    {
        public static void WriteError(string error)
        {
            ChangeColor(Types.Error);
            Console.WriteLine(error);
            Console.ResetColor();
        }
        private static void ChangeColor(Types type)
        {
            Console.Write("RTS messager: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
    }
}
