using Godot;
using System;

public partial class Jail : Node3D
{
	private Node3D vacuum;
	private Area3D jailField;

	public override void _Ready()
	{
		vacuum = GetNode<Node3D>("vacuum");
		if (vacuum == null)
		{
			GD.PrintErr("Jail: Vacuum node not found.");
			return;
		}

		jailField = new Area3D();
		jailField.CollisionLayer = 8; // arbitrary layer
		jailField.CollisionMask = 1; // detect on layer 1
		var shape = new CollisionShape3D();
		shape.Shape = new BoxShape3D { Size = new Vector3(5, 5, 5) }; // adjust size
		jailField.AddChild(shape);
		AddChild(jailField);

		jailField.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("cats") && body is RigidBody3D rb)
		{
			GD.Print("Cat entered jail, sucking to vacuum");
			rb.Freeze = true; // Disable physics
			if (rb is CatColect cat)
			{
				cat.BreakRope();
			}
			Tween tween = CreateTween();
			tween.TweenProperty(rb, "global_position", vacuum.GlobalPosition, 0.5f).SetTrans(Tween.TransitionType.Sine);
			tween.TweenCallback(Callable.From(() => rb.QueueFree())); // Remove cat after sucking
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
