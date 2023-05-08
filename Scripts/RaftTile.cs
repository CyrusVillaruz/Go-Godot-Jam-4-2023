using Godot;
using System;

public partial class RaftTile : Building
{
    //CONSTANT
    [Export] float knockbackDuration = 0.15f;
    [Export] float knockbackSpeed = 800;
    
    [Export] float health = 10;
    [Export] float damage = 1;

    public void onDamageAreaHit(Node2D body) {
        Vector2 knockbackDirection = (body.Position - Position).Normalized();
        body.Call("onHit", damage, knockbackDirection, knockbackSpeed, knockbackDuration);
    }
}
