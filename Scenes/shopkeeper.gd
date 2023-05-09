extends StaticBody2D

@export var facing_direction : Vector2 = Vector2(0, 1)

@onready var animation_tree = $AnimationTree
@onready var state_machine = animation_tree.get("parameters/playback")
@onready var interaction_hit_box = $InteractionHitBox

func _ready():
	animation_tree.active = true
	animation_tree.set("parameters/Idle/blend_position", facing_direction)

func _physics_process(delta):
	state_machine.travel("Idle")
	
	# Logic to handle animation facing direction.
	# e.g. if player is left of shopkeeper, make shopkeeper play
	# the idle_left animation
