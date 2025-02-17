using Godot;
using System;

public partial class mainScene : Node2D
{
	public Vector2 ScreenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		var player = GetNode<Player>("Player");

		if (Globals.previousScene == "")
		{
			var startPosition = GetNode<Marker2D>("StartPosition");
			startPosition.Position = new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2);
			player.Start(startPosition.Position);
		}
		else
		{
			var doorPosition = GetNode<Marker2D>("DoorPosition");
			player.Start(doorPosition.Position);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
 