using Godot;
using System;

public partial class Player : Character
{
    [Export] Label stateLabel; //temp for debugging
    [Export] Node2D raft;


    Vector2 lookDirection;
    bool buildMode = false;

    public override void _Ready() {
        // CONSTANTS
        walkSpeed = 300.0f;
        dashSpeed = 1800.0f;
        dashDuration = 0.2f;
        maxHealth = 10;

		// VARYING
		health = maxHealth;

		base._Ready();
	}


    /// <summary>performs dashing state actions</summary>
    protected override void updateDashingState(float dt) {
        Velocity = currentDirection * Mathf.Lerp(walkSpeed, dashSpeed, currentStateTimer/dashDuration);

        currentStateTimer -= dt;
        if (currentStateTimer <= 0) {
            currentState = State.idle;
        }
    }
    /// <summary>performs casting state actions</summary>
    protected override void updateCastingState(float dt) {
        Velocity = lookDirection * Mathf.Lerp(walkSpeed, currentStateDashSpeed, currentStateTimer/currentStateDuration);
        
        currentStateTimer -= dt;
        if (currentStateTimer <= 0) {
            currentState = State.idle;
        }
    }



	public override void _PhysicsProcess(double delta)
	{

        float dt = (float) delta;
        
        // GD.Print(Position);

        if (currentState != State.casting) {
            lookDirection = (GetGlobalMousePosition() - Position).Normalized();
            primaryWeapon.Rotation = lookDirection.Angle() + Mathf.Pi/2;
        }

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
            currentStateTimer = dashDuration;
        }

        if (canPrimaryAttack() && Input.IsActionJustPressed("Attack")) {
            primaryWeapon.Call("StartAttack");
            currentState = State.casting;
            currentStateDuration = (float) primaryWeapon.Get("animationTime");
            currentStateTimer = currentStateDuration;
            currentStateDashSpeed = (float) primaryWeapon.Get("attackDashSpeed");
        }

        if (Input.IsActionJustPressed("ToggleBuild")) {
            raft.Call("ToggleBuild");
            buildMode = !buildMode;
        }

        if (buildMode && Input.IsActionJustPressed("PlaceTile")) {
            raft.Call("PlaceTile");
        }

        performStateActions(dt);

		// update state label (for debugging)
		stateLabel.Text = currentState.ToString(); 

		MoveAndSlide();
	}
}
