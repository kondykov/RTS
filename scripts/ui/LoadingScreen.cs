using Godot;
using System;
using LoggerService;
using RTS.Debug;
using Array = Godot.Collections.Array;

namespace RTS.UI;

public partial class LoadingScreen : Control
{
    [Export] private ProgressBar _bar;
    [Export] private Label _label;
    private Array _progress = [0];
    private string NextScene { get; set; }
    public static readonly string Loader = "res://prefabs/menus/Loading.tscn";

    public override void _Ready()
    {
        NextScene = MainCommand.SceneToLoad;
        MainCommand.SceneToLoad = null;
        _bar.Value = 100;
        _label.Text = "Loading scene";
        if (NextScene != null) ResourceLoader.LoadThreadedRequest(NextScene);
        else
        {
            Logger<LoadingScreen> logger = new(new FileService());
            logger.Log(LogStatus.Error, StatusHandler.GetMessage(Status.ErrFailedToLoadScene));
            Console.WriteLine(StatusHandler.GetMessage(Status.ErrFailedToLoadScene));
            GetTree().ChangeSceneToFile(MainCommand.PathToMainMenu);
        }
    }

    public override void _Process(double delta)
    {
        ResourceLoader.LoadThreadedGetStatus(NextScene, _progress);
        _bar.Value = (double)_progress[0] * 100;
        if ((double)_progress[0] != 1) return;
        var scene = (PackedScene)ResourceLoader.LoadThreadedGet(NextScene);
        GetTree().ChangeSceneToPacked(scene);
    }
}