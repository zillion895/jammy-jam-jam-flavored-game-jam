extends MeshInstance3D

@onready var InteractField = $InteractField
@onready var HealthBar = $"/root/HealthBar"
@onready var label = $Label3D
@onready var timer = $Timer

@onready var operational = true

func _ready() -> void:
	timer.connect("timeout", Callable(self, "_on_timer_timeout"))
	InteractField.interact = Callable(self, "_fix")

func _on_timer_timeout():
	print("Timer")
	if operational == true:
		_break()
	else:
		print("damage")
		HealthBar.value -= 1

func _fix():
	label.text = "Fixed"
	print("fix")
	operational = true
	
func _break():
		print("Break")
		operational = false
		label.text = "Fix Me"
	
