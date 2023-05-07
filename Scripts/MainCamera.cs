using Godot;
using System;

public partial class MainCamera : Camera2D
{
    // CONSTANT
    [Export] CharacterBody2D playerBody;
    float maxSpeed = 2000;
    float minSpeed = 0f;
    float maxDistance = 1000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
        float dt = (float) delta;
        float playerDistance = (playerBody.Position - Position).Length();
        Vector2 playerDirection = (playerBody.Position - Position).Normalized();

        Position += playerDirection * Mathf.Lerp(minSpeed, maxSpeed, playerDistance/maxDistance) * dt;
	}
}
