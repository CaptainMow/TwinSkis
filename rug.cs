using Godot;
using System;

public partial class rug : Area2D
{
	public float InteractionRange = 200f;
	public bool IsRugMoved = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var player = GetNode<Player>("../Player"); // Adjust path to your player node
        float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
        
        if (distance <= InteractionRange && Input.IsActionJustPressed("ui_accept") && !IsRugMoved)
		{
			IsRugMoved = true;
			GetNode<Sprite2D>("Sprite2D").Visible = false;
		}
	}
}
