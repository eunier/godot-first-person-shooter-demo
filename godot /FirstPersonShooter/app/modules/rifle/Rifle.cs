namespace App.Modules.RifleModule
{
	using App.Modules.HealthModule;
	using App.Modules.HitscanModule;
	using App.Modules.SparkControllerModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Rifle : Weapon
	{
		private const string FireRateTimerNodePath = "%FireRateTimer";
		private const string HitscanNodePath = "%Hitscan";
		private const string MuzzleFlashNodePath = "%MuzzleFlash";
		private const string SparkControllerNodePath = "%SparkController";

		[Export]
		private WeaponResource? resource;

		private GpuParticles3D? muzzleFlash;
		private Hitscan? hitscan;
		private SparkController? sparkController;
		private Timer? fireRateTimer;

		public override void _Ready()
		{
			this.hitscan = this.GetNode<Hitscan>(Rifle.HitscanNodePath);

			this.sparkController = this.GetNode<SparkController>(
				Rifle.SparkControllerNodePath
			);

			this.fireRateTimer = this.GetNode<Timer>(Rifle.FireRateTimerNodePath);
			this.fireRateTimer!.WaitTime = this.resource!.FireRateWaitTime;

			this.muzzleFlash = this.GetNode<GpuParticles3D>(
				Rifle.MuzzleFlashNodePath
			);
		}

		public void Shoot(Camera3D camera)
		{
			if (!this.fireRateTimer!.IsStopped())
			{
				return;
			}

			Logger.Print($"Shooting");
			this.muzzleFlash!.Restart();
			this.fireRateTimer!.Start();
			var res = this.hitscan!.Shoot(camera, this.resource!.Range);

			if (res is not null)
			{
				this.sparkController!.Create(res.Position);

				if (res.Collider is IWithHealth collider)
				{
					collider.Health.Damage(this.resource!.Damage);
				}
			}

			Logger.Print($"Hit something? res: {res}.");
		}
	}
}
