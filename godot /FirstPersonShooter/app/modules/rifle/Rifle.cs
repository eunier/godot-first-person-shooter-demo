namespace App.Modules.RifleModule
{
	using App.Modules.HealthModule;
	using App.Modules.HitscanModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Rifle : Weapon
	{
		private const string HitscanNodePath = "%Hitscan";
		private Hitscan? hitscan;

		public override void _Ready()
		{
			this.hitscan = this.GetNode<Hitscan>(Rifle.HitscanNodePath);
		}

		public void Shoot(Camera3D camera)
		{
			Logger.Print($"Shooting");
			var res = this.hitscan!.Shoot(camera, 100);

			if (res is not null && res.Collider is IWithHealth c)
			{
				c.Health.Damage(5);
			}

			Logger.Print($"Hit something? res: {res}.");
		}
	}
}
