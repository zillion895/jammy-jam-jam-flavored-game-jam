extends MeshInstance3D

@onready var InteractionField = $InteractField
@onready var label = $Label3D

func _ready() -> void:
	InteractionField.interact = Callable(self, "_fix")

func _fix():
	label.text = "Fixed"
	print("interacted")
	
