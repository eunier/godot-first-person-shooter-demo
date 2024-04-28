namespace App.Modules.Rifle
{
	using App.Global;
	using App.Shared.Abstract;
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		private GlobalState? globalState;
		private HitscanShootComponent? shootComponent;
		private RayCast3D? rayCast;

		public override void _Ready()
		{
			this.globalState = this.GetNode<GlobalState>(
				RifleConstants.NodePaths.GlobalState
			);

			this.shootComponent = this.GetNode<HitscanShootComponent>(
				RifleConstants.NodePaths.HitscanShootComponent
			);

			this.rayCast = this.GetNode<RayCast3D>(
				RifleConstants.NodePaths.RayCast3D
			);
		}

		public void Shoot()
		{
			Logger.Print($"Shooting");
			Logger.Print($"{this.rayCast?.GlobalPosition}");

			if (this.globalState?.PlayerCameraGlobalPosition is not null)
			{
				this.rayCast = new RayCast3D
				{
					GlobalPosition = (Vector3)this.globalState.PlayerCameraGlobalPosition,
					TargetPosition = new Vector3(0, 0, -5)
				};

				this.AddChild(this.rayCast);
				var collider = this.shootComponent?.Shoot();
				Logger.Print($"{this.rayCast.GlobalPosition}");
			}
		}
	}
}
