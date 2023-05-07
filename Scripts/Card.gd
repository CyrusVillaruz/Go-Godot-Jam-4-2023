extends Sprite2D

signal click

@export var mouse_enter: Color
@export var mouse_exit: Color

@onready var timer = $Timer
var allow_click = true

func _input(event):
	if event.is_action("Attack") and allow_click:
		if get_rect().has_point(to_local(event.position)):
			allow_click = false
			timer.start(0.5)
			emit_signal("click")


func _on_timer_timeout():
	allow_click = true

func _on_area_2d_mouse_entered():
	modulate = mouse_enter

func _on_area_2d_mouse_exited():
	modulate = mouse_exit
