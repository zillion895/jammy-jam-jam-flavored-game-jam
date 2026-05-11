using Godot;
using System;
using System.Numerics;

public partial class Door : Node3D
{
	private Node3D door;
	private Node3D endPoint;
	private Godot.Vector3 startPoint;
	private Node interactField;

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
	}

	public void Open()
	{
		GD.Print("open");
		Tween tween = CreateTween();
		tween.TweenProperty(door, "position", endPoint.Position, 1f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
		tween.TweenCallback(Callable.From(() => close())).SetDelay(2f);
	}

	public void close()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(door, "position", startPoint, 1f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
	}
}
		
