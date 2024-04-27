namespace App.Shared.Components
{
	using App.Shared.Utils;
	using Godot;

	public partial class HitscanShootComponent : Node3D
	{
		public void Shoot(RayCast3D rayCast, float range)
		{
			Logger.Print($"Shotting with `rayCast`: {rayCast}.");
			Logger.Print($"Shotting with `range`: {range}.");
		}
	}
}
