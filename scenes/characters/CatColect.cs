using Godot;
using System;

public partial class CatColect : RigidBody3D
{
	private Node3D player;
	private Area3D detectionArea;
	private bool playerInRange = false;
	private bool isAttracting = false;
	private bool spaceWasPressed = false;
	private float attractionStrength = 50f;
	private float minDistance = 4f;
	private float machineMinDistance = 2f;
	private float repelDistance = 2f;
	private MeshInstance3D ropeMesh;
	private float directionChangeTimer = 0f;
	private Vector3 currentRandomDir;

	public override void _Ready()
	{
		player = (Node3D)GetTree().GetFirstNodeInGroup("player");
		if (player == null)
		{
			GD.PrintErr("CatColect: Player not found in group 'player'");
			return;
		}

		// Set damping and low friction to prevent sticking
		this.LinearDamp = 5f;
		this.PhysicsMaterialOverride = new PhysicsMaterial { Friction = 0.1f };
		this.AddToGroup("cats");

		detectionArea = new Area3D();
		detectionArea.CollisionLayer = 2;
		detectionArea.CollisionMask = 1;
		var shape = new CollisionShape3D();
		shape.Shape = new SphereShape3D { Radius = 2f };
		detectionArea.AddChild(shape);
		AddChild(detectionArea);

		detectionArea.BodyEntered += OnBodyEntered;
		detectionArea.BodyExited += OnBodyExited;

		ropeMesh = new MeshInstance3D();
		ropeMesh.Mesh = new CylinderMesh { Height = 1, TopRadius = 0.05f, BottomRadius = 0.05f };
		AddChild(ropeMesh);
		ropeMesh.Visible = false;

		currentRandomDir = new Vector3((float)Random.Shared.NextDouble() * 2 - 1, 0, (float)Random.Shared.NextDouble() * 2 - 1).Normalized();
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("player"))
		{
			playerInRange = true;
			GD.Print("Player entered cat range");
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body.IsInGroup("player"))
		{
			playerInRange = false;
			GD.Print("Player exited cat range");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (playerInRange && Input.IsKeyPressed(Key.Space))
		{
			if (!spaceWasPressed)
			{
				isAttracting = !isAttracting;
				GD.Print("Cat attraction toggled: ", isAttracting);
				spaceWasPressed = true;
			}
		}
		else
		{
			spaceWasPressed = false;
		}
	}

	public override void _Process(double delta)
	{
		if (isAttracting && player != null)
		{
			ropeMesh.Visible = true;
			Vector3 dir = player.GlobalPosition - GlobalPosition;
			float length = dir.Length();
			if (length > 0.01f)
			{
				Vector3 mid = GlobalPosition + dir / 2;
				ropeMesh.GlobalPosition = mid;

				Vector3 forward = dir.Normalized();
				Vector3 right = forward.Cross(Vector3.Up);
				if (right.LengthSquared() < 0.001f)
				{
					right = Vector3.Right;
				}
				right = right.Normalized();
				Vector3 up = right.Cross(forward);
				ropeMesh.GlobalBasis = new Basis(right, forward, up);

				ropeMesh.Scale = new Vector3(1, length, 1);
			}
		}
		else
		{
			ropeMesh.Visible = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
			float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
			if (distance < repelDistance)
			{
				// Repel from player
				Vector3 direction = (GlobalPosition - player.GlobalPosition).Normalized();
				ApplyCentralForce(direction * attractionStrength);
			}
			else if (isAttracting)
			{
				if (distance > minDistance)
				{
					float extraStrength = (distance - minDistance) * 10f;
					float totalStrength = attractionStrength + extraStrength;
					Vector3 direction = (player.GlobalPosition - GlobalPosition).Normalized();
					ApplyCentralForce(direction * totalStrength);
				}
			}
			else
			{
				// Pathfind to nearest machine
				var machines = GetTree().GetNodesInGroup("Machines");
				Node3D nearestMachine = null;
				float minDist = float.MaxValue;
				foreach (var m in machines)
				{
					if (m is Node3D nm)
					{
						float d = GlobalPosition.DistanceTo(nm.GlobalPosition);
						if (d < minDist)
						{
							minDist = d;
							nearestMachine = nm;
						}
					}
				}
				if (nearestMachine != null && minDist > machineMinDistance)
				{
					if (minDist > 15f)
					{
						// Run in random direction, change every second
						directionChangeTimer -= (float)delta;
						if (directionChangeTimer <= 0)
						{
							currentRandomDir = new Vector3((float)Random.Shared.NextDouble() * 2 - 1, 0, (float)Random.Shared.NextDouble() * 2 - 1).Normalized();
							directionChangeTimer = 1f;
						}
						ApplyCentralForce(currentRandomDir * attractionStrength);
					}
					else
					{
						Vector3 direction = (nearestMachine.GlobalPosition - GlobalPosition).Normalized();
						ApplyCentralForce(direction * attractionStrength);
					}
				}
			}
		}
	}

	public void BreakRope()
	{
		ropeMesh.Visible = false;
		isAttracting = false;
	}
}
