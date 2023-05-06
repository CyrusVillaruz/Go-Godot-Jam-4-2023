using Godot;
using System;

public partial class Enemy : Character
{
    [Export] CharacterBody2D PlayerBody;

    float primaryAttackRange = 50;

    public override void _Ready() {
        // CONSTANTS
        //PlayerBody = GetNode<CharacterBody2D>("/root/Player");
        walkSpeed = 200.0f;
        dashSpeed = 1800.0f;
        dashTime = 0.2f;
        maxHealth = 10;

        // VARYING
        health = maxHealth;

        base._Ready();
    }


	public override void _PhysicsProcess(double delta)
	{
        float dt = (float)delta;

        float playerDistance = (PlayerBody.Position - Position).Length();
		Vector2 playerDirection = (PlayerBody.Position - Position).Normalized();

        if (canWalk()) {
            currentState = State.walking;
            currentDirection = playerDirection;
        }

        // if (canPrimaryAttack() && playerDistance < primaryAttackRange/2) {
        //     currentState = State.casting;

        // } 


        performStateActions(dt);

		MoveAndSlide();
	}
}
