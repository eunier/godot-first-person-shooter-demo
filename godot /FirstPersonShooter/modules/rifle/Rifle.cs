namespace App.Modules.Rifle
{
	using App.Modules.Rifle.Constants;
	using App.Shared.Abstract;
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		private HitscanShootComponent shootComponent;
		public RayCast3D RayCast { private get; set; }

		public override void _Ready()
		{
			this.shootComponent = this.GetNode<HitscanShootComponent>(
				NodePaths.HitscanShootComponent
			);
		}

		public void Shoot()
		{
			if (this.RayCast is null)
			{
				Logger.Print("No `RayCast` defined, returning.");
				return;
			}

			Logger.Print($"Shooting with `RayCast`: {this.RayCast}.");
			this.shootComponent.Shoot(this.RayCast, 100);
		}
	}
}
