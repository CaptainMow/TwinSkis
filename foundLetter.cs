using Godot;
using System;

public partial class foundLetter : CanvasLayer
{
	Godot.Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
		timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
