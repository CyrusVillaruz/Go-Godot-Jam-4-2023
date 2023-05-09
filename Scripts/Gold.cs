using Godot;
using System;

public partial class Gold : RigidBody2D
{
    // CONSTANT
    [Export] Sprite2D sprite;
    const float bobbingRotation = Mathf.Pi;
    const float bobbingVertical = 100;
    float positionAnimationDuration = 1.5f;
    float rotationAnimationDuration = 2.5f;
    Vector2 originalPosition;
    float waterSpeed = 50;

    // VARYING
    float rotationAnimationTimer;
    float positionAnimationTimer;
    float destroyTime = 60;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
        originalPosition = Position;
        rotationAnimationTimer = rotationAnimationDuration;
        positionAnimationTimer = positionAnimationDuration;

        LinearVelocity = new Vector2(waterSpeed, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        float dt = (float) delta;

        bobbing(dt);
	}

    void bobbing(float dt) {
        float rotationDisplacement = bobbingRotation * dt/rotationAnimationDuration;
        Vector2 positionDisplacement = new Vector2(0, bobbingVertical * dt/positionAnimationDuration);

        sprite.Position += positionAnimationTimer <= positionAnimationDuration*0.5f? positionDisplacement: -positionDisplacement;
        sprite.Rotate(rotationAnimationTimer <= rotationAnimationDuration*0.5f? rotationDisplacement: -rotationDisplacement);

        positionAnimationTimer -= dt;
        if (positionAnimationTimer <= 0) {
            positionAnimationTimer = positionAnimationDuration;
        }

        rotationAnimationTimer -= dt;
        if (rotationAnimationTimer <= 0) {
            rotationAnimationTimer = rotationAnimationDuration;
        }

        destroyTime -= dt;
        if (destroyTime <= 0) {
            QueueFree();
        }

    }
}
