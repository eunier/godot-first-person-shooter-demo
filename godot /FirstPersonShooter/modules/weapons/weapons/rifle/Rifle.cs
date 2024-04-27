namespace App.Modules.Weapons
{
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		[Export]
		private HitscanShootComponent shootComponent;

		public void Shoot(RayCast3D rayCast)
		{
			Logger.Print($"Shotting with `rayCast`: {rayCast}.");
			this.shootComponent.Shoot(rayCast, 100);
		}
	}
}
