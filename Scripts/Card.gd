extends Sprite2D

signal click

func _input(event):
	if event.is_action("Attack"):
		if get_rect().has_point(to_local(event.position)):
			print("123")
			emit_signal("click")
