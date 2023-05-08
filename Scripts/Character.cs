using Godot;
using System;

public partial class Character : CharacterBody2D
{
    // possible player states
    protected enum State {idle, walking, dashing, casting, knockback} 

    // CONSTANTS
    [Export] Label healthLabel;
	protected float walkSpeed;
	protected float dashSpeed;
    protected float dashDuration;
    protected float maxHealth;

    // VARYING
    [Export] protected Node2D primaryWeapon;
    protected Vector2 currentDirection = Vector2.Zero;
    protected State currentState = State.idle;
    protected float currentStateTimer = 0; // time remaining for current state (used by dashing, casting, etc)
    protected float health;
    protected Vector2 currentStateDashDirection = Vector2.Zero;
    protected float currentStateDashSpeed = 0;
    protected float currentStateDuration;

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

    protected virtual void updateKnockbackState(float dt) {
        Velocity = currentStateDashDirection * Mathf.Lerp(walkSpeed, currentStateDashSpeed, currentStateTimer/currentStateDuration);

        currentStateTimer -= dt;
        if (currentStateTimer <= 0) {
            currentState = State.idle;
        }
    }

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
            case State.knockback:
                updateKnockbackState(dt);
                break;
        }
    }

    void updateHealthLabel() {
        healthLabel.Text = health + "/" + maxHealth + "HP";
    }

    void onHit(float damage, Vector2 knockbackDirection, float knockbackSpeed=0, float knockbackDuration=0) {
        health -= damage;
        if (health <= 0) {
            onDeath();
        }
        updateHealthLabel();

        if (knockbackDuration != 0) {
            currentState = State.knockback;
            currentStateTimer = knockbackDuration;
            currentStateDuration = knockbackDuration;
            currentStateDashSpeed = knockbackSpeed;
            currentStateDashDirection = knockbackDirection;
        }
    }

    protected virtual void onDeath() {
        QueueFree();
    }

    
}
