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
		private RayCast3D rayCast;

		public override void _Ready()
		{
			this.shootComponent = this.GetNode<HitscanShootComponent>(
				NodePaths.HitscanShootComponent
			);

			this.rayCast = this.GetNode<RayCast3D>(NodePaths.RayCast3D);
		}

		public void Shoot(Vector3 fromPosition)
		{
			Logger.Print($"Shooting");
			this.rayCast.GlobalPosition = fromPosition;
			this.rayCast.TargetPosition = new Vector3(0, 0, -5);
			this.shootComponent.Shoot();
		}
	}
}
