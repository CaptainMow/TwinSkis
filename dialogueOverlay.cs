using Godot;
using System;
using System.Collections.Generic;

public partial class dialogueOverlay : CanvasLayer
{
	Label label;

	TextureRect playerTexture;
	TextureRect ghostTexture;

	Godot.Timer timer;
	bool nextDialogue = false;
	int dialogueCount = 0;

	[Export]
	PackedScene fadeOutScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// default output
		label = GetNode<Label>("Control/Panel/Label");

		timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += OnTimerTimeout;

		ghostTexture = GetNode<TextureRect>("Control/GhostTexture");
		playerTexture = GetNode<TextureRect>("Control/DetectiveTexture");

		ghostTexture.Visible = false;

		label.Text = "You can rest now... your torment is over.";
		timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// ghost
		if (nextDialogue && dialogueCount == 1)
		{
			ghostTexture.Visible = true;
			playerTexture.Visible = false;

			label.Text = "...";
			nextDialogue = false;
			timer.Start();
		}
		else if (nextDialogue && dialogueCount == 2)
		{
			// GetTree().Quit();
			label.Text = "Thank you.";
			nextDialogue = false;
			timer.Start();
		}
		else if (nextDialogue && dialogueCount == 3)
		{
			fadeOut fadeOut = (fadeOut)fadeOutScene.Instantiate();
			GetParent().AddChild(fadeOut);

			QueueFree();
		}
	}

	private void OnTimerTimeout()
	{
		nextDialogue = true;
		dialogueCount++;
	}
}
