extends Node3D

@export var speed: Vector3 = Vector3(10, 50, 10)

func _process(delta: float) -> void:
	rotation_degrees += speed * delta
