using Godot;
using System;

public partial class Character : CharacterBody2D
{

    // possible player states
    enum State {idle, walking, dashing, casting} 

	const float walkSpeed = 300.0f;
	const float dashSpeed = 1800.0f;
    const float dashTime = 0.2f;

    Vector2 currentDirection = Vector2.Zero;
    State currentState = State.idle;
    float currentActionTimer = 0; // time remaining for current state (used by dashing, casting, etc)
}
