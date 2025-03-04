using Godot;
using System;

public partial class trapdoor : Area2D
{
	[Export] 
	public string NewScenePath;

	private AnimatedSprite2D anim;
	public float InteractionRange = 100f;
	public bool IsOpen = false;
	private Label _promptLabel;

	//res://cabin.tscn

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_promptLabel = GetNode<Label>("PromptLabel");
		_promptLabel.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var player = GetNode<Player>("../Player"); // Adjust path to your player node
        float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
		
		if (IsOpen && distance <= InteractionRange)
		{
			_promptLabel.Visible = true;

			if (Input.IsActionJustPressed("ui_accept"))
			{
				OpenNewScene();
			}
		}
		else
		{
			_promptLabel.Visible = false;
		}

		var rug = GetNode<rug>("../Rug");

        if (distance <= InteractionRange && Input.IsActionJustPressed("ui_accept") && !IsOpen && rug.IsRugMoved)
		{
			GD.Print("in");
			IsOpen = true;
			anim.Play("open_door");
		}
	}

	private void OpenNewScene()
    {
		Globals.previousScene = GetTree().CurrentScene.ToString();
		GetTree().ChangeSceneToFile(NewScenePath);
    }
}
