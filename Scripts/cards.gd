extends Node

@onready var card = $Card
@onready var card1 = $Card/Card1
@onready var card2 = $Card/Card2
@onready var card3 = $Card/Card3
@onready var animate = $AnimationPlayer
@onready var overlay = $Overlay
@onready var pick = $Pick

var pause_state = false
# Called when the node enters the scene tree for the first time.
func _ready():
	unpause()

func unpause():
	pause_state = false
	get_tree().paused = pause_state
	card1.visible = pause_state
	card2.visible = pause_state
	card3.visible = pause_state
	overlay.visible = pause_state
	pick.visible = pause_state

func pause():
	pause_state = true
	get_tree().paused = pause_state
	card1.visible = pause_state
	card2.visible = pause_state
	card3.visible = pause_state
	overlay.visible = pause_state
	animate.play("flip")
	
func _input(event):
	if event.is_action_pressed("test-card"):
		pause()

func _on_animation_player_animation_finished(anim_name):
	pick.visible = true
