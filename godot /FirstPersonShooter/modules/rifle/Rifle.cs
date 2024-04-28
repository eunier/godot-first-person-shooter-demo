namespace App.Modules.RifleModule
{
	using App.Shared.Abstract;
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		private HitscanShootComponent? shootComponent;

		public override void _Ready()
		{
			this.shootComponent = this.GetNode<HitscanShootComponent>(
				RifleConstants.NodePaths.HitscanShootComponent
			);
		}

		public void Shoot(Camera3D camera)
		{
			Logger.Print($"Shooting");
			var res = this.shootComponent!.Shoot(camera, 100);
			Logger.Print($"Hit something? res: {res}.");
		}
	}
}
