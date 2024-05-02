namespace App.Modules.HitscanModule
{
	using Godot;

	public class ShootResult
	{
		public ShootResult(GodotObject collider, Vector3 position, Vector3 normal)
		{
			this.Collider = collider;
			this.Position = position;
			this.Normal = normal;
		}

		public GodotObject Collider { get; private set; }
		public Vector3 Position { get; private set; }
		public Vector3 Normal { get; private set; }
	}
}
