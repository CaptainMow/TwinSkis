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
	private Area2D Sword;
	private Marker2D HandPosition;
	private float SwordDefaultRotation;
	private bool IsSwinging = false;
	private float returnTime = 0.5f;
    private double timeElapsed = 0.0;
	private AnimatedSprite2D anim;

	//hitboxes from sword
	private CollisionPolygon2D rightHitbox;
	private CollisionPolygon2D leftHitbox;


	// Define the signal
    [Signal]
    public delegate void PlayerDamagedEventHandler(int damage);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		// Shield = GetNode<Area2D>("Shield");
		// ShieldDefaultPosition = Shield.Position;
		// Shield.Visible = false;

		Sword = GetNode<Area2D>("Sword");
		HandPosition = GetNode<Marker2D>("HandPosition");
		SwordDefaultRotation = Sword.Rotation;
		anim = GetNode<AnimatedSprite2D>("Sword/AnimatedSprite2D");

		rightHitbox = GetNode<CollisionPolygon2D>("Sword/SwingRightHitbox");
		leftHitbox = GetNode<CollisionPolygon2D>("Sword/SwingLeftHitbox");
		rightHitbox.Disabled = true;
		leftHitbox.Disabled = true;

		anim.AnimationFinished += OnSwordAnimationFinished;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

		// if (Input.IsActionPressed("block"))
		// {
		// 	Shield.Position = direction.Normalized() * BlockRange;
		// }
		// else
		// {
		// 	Shield.Position = ShieldDefaultPosition;
		// }

		if (Input.IsActionPressed("basicAttack") && !IsSwinging)
		{
			IsSwinging = true;

			if (direction.X > 0)
			{
				anim.Play("slashRight");
				rightHitbox.Disabled = false;
			}
			else 
			{
				anim.Play("slashLeft");
				leftHitbox.Disabled=false;
			}
		}

		if (Globals.playerHealth <= 0)
		{
			Globals.previousScene = GetTree().CurrentScene.ToString();
			GetTree().ChangeSceneToFile("res://youDied.tscn");
		}
	}

	public void Start(Vector2 _position)
	{
		Position = _position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public void _on_body_entered(Node2D body)
	{
		if (body is orb)
		{
			TakeDamage(1);
		}
	}

	public void OnSwordAnimationFinished()
	{
		IsSwinging = false;
		rightHitbox.Disabled = true;
		leftHitbox.Disabled = true;
	}

	public int GetHealth()
	{
		return Globals.playerHealth;
	}

	public void TakeDamage(int damage)
	{
		Globals.playerHealth = Globals.playerHealth - damage;

		EmitSignal("PlayerDamaged", damage);
	}

	public void Die()
	{
		GD.Print("PLAYER IS DEAD");
	}
}
