namespace App.Shared.Components
{
	using App.Shared.Utils;
	using Godot;

	public partial class HitscanShootComponent : Node3D
	{
		public GodotObject? Shoot(RayCast3D rayCast)
		{
			var collider = rayCast.GetCollider();

#if DEBUG
			if (collider is not null)
			{
				Logger.Print($"Hit: {collider}.");
			}
			else
			{
				Logger.Print("Miss.");
			}
#endif

			return collider;
		}
	}
}
