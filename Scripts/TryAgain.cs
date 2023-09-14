using Godot;
using System;

public partial class TryAgain : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Hide the mouse when the game starts
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnButtonDown()
	{
		//load the main scene
		GetTree().ChangeSceneToFile("res://Assets/Scenes/Main.tscn");
	}
}
