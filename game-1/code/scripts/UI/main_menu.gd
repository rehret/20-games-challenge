extends Node2D

@export_file("*.tscn") var main_scene : String

func _on_start_button_pressed() -> void:
	get_tree().change_scene_to_file(main_scene)
