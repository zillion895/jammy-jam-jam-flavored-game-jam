using Godot;
using System;

public partial class Player : CharacterBody3D
{
    private const float speed = 8.0f;
    private const float CONVERGENCE_SPEED = 0.8f;
    private const float WALKING_CONVERGENCE_SPEED = 0.5f;

    private const float RUN_VELOCITY = 12f;
    private float axisX;
    private float axisY;
    private Node3D cameraRoot;
    private Node3D modelOrientation;
    public override void _Ready()
    {
        cameraRoot = GetNode<Node3D>("camRoot");
        modelOrientation = GetNode<Node3D>("orientation");
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }
        handleInput();
        // Get the input direction and handle the movement/deceleration.

        Vector3 direction = (cameraRoot.Transform.Basis * new Vector3(axisX, 0, axisY)).Normalized();
        if (direction != Vector3.Zero)
        {
            float targetSpeed = Input.IsKeyPressed(Key.Shift) ? RUN_VELOCITY : speed;
            if (Input.IsKeyPressed(Key.Shift))
            {
                GD.Print("Running");
            }
            velocity.X = Mathf.MoveToward(velocity.X, direction.X * targetSpeed, WALKING_CONVERGENCE_SPEED);
            velocity.Z = Mathf.MoveToward(velocity.Z, direction.Z * targetSpeed, WALKING_CONVERGENCE_SPEED);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, CONVERGENCE_SPEED);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, CONVERGENCE_SPEED);
        }
        //look in direction of movement
        if (velocity.LengthSquared() > 0.01f)
        {            modelOrientation.LookAt(GlobalPosition + velocity, Vector3.Up);
        }
        

        Velocity = velocity;
        MoveAndSlide();
    }
    private void handleInput()
    {
        axisX = Input.GetAxis("move_left", "move_right");
        axisY = Input.GetAxis("move_up", "move_down");
    }
}
