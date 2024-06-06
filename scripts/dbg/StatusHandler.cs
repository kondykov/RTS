using System.Collections.Generic;

namespace RTS.Debug
{
    public enum Status
    {
        OK,
        WARNING_RAYCAST_RETURNS_NULL,
        WARNING_CHUNKMANAGER_THREAD_INTERRUPTED,
        ERR_FAILED,
        ERR_FAILED_TO_GET_CAMERA_DATA,
        ERR_FAILED_TO_LOAD_DEBUG_MENU,
    }
    public static class StatusHandler
    {
        public static readonly Dictionary<Status, string> StatusNames = new Dictionary<Status, string>()
        {
            { Status.OK, "Ok." },
            { Status.ERR_FAILED, "Unhandled exception." },
            { Status.WARNING_RAYCAST_RETURNS_NULL, "Raycast returns null." }
        };
        public static string GetMessage(Status status)
        {
            if (StatusNames.ContainsKey(status)) return StatusNames[status];
            return $"{status}";
        }
    }
}
