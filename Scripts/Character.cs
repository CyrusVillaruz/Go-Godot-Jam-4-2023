using Godot;
using System;

public partial class Character : CharacterBody2D
{
    // possible player states
    protected enum State {idle, walking, dashing, casting} 

    // CONSTANTS
    [Export] Label healthLabel;
	protected float walkSpeed;
	protected float dashSpeed;
    protected float dashTime;
    protected float maxHealth;

    // VARYING
    protected Vector2 currentDirection = Vector2.Zero;
    protected State currentState = State.idle;
    protected float currentActionTimer = 0; // time remaining for current state (used by dashing, casting, etc)
    protected float health;

    public override void _Ready() {
        updateHealthLabel();
    }

    /// <summary>returns true if character is allowed to enter walking state</summary>
    protected virtual bool canWalk() {return currentState == State.idle || currentState == State.walking;}

    /// <summary>returns true if character is allowed to enter dashing state</summary>
    protected virtual bool canDash() {return currentState == State.idle || currentState == State.walking;}

    protected virtual bool canPrimaryAttack() {return currentState == State.idle || currentState == State.walking;} 

    /// <summary>performs idle state actions</summary>
    protected virtual void updateIdleState(float dt) {
        Velocity = Vector2.Zero;
    }
    /// <summary>performs walking state actions</summary>
    protected virtual void updateWalkingState(float dt) {
        Velocity = currentDirection * walkSpeed;
    }
    /// <summary>performs dashing state actions</summary>
    protected virtual void updateDashingState(float dt) {}
    /// <summary>performs casting state actions</summary>
    protected virtual void updateCastingState(float dt) {}

    protected void performStateActions(float dt) {
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
    }

    void updateHealthLabel() {
        healthLabel.Text = health + "/" + maxHealth + "HP";
    }

    void onDamaged(float damage) {
        health -= damage;
        if (health <= 0) {
            onDeath();
        }
        updateHealthLabel();
    }

    void onDeath() {
        OS.Alert("You died. Game Over");
    }

    
}
