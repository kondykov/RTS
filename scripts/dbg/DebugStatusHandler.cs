using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Debug
{
    public enum FunctionStatus
    {
        OK,
        FAILED,
        WARNING_WORLD_CAMERA_RAYCAST_RETURNS_NULL,
        ERR_FAILED_TO_GET_CAMERA_DATA
    }
    public static class StatusHandler
    {
        public static readonly Dictionary<FunctionStatus, string> StatusNamesEn = new Dictionary<FunctionStatus, string>()
        {
            { FunctionStatus.OK, "Ok." },
            { FunctionStatus.FAILED, "Generic error." },
            { FunctionStatus.WARNING_WORLD_CAMERA_RAYCAST_RETURNS_NULL, "Raycast returns null." }
        };
        public static readonly Dictionary<FunctionStatus, string> StatusNamesRu = new Dictionary<FunctionStatus, string>()
        {
            { FunctionStatus.OK, "Успешно." },
            { FunctionStatus.FAILED, "Провал." },
            { FunctionStatus.WARNING_WORLD_CAMERA_RAYCAST_RETURNS_NULL, "Проброс луча вернул значение null." }
        };
        public static string GetMessage(FunctionStatus status)
        {
            if (StatusNamesRu.ContainsKey(status)) return StatusNamesRu[status];
            return $"{status}";
        }
        public static string GetMessage<Ru>(FunctionStatus status)
        {
            if (StatusNamesRu.ContainsKey(status)) return StatusNamesRu[status];
            return $"{status}";
        }
    }
}
