using Godot;
using System;

public partial class WeaponBody : Node2D
{
    // CONSTANT
    [Export] CollisionPolygon2D hitbox;
    [Export] Node2D weaponBody;
    enum AttackState {leftSwing, rightSwing, forwardThrust}

    Vector2 startingPosition = new Vector2(0, -60);
    float damage = 1;
    float knockbackSpeed = 1000;
    float knockbackTime = 0.2f;
    float swingDegrees = 120;
    float animationTime = 0.2f;
    float horizontalMoveDistance = 150;
    float attackDashSpeed = 1500;

    // VARYING
    AttackState attackType; 
    float currentAnimationTimer = 0; 

    public override void _Ready() {
        weaponBody.Position = startingPosition;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        if (currentAnimationTimer <= 0) {return;} 

        float dt = (float) delta;

        switch (attackType) {
            case AttackState.leftSwing:
                UpdateLeftSwing(dt);
                break;
            case AttackState.rightSwing:
                UpdateRightSwing(dt);
                break;
            case AttackState.forwardThrust:
                UpdateForwardThrust(dt);
                break;
        }
	}

    public void _OnBodyEntered(Node2D body) {
        onAttackHit(body);
    }

    void SetHitboxEnabled(bool value) {
        hitbox.Disabled = !value;
    }

    void onAttackHit(Node2D body) {
        Vector2 knockbackDirection = (body.Position - GlobalPosition).Normalized();
        body.Call("onHit", damage, knockbackDirection, knockbackSpeed, knockbackTime);
    }

    void StartAttack() {
        currentAnimationTimer = animationTime;
        SetHitboxEnabled(true);
    }

    void UpdateLeftSwing(float dt) {
        float weight = currentAnimationTimer/animationTime;

        float halfRadians = Mathf.DegToRad(swingDegrees/2);
        weaponBody.Rotation = Mathf.LerpAngle(halfRadians, -halfRadians, weight);

        float halfHorizontal = horizontalMoveDistance/2;
        float newX = Mathf.Lerp(halfHorizontal, -halfHorizontal, weight);
        weaponBody.Position = new Vector2(newX, weaponBody.Position.Y);
        
        currentAnimationTimer -= dt;
        if (currentAnimationTimer <= 0) {
            attackType = AttackState.rightSwing;
            SetHitboxEnabled(false);
        }
    }

    void UpdateRightSwing(float dt) {
        float weight = currentAnimationTimer/animationTime;

        float halfRadians = Mathf.DegToRad(swingDegrees/2);
        weaponBody.Rotation = Mathf.LerpAngle(-halfRadians, halfRadians, weight);

        float halfHorizontal = horizontalMoveDistance/2;
        float newX = Mathf.Lerp(-halfHorizontal, halfHorizontal, weight);
        weaponBody.Position = new Vector2(newX, weaponBody.Position.Y);
        
        currentAnimationTimer -= dt;
        if (currentAnimationTimer <= 0) {
            attackType = AttackState.forwardThrust;
            SetHitboxEnabled(false);
        }
    }

    void UpdateForwardThrust(float dt) {
        float weight = currentAnimationTimer/animationTime;

        weaponBody.Position = startingPosition;
        weaponBody.Rotation = 0;
        
        currentAnimationTimer -= dt;
        if (currentAnimationTimer <= 0) {
            attackType = AttackState.leftSwing;
            SetHitboxEnabled(false);
        }
    }



}
