extends Node

const HTerrainData = preload("res://addons/zylann.hterrain/hterrain_data.gd")

@onready var _terrain = $"../HTerrain"

func _ready():
	run()

func run():
	print("GD: Entered.")
	# Get the image
	var data : HTerrainData = _terrain.get_data()
	var colormap : Image = data.get_image(HTerrainData.CHANNEL_COLOR)

	# Modify the image
	var position = Vector2i(42, 36)
	colormap.set_pixel(position.x, position.y, Color(0, 0, 0))
	

	# Notify the terrain of our change
	data.notify_region_changed(Rect2(position.x, position.y, 1, 1), HTerrainData.CHANNEL_COLOR)
