using Godot;
using System;

public partial class MainCamera : Camera2D
{
    // CONSTANT
    [Export] CharacterBody2D playerBody;
    float maxSpeed = 2000;
    float minSpeed = 10f;
    float maxDistance = 1000;
    const float minPositionChange = 11;
    RandomNumberGenerator randomGenerator;

    // VARYING
    float shakeDuration = 0;
    float shakeTimer = 0;
    float minStrength = 0;
    float maxStrength = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
        randomGenerator = new RandomNumberGenerator();
        randomGenerator.Randomize();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
        float dt = (float) delta;

        followPlayer(dt);
        if (shakeTimer > 0) {
            performShake(dt);
        }
	}

    void startShake(float time, float minStrength, float maxStrength) {
        shakeDuration = time;
        shakeTimer = time;
        this.minStrength = minStrength;
        this.maxStrength = maxStrength;
    }

    void followPlayer(float dt) {
        float playerDistance = (playerBody.Position - Position).Length();
        Vector2 playerDirection = (playerBody.Position - Position).Normalized();

        Vector2 positionChange = playerDirection * Mathf.Lerp(minSpeed, maxSpeed, playerDistance/maxDistance) * dt;
        if (positionChange.Length() >= minPositionChange*dt) {
            Position += positionChange;
        }
        else {
            Position = playerBody.Position;
        }
    }

    void performShake(float dt) {
        float randomX = randomGenerator.RandfRange(-1, 1);
        float randomY = randomGenerator.RandfRange(-1, 1);
        float randomStrength = randomGenerator.RandfRange(minStrength, maxStrength);
        Vector2 randomDirection = new Vector2(randomX, randomY).Normalized();

        Position += randomDirection * randomStrength;

        shakeTimer -= dt;
    }

}
