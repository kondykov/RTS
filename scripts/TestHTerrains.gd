extends Node

const HTerrain = preload("res://addons/zylann.hterrain/hterrain.gd")
const HTerrainData = preload("res://addons/zylann.hterrain/hterrain_data.gd")

func _ready():
	var data = HTerrainData.new()
	data.resize(513)

	var terrain = HTerrain.new()
	terrain.set_data(data)

	var colormap : Image = data.get_image(HTerrainData.CHANNEL_COLOR)

	# Modify the image
	var position = Vector2(42, 36)
	colormap.set_pixel(position.x, position.y, Color(1, 0, 0))
	var heightmap : Image = data.get_image(HTerrainData.CHANNEL_HEIGHT)
	heightmap.set_pixel(position.x, position.y, Color(1,1,1))
	terrain.name = "terrain"
	add_child(terrain)

func _process(delta):
	var _terrain = $"./terrain"
	if(Input.is_action_pressed("mouse_left_click")):
		var camera_controller = load("res://scripts/camera/WorldCamera.cs")
		var main_controller = load("res://scripts/MainCommandHandler.cs")
		var cam = camera_controller.new()
		var raycast = cam.GetRaycast()
		if(raycast == null):
			return
		var position = main_controller.Vector3FloatToInt(raycast['position'])
		
		var data : HTerrainData = _terrain.get_data()
		var colormap : Image = data.get_image(HTerrainData.CHANNEL_COLOR)
		
		colormap.set_pixel(position.x, position.z, Color(0, 0, 1, 0))
		data.notify_region_change(Rect2(position.x, position.z, 1, 1), HTerrainData.CHANNEL_COLOR)
		print("gd", raycast['position'], position)
	pass
	
