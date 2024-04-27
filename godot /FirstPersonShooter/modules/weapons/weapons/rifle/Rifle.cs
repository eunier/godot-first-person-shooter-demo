namespace App.Modules.Weapons
{
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		[Export]
		private HitscanShootComponent shootComponent;

		[Export]
		private RayCast3D rayCast;

		public void Shoot()
		{
			Logger.Print($"Shotting with `rayCast`: {this.rayCast}.");
			this.shootComponent.Shoot(this.rayCast, 100);
		}
	}
}
