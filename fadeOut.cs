using Godot;
using System;

public partial class fadeOut : Node2D
{
	ColorRect c;
	Tween fadeTween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		c = GetNode<ColorRect>("ColorRect");
		fadeTween =  CreateTween();

		c.Color = new Color(0, 0, 0, 0);
		fadeTween.TweenProperty(c, "color", new Color(0, 0, 0, 1), 3);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
