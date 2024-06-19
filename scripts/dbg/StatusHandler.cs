using System.Collections.Generic;

namespace RTS.Debug;

public enum Status
{
    Ok,
    WarningRaycastReturnsNull,
    WarningChunkmanagerThreadInterrupted,
    ErrFailed,
    ErrFailedToGetCameraData,
    ErrFailedToLoadDebugMenu,
    ErrFailedToLoadScene,
}

public static class StatusHandler
{
    private static readonly Dictionary<Status, string> StatusNames = new Dictionary<Status, string>()
    {
        { Status.Ok, "Ok." },
        { Status.ErrFailed, "Unhandled exception." },
        { Status.WarningRaycastReturnsNull, "Raycast returns null." }
    };

    public static string GetMessage(Status status) =>
        StatusNames.TryGetValue(status, out var value) ? value : $"{status}";
}