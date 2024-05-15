#if TOOLS
using Godot;
using System;

namespace Terrain
{
	[Tool]
	public partial class SmoothTerrain : EditorPlugin
	{
		public override void _EnterTree()
		{
			
		}

		public override void _ExitTree()
		{
			// Clean-up of the plugin goes here.
		}
	}
}
#endif
