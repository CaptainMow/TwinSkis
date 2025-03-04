using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public partial class chest : Area2D
{
	[Export]
	public string ChestContents; 
	[Export]
	public bool IsLocked;
	public float InteractionRange = 200f;
	private Label _promptLabel;
	private bool _playerInRange = false;
	private PanelContainer ChestPanelContainer;
	bool SwitchingItems = false;
	Godot.Timer timer;
	bool IsLooted = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Godot.Timer>("Timer");
		timer.Timeout += OnTimerTimeout;

		ChestPanelContainer = GetNode<PanelContainer>("ChestPanelContainer");
		ChestPanelContainer.Visible = false;
		DisplayChestContents();

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

            if (Input.IsActionJustPressed("ui_accept") && !IsLooted)
            {
                ChestPanelContainer.Visible = true;

				foreach (Panel child in ChestPanelContainer.GetChildren())
				{
					child.Visible = true;
					
					timer.Start();
				}

				IsLooted = true;
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

	public void DisplayChestContents()
	{
		Panel panel = new Panel();
		panel.DrawRect(new Rect2(new Vector2(200, 50), new Vector2(200, 50)), Colors.Gray);
		panel.Size = new Vector2(200, 50);

		ChestPanelContainer.AddChild(panel);

		Label label = new Label();
		label.Text = ChestContents;
		label.SetSize(new Vector2(200, 50));

		panel.AddChild(label);
		panel.Visible = false;
		ChestPanelContainer.AddChild(panel);
	}

	private void OnTimerTimeout()
	{
		SwitchingItems = true;

		foreach (Panel child in ChestPanelContainer.GetChildren())
		{
			child.Visible = false;
			ChestPanelContainer.Visible = false;
		}
	}
}
