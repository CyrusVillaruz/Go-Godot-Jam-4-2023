extends Node

@onready var card1 = $Card1
@onready var card2 = $Card2
@onready var card3 = $Card3
@export var mouse_ent : Color
@export var mouse_exit : Color



func card3_enter():
	card3.modulate.a = 0.5

func card2_enter():
	card2.modulate.a = 0.5

func card_enter():
	card1.modulate.a = 0.5

func card_exit():
	card1.modulate.a = 1

func card3_exit():
	card3.modulate.a = 1

func card2_exit():
	card2.modulate.a = 1
