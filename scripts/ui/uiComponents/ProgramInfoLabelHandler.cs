using Godot;

namespace RTS.UI;

[Tool]
public partial class ProgramInfoLabelHandler : Label
{
    public static string ProgramName { get; } = "Golden fever";
    public static string ProgramVersion { get; } = "Indev 1.0.0";
    private Label Label { get; set; }

    private static string GetInfo()
    {
        return $"{ProgramName}. {ProgramVersion}";
    }
    public override void _Ready()
    {
        Label = this;
        Label.Text = GetInfo();
    }
}
