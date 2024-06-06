using Godot;

namespace RTS.UI;

public partial class MouseModeHandler : Node
{
    public static void ChangeCaptureMouseMode()
    {
        Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
            ? Input.MouseModeEnum.Visible
            : Input.MouseModeEnum.Captured;
    }

    public static void CaptureMouse() => Input.MouseMode = Input.MouseModeEnum.Captured;
    public static void VisibleMouse() => Input.MouseMode = Input.MouseModeEnum.Visible;
}