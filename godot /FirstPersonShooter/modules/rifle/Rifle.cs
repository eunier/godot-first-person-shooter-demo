namespace App.Modules.Rifle
{
	using App.Modules.Rifle.Constants;
	using App.Shared.Abstract;
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		[Export]
		private RayCast3D rayCast;

		private HitscanShootComponent shootComponent;

		public override void _Ready()
		{
			this.shootComponent = this.GetNode<HitscanShootComponent>(
				NodePaths.HitscanShootComponent
			);
		}

		public void Shoot()
		{
			Logger.Print($"Shotting with `rayCast`: {this.rayCast}.");
			this.shootComponent.Shoot(this.rayCast, 100);
		}
	}
}
