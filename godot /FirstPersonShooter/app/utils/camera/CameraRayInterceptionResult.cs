namespace App.Utils.CameraUtilsModule
{
	using Godot;

	public class CameraRayInterceptionResult
	{
		public GodotObject? Collider { get; set; }
		public Vector3 Position { get; set; }
		public Vector3? Normal { get; set; }
	}
}
