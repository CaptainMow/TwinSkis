using Godot;
using System;

public partial class Cabin : Node2D
{
	public Vector2 ScreenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		var player = GetNode<Player>("Player");
	
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		var door = GetNode<Area2D>("Door");
		door.GetNode<ColorRect>("ColorRect").Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
