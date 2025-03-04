using Godot;
using System;

public partial class LetterDiscoverable : Area2D
{
	public float InteractionRange = 200f;
	private bool IsLooted = false;

	[Export]
	PackedScene foundLetterScene;

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
            if (Input.IsActionJustPressed("ui_accept") && !IsLooted)
            {
				foundLetter foundLetterObj = (foundLetter)foundLetterScene.Instantiate();
				GetParent().AddChild(foundLetterObj);

				IsLooted = true;
            }
        }
	}
}
