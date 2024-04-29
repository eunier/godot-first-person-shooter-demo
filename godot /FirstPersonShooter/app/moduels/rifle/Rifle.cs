namespace App.Modules.RifleModule
{
	using App.Module.Utils.LoggerModule;
	using App.Modules.HitscanShooterModule;
	using App.Modules.WeaponModule;
	using Godot;

	public partial class Rifle : Weapon
	{
		private HitscanShooter? hitscanShooter;

		public override void _Ready()
		{
			this.hitscanShooter = this.GetNode<HitscanShooter>(
				RifleConstants.NodePaths.HitscanShootComponent
			);
		}

		public void Shoot(Camera3D camera)
		{
			Logger.Print($"Shooting");
			var res = this.hitscanShooter!.Shoot(camera, 100);
			Logger.Print($"Hit something? res: {res}.");
		}
	}
}
