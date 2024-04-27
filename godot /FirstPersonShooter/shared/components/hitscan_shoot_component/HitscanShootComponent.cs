namespace App.Shared.Components
{
	using App.Shared.Utils;
	using Godot;

	public partial class HitscanShootComponent : Node3D
	{
		[Export]
		private RayCast3D rayCast;

		public GodotObject Shoot()
		{
			if (this.rayCast is null)
			{
				Logger.Print("No RayCast3D set, returning.");
				return null;
			}

			var collider = this.rayCast.GetCollider();

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
