using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
    public int Speed { get; set; } = 400;
	public Vector2 ScreenSize;

	// combat
	[Export]
	public float BlockRange = 40f;
	private bool IsBlocking = false;
	private Vector2 MousePosition;
	private Area2D Shield;
	private Vector2 ShieldDefaultPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		Shield = GetNode<Area2D>("Shield");
		ShieldDefaultPosition = Shield.Position;
		// Shield.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// debug
		GD.Print("Player: " +  Position);
		GD.Print("Shield: " + Shield.Position);


		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("moveRight"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("moveLeft"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("moveDown"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("moveUp"))
		{
			velocity.Y -= 1;
		}

		var sprite2D = GetNode<Sprite2D>("Sprite2D");

		if (velocity.Length() > 0)
    	{
        	velocity = velocity.Normalized() * Speed;
		}

		Position += velocity * (float)delta;

		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		// get mouse position
		MousePosition = GetGlobalMousePosition();

		// eventually the player will rotate toward the mouse. for now just block that way
		Vector2 direction = MousePosition - GlobalPosition;

		if (Input.IsActionPressed("block"))
		{
			Shield.Position = direction.Normalized() * BlockRange;
		}
		else
		{
			Shield.Position = ShieldDefaultPosition;
		}
	}

	public void Start(Vector2 _position)
	{
		Position = _position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
