using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] Label stateLabel; //temp for debugging

    // possible player states
    enum State {idle, walking, dashing, casting} 

	const float walkSpeed = 300.0f;
	const float dashSpeed = 1800.0f;
    const float dashTime = 0.2f;

    Vector2 currentDirection = Vector2.Zero;
    State currentState = State.idle;
    float currentActionTimer = 0; // time remaining for current state (used by dashing, casting, etc)
    
    /// <summary>returns true if character is allowed to enter walking state</summary>
    bool canWalk() {return currentState == State.idle || currentState == State.walking;}

    /// <summary>returns true if character is allowed to enter dashing state</summary>
    bool canDash() {return currentState == State.idle || currentState == State.walking;}

    /// <summary>performs idle state actions</summary>
    void updateIdleState(float dt) {
        Velocity = Vector2.Zero;
    }
    /// <summary>performs walking state actions</summary>
    void updateWalkingState(float dt) {
        Velocity = currentDirection * walkSpeed;
    }
    /// <summary>performs dashing state actions</summary>
    void updateDashingState(float dt) {
        Velocity = currentDirection * Mathf.Lerp(walkSpeed, dashSpeed, currentActionTimer/dashTime);

        currentActionTimer -= dt;
        if (currentActionTimer <= 0) {
            currentState = State.idle;
        }
    }
    /// <summary>performs casting state actions</summary>
    void updateCastingState(float dt) {}


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

        // perform current state actions
        switch (currentState) {
            case State.idle:
                updateIdleState(dt);
                break;
            case State.walking:
                updateWalkingState(dt);
                break;
            case State.dashing:
                updateDashingState(dt);
                break;
            case State.casting:
                updateCastingState(dt);
                break;
        }
        // update state label (for debugging)
        stateLabel.Text = currentState.ToString(); 

		MoveAndSlide();
	}
}
