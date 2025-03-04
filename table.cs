using Godot;
using System;

public partial class table : Area2D
{
	public float InteractionRange = 90f;
	public bool _playerInRange = false;
	public bool HasBeenMoved = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Check for the player's distance

        var player = GetNode<Player>("../Player"); // Adjust path to your player node
        float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
        
        if (distance <= InteractionRange)
        {
            if (!_playerInRange)
            {
                _playerInRange = true;
            }

            if (Input.IsActionJustPressed("ui_accept") && !HasBeenMoved)
            {
                HasBeenMoved = true;

				Position += new Vector2(0, -150);
            }
        }
	}
}
