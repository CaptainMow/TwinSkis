using Godot;
using System;

public partial class orb : CharacterBody2D
{
	public Vector2 _position;
	public float _rotation;
	public float _direction;
	float speed = 800f;
	float damage = 40;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalPosition = _position;
		GlobalRotation = _rotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Velocity = new Vector2(speed, 0).Rotated(_direction);
		MoveAndSlide();
	}
}
