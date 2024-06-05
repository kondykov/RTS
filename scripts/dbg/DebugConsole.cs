using System;

namespace RTS.Debug
{
    enum TypeofStatus
    {
        SUCCESS,
        WARNING,
        ERROR,
        DEFAULT
    }
    public class DebugConsole
    {
        public static void WriteMessage(FunctionStatus error)
        {
            ChangeColor(error);
            Console.WriteLine(StatusHandler.GetMessage(error));
            Console.ResetColor();
        }
        public static void WriteMessage<Ru>(FunctionStatus error)
        {
            ChangeColor(error);
            Console.WriteLine(StatusHandler.GetMessage<Ru>(error));
            Console.ResetColor();
        }
        private static void ChangeColor(FunctionStatus type)
        {
            string tmp = type.ToString();
            TypeofStatus selector = TypeofStatus.DEFAULT;
            if (tmp.Contains("ERR")) selector = TypeofStatus.ERROR;
            if (tmp.Contains("WARNING")) selector = TypeofStatus.WARNING;
            if (tmp.Contains("SUCCESS")) selector = TypeofStatus.SUCCESS;

            switch (selector)
            {
                case TypeofStatus.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERR: ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case TypeofStatus.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARNING: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case TypeofStatus.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
        }
    }
}
