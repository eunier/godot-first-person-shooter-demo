namespace App.Modules.HitscanModule
{
	using Godot;

	public class ShootResult
	{
		public ShootResult(GodotObject collider)
		{
			this.Collider = collider;
		}

		public GodotObject Collider { get; private set; }
	}
}
