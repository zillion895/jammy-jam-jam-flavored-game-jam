extends MeshInstance3D

@onready var InteractField = $InteractField
@onready var HealthBar = get_parent().get_node("HealthBar")
@onready var label = $Label3D
@onready var timer = $Timer

@onready var operational = true

func _ready() -> void:
	timer.connect("timeout", Callable(self, "_on_timer_timeout"))
	InteractField.interact = Callable(self, "_fix")

func _on_timer_timeout():
	if operational == true:
		HealthBar.value += 1
	else:
		HealthBar.value -= 1

func _fix():
	label.text = "Working"
	operational = true
	
func _break():
		operational = false
		label.text = "Fix Me"
	
