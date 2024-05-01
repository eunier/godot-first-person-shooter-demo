namespace App.Modules.RifleModule
{
	using App.Modules.HealthModule;
	using App.Modules.HitscanShooterModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Rifle : Weapon
	{
		private const string HitscanShooterNodePath = "%HitscanShooter";
		private HitscanShooter? hitscanShooter;

		public override void _Ready()
		{
			this.hitscanShooter = this.GetNode<HitscanShooter>(
				Rifle.HitscanShooterNodePath
			);
		}

		public void Shoot(Camera3D camera)
		{
			Logger.Print($"Shooting");
			var res = this.hitscanShooter!.Shoot(camera, 100);

			if (res is not null && res.Collider is IWithHealth c)
			{
				c.Health.Damage(5);
			}

			Logger.Print($"Hit something? res: {res}.");
		}
	}
}
