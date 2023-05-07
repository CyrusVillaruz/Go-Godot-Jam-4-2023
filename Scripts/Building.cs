using Godot;
using System;

public partial class Building : Node2D
{
    // CONSTANT
    [Export] Sprite2D sprite;
    const float animationDuration = 0.3f;
    Vector2 verticalAnimationDisplacement = new Vector2(0, -70);
    float rotationDisplacement = 120;
    Vector2 scaleDisplacement = new Vector2(-0.75f, -0.75f);
    Vector2 originalPosition;
    float originalRotation;
    Vector2 originalScale;

    // VARYING
    float animationTimer = 0;


    public void StartPlaceAnimation() {
        originalPosition = Position;
        originalRotation = Rotation;
        originalScale = Scale;

        animationTimer = animationDuration;

        sprite.SelfModulate = new Color(sprite.SelfModulate, 1);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        float dt = (float) delta;

        if (animationTimer > 0) {
            float weight = animationTimer/animationDuration;
            Position = originalPosition.Lerp(originalPosition + verticalAnimationDisplacement, weight);
            Rotation = Mathf.LerpAngle(originalRotation, originalRotation+rotationDisplacement, weight);
            Scale = originalScale.Lerp(originalScale + scaleDisplacement, weight);

            animationTimer -= dt;
            if (animationTimer <= 0) {
                Position = originalPosition;
                Rotation = originalRotation;
            }
        }
	}
}
