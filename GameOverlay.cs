using Godot;
using System;

public partial class GameOverlay : CanvasLayer
{
	public HBoxContainer heartContainer;

	[Export]
    public PackedScene fullHeartScene;
	[Export]
	public PackedScene emptyHeartScene;

	public override void _Ready()
	{
		heartContainer = GetNode<HBoxContainer>("Control/HBoxContainer");

		for (int i = 0; i < 10; i++)
		{
			if (i < Globals.playerHealth)
			{
				heartContainer.AddChild( (TextureRect)fullHeartScene.Instantiate() );
			}
			else
			{
				heartContainer.AddChild( (TextureRect)emptyHeartScene.Instantiate() );
			}
		}

		Player player = GetNode<Player>("../Player");
		// player.Connect("PlayerDamaged", this, "_OnPlayerDamaged");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void _on_player_player_damaged(int damage)
	{
		foreach (var heart in heartContainer.GetChildren())
		{
			heartContainer.RemoveChild(heart);
		}
		
		for (int i = 0; i < 10; i++)
		{
			if (i < Globals.playerHealth)
			{
				heartContainer.AddChild( (TextureRect)fullHeartScene.Instantiate() );
			}
			else
			{
				heartContainer.AddChild( (TextureRect)emptyHeartScene.Instantiate() );
			}
		}
	}
}
