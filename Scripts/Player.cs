using Godot;
using System;

public partial class Player : Character
{
	[Export] Label stateLabel; //temp for debugging

	public override void _Ready() {
		// CONSTANTS
		walkSpeed = 300.0f;
		dashSpeed = 1800.0f;
		dashTime = 0.2f;
		maxHealth = 10;

		// VARYING
		health = maxHealth;

		base._Ready();
	}

	/// <summary>performs dashing state actions</summary>
	protected override void updateDashingState(float dt) {
		Velocity = currentDirection * Mathf.Lerp(walkSpeed, dashSpeed, currentActionTimer/dashTime);

		currentActionTimer -= dt;
		if (currentActionTimer <= 0) {
			currentState = State.idle;
		}
	}
	/// <summary>performs casting state actions</summary>
	protected override void updateCastingState(float dt) {}


	public override void _PhysicsProcess(double delta)
	{
		float dt = (float) delta;
		
		// Update movement direction
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (canWalk()) {
			if (inputDirection == Vector2.Zero) {
				currentState = State.idle;
			}
			else {
				currentState = State.walking;
				currentDirection = inputDirection;
			}
		}

		if (canDash() && Input.IsActionJustPressed("Dash")) {
			currentState = State.dashing;
			currentActionTimer = dashTime;
		}

		performStateActions(dt);

		// update state label (for debugging)
		stateLabel.Text = currentState.ToString(); 

		MoveAndSlide();
	}
}
