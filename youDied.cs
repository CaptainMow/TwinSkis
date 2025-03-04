using Godot;
using System;

public partial class youDied : Node2D
{
	VideoStreamPlayer vsp;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vsp = GetNode<VideoStreamPlayer>("VideoStreamPlayer");
		vsp.Finished += OnVideoFinished;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnVideoFinished()
	{
		GetTree().Quit();
	}
}
