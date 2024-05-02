namespace App.Modules.RifleModule
{
	using App.Modules.HealthModule;
	using App.Modules.HitscanModule;
	using App.Modules.SparksControllerModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Rifle : Weapon
	{
		private const float FireRate = 14;
		private const int DamageAmount = 15; // TODO: load the resource
		private const string FireRateTimerNodePath = "%FireRateTimer";
		private const string HitscanNodePath = "%Hitscan";
		private const string MuzzleFlashNodePath = "%MuzzleFlash";
		private const string SparksControllerNodePath = "%SparksController";
		private GpuParticles3D? muzzleFlash;
		private Hitscan? hitscan;
		private SparksController? sparksController;
		private Timer? fireRateTimer;

		public override void _Ready()
		{
			this.hitscan = this.GetNode<Hitscan>(Rifle.HitscanNodePath);

			this.sparksController = this.GetNode<SparksController>(
				Rifle.SparksControllerNodePath
			);

			this.fireRateTimer = this.GetNode<Timer>(Rifle.FireRateTimerNodePath);
			this.fireRateTimer!.WaitTime = 1 / Rifle.FireRate;

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
			var res = this.hitscan!.Shoot(camera, 100); // TODO: use resource

			if (res is not null)
			{
				this.sparksController!.Create(res.Position); // TODO remove sparks to spark (include controller)

				if (res.Collider is IWithHealth collider)
				{
					collider.Health.Damage(Rifle.DamageAmount);
				}
			}

			Logger.Print($"Hit something? res: {res}.");
		}
	}
}
