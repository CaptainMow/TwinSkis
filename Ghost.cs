using Godot;
using System;

public partial class Ghost : CharacterBody2D
{
	Godot.Timer OrbCooldown;
	private bool OrbIsReady = true;

	Godot.Timer Damaged;
	private bool DamagedReady = false;
	private int TotalBlinks = 0;

	private int Health;

	[Export]
    public PackedScene OrbScene;
	private Node2D player;

	private Vector2 meanderAreaMin = new Vector2(800, 588);
    private Vector2 meanderAreaMax = new Vector2(1382, 417); 
	private Vector2 targetPosition;
	private RandomNumberGenerator rng = new ();
	private Area2D area;

	[Export]
    public PackedScene DialogueScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("../Player");

		OrbCooldown = GetNode<Godot.Timer>("OrbCooldown");
		OrbCooldown.Timeout += OnOrbCooldown;

		Damaged = GetNode<Godot.Timer>("Damaged");
		Damaged.Timeout += OnDamagedTimeout;

		Health = 100;
		
		targetPosition = new Vector2(
            rng.RandfRange(meanderAreaMin.X, meanderAreaMax.X),
            rng.RandfRange(meanderAreaMin.Y, meanderAreaMax.Y)
        );

		area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (OrbIsReady && Health > 0)
		{
			OrbIsReady = false;
			OrbCooldown.Start();
			
			Vector2 direction = (this.GlobalPosition - player.GlobalPosition).Normalized();

			// LookAt(player.GlobalPosition);
			LaunchOrb();
		}
		
		if (Health > 0)
		{
			Meander();
		}
		
		if (DamagedReady && Health > 0)
		{
			if (TotalBlinks < 4)
			{
				TotalBlinks++;
				DamagedReady = false;
				Visible = !Visible;
			}
		}

		if (DamagedReady && Health <= 0)
		{
			if (TotalBlinks < 8)
			{
				TotalBlinks++;
				DamagedReady = false;
				Visible = !Visible;
			}
			else 
			{
				QueueFree();

				GetNode<Area2D>("../Player").QueueFree();
				GetNode<CanvasLayer>("../GameOverlay").QueueFree();

				// ADD THE DIALOGUE OVERLAY SCENE
				dialogueOverlay newDialogue = (dialogueOverlay)DialogueScene.Instantiate();
				GetParent().AddChild(newDialogue);
			}
		}

	}

	public void OnOrbCooldown()
	{
		OrbIsReady = true;
	}

	public int GetHealth()
	{
		return Health;
	}

	public void TakeDamage(int damage)
	{
		TotalBlinks = 0;
		Damaged.Start();
		Health = Health - damage;
	}

	public void Die()
	{
		GD.Print("GHOST IS DEAD");
	}

	private void LaunchOrb()
	{
		orb newOrb = (orb)OrbScene.Instantiate();

		newOrb._position = GlobalPosition;

		Vector2 directionToPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
		newOrb._rotation = directionToPlayer.Angle();
		newOrb._direction = directionToPlayer.Angle();

		GetParent().AddChild(newOrb);
	}

	private void Meander()
	{
		// Move the enemy towards the target position
        Vector2 direction = targetPosition - Position;
        if (direction.Length() > 5f) 
        {
            direction = direction.Normalized();
			Velocity = direction * 50f;
            MoveAndSlide();
        }
        else
        {
			targetPosition = new Vector2(
				rng.RandfRange(meanderAreaMin.X, meanderAreaMax.X),
				rng.RandfRange(meanderAreaMin.Y, meanderAreaMax.Y)
			);
        }
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is sword)
		{
			GD.Print("hell yeah");
		}
	}

	// since the sword is an Area2D its getting picked up here not above 
	private void OnAreaEntered(Area2D area)
	{
		if (area is sword)
		{
			TakeDamage(20);
			GD.Print("Ghost HP: " + Health);
		}
	}

	private void OnDamagedTimeout()
	{
		DamagedReady = true;
	}
}
