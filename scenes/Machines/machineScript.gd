extends MeshInstance3D

@onready var InteractField = $InteractField
@onready var HealthBar = get_parent().get_node("HealthBar")
@onready var label = $Label3D
@onready var timer = $Timer

@onready var operational = true

var cats_touching: int = 0

func _ready() -> void:
	timer.connect("timeout", Callable(self, "_on_timer_timeout"))
	InteractField.interact = Callable(self, "_fix")
	InteractField.body_entered.connect(_on_interact_field_body_entered)
	InteractField.body_exited.connect(_on_interact_field_body_exited)

func _is_cat_body(body: Node) -> bool:
	var current: Node = body
	while current:
		if current.is_in_group("cat"):
			return true
		if current.name.begins_with("Cat"):
			return true
		current = current.get_parent()
	return false

func _on_timer_timeout():
	print("Time")
	print(HealthBar.value)
	if cats_touching > 0:
		_break()
	
	if operational == true:
		HealthBar.value += 1
	else:
		HealthBar.value -= 5

func _on_interact_field_body_entered(body: Node3D) -> void:
	if _is_cat_body(body):
		cats_touching += 1

func _on_interact_field_body_exited(body: Node3D) -> void:
	if _is_cat_body(body):
		cats_touching -= 1

func _fix():
	label.text = "Working"
	operational = true
	
func _break():
	print("break")
	operational = false
	label.text = "Fix Me"
	
