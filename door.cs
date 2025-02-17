using Godot;
using System;

public partial class door : Area2D
{
	[Export] 
	public float InteractionRange = 200f;
    [Export] 
	public string NewScenePath;

	private bool _playerInRange = false;
    private Label _promptLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_promptLabel = GetNode<Label>("PromptLabel");
        _promptLabel.Visible = false; // Initially hidden
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
                _promptLabel.Visible = true; // Show the prompt
            }

            if (Input.IsActionJustPressed("ui_accept"))
            {
                OpenNewScene();
            }
        }
        else
        {
            if (_playerInRange)
            {
                _playerInRange = false;
                _promptLabel.Visible = false; // Hide the prompt
            }
        }
	}

	private void OpenNewScene()
    {
		Globals.previousScene = GetTree().CurrentScene.ToString();
		GetTree().ChangeSceneToFile(NewScenePath);
    }
}
