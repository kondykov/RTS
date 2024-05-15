using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Debug
{
    enum Types
    {
        SUCCESS,
        WARNING,
        ERROR,
        DEFAULT
    }

    public enum Status
    {
        OK,
        FAILED,
        WARNING_RAYCAST_RETURNS_NULL
    }
    public static class StatusHandler
    {
        public static readonly Dictionary<Status, string> StatusNames = new Dictionary<Status, string>()
        {
            { Status.OK, "Ok." },
            { Status.FAILED, "Generic error" },
            { Status.WARNING_RAYCAST_RETURNS_NULL, "Raycast returns null." }
        };
        public static string GetMessage(Status status)
        {
            if (StatusNames.ContainsKey(status)) return StatusNames[status];
            return $"{status}";
        }
    }
    public static class Messager
    {
        public static void WriteMessage(Status error)
        {
            ChangeColor(error);
            Console.WriteLine(StatusHandler.GetMessage(error));
            Console.ResetColor();
        }
        private static void ChangeColor(Status type)
        {
            string tmp = type.ToString();
            Types selector = Types.DEFAULT;
            if (tmp.Contains("ERR")) selector = Types.ERROR;
            if (tmp.Contains("WARNING")) selector = Types.WARNING;
            if (tmp.Contains("SUCCESS")) selector = Types.SUCCESS;

            switch (selector)
            {
                case Types.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERR: ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case Types.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARNING: ");
                    break;
                case Types.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
        }
    }
}
