using Godot;
using System;
using System.Numerics;

public partial class Door : Node3D
{
	private Node3D door;
	private Node3D endPoint;
	private Godot.Vector3 startPoint;
	private Node interactField;
	private Area3D catArea;
	private int catsInRange = 0;
	private Timer closeTimer;

	public override void _Ready()
	{
		door = GetNode<Node3D>("model");
		endPoint = GetNode<Node3D>("endPoint");
		startPoint = door.Position;

		interactField = GetNode("InteractField");
		if (interactField == null)
		{
			GD.PrintErr("Door: InteractField node not found.");
			return;
		}

		interactField.Set("interact", Callable.From(Open));

		catArea = new Area3D();
		catArea.CollisionLayer = 4; // different layer
		catArea.CollisionMask = 1; // detect cats on layer 1?
		var shape = new CollisionShape3D();
		shape.Shape = new SphereShape3D { Radius = 3f }; // detection radius
		catArea.AddChild(shape);
		AddChild(catArea);

		catArea.BodyEntered += OnCatEntered;
		catArea.BodyExited += OnCatExited;

		closeTimer = new Timer();
		closeTimer.OneShot = true;
		closeTimer.WaitTime = 2f;
		closeTimer.Timeout += Close;
		AddChild(closeTimer);
	}

	private void OnCatEntered(Node3D body)
	{
		if (body.IsInGroup("cats"))
		{
			catsInRange++;
			if (catsInRange == 1)
			{
				Open();
				closeTimer.Stop(); // Hold open
			}
		}
	}

	private void OnCatExited(Node3D body)
	{
		if (body.IsInGroup("cats"))
		{
			catsInRange--;
			if (catsInRange == 0)
			{
				closeTimer.Start(); // Start closing after delay
			}
		}
	}

	public void Open()
	{
		GD.Print("Door opened by cat or interaction");
		Tween tween = CreateTween();
		tween.TweenProperty(door, "position", endPoint.Position, 0.3f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
		if (catsInRange == 0)
		{
			closeTimer.Start();
		}
	}

	public void Close()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(door, "position", startPoint, 0.3f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
	}
}
