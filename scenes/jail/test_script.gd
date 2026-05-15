extends Node3D

@export var speed: Vector3 = Vector3(50, 50, 50)

func _process(delta: float) -> void:
	rotation_degrees += speed * delta
