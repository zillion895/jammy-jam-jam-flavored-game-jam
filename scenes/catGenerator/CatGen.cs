using Godot;
using System;
using System.Runtime.CompilerServices;


//randomly generates cats in a radius around the scene 


public partial class CatGen : Node3D
{
	// Called when the node enters the scene tree for the first time.

	private float spawnRadius = 10f;
	private float spawnChance = 0.01f;

	private PackedScene catScene;
	public override void _Ready()
	{
		catScene = GD.Load<PackedScene>("res://scenes/characters/cat.tscn");
		if (catScene == null)
    {
        GD.PrintErr("FAILED TO LOAD SCENE");
    }
    else
    {
        GD.Print("Scene loaded successfully");
    }

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (Random.Shared.NextDouble() < spawnChance)
		{
			GD.Print("Spawning cat");

			Vector3 randomPos = new Vector3(
				(float)(Random.Shared.NextDouble() * 2 - 1) * spawnRadius,
				0,
				(float)(Random.Shared.NextDouble() * 2 - 1) * spawnRadius
			);
			Node3D catInstance = catScene.Instantiate<Node3D>();
			catInstance.GlobalTransform = new Transform3D(
    catInstance.GlobalTransform.Basis,
    randomPos
);
			AddChild(catInstance);
		}
	}
	private void OnSpawnTimerTimeout()
	{
		
	}
}
