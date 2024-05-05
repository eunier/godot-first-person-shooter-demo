namespace App.Modules.RifleModule
{
	using App.Modules.HealthModule;
	using App.Modules.HitscanModule;
	using App.Modules.SparkControllerModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Rifle : Weapon // TODO do i need this?
	{
		private const string FireRateTimerNodePath = "%FireRateTimer";
		private const string HitscanNodePath = "%Hitscan";
		private const string MuzzleFlashNodePath = "%MuzzleFlash";
		private const string SparkControllerNodePath = "%SparkController";
		private const string ReloadTimerNodePath = "%ReloadTimer";

		// [Export]
		// private WeaponResource? resource;

		private GpuParticles3D? muzzleFlash;
		private Hitscan? hitscan;
		private SparkController? sparkController;
		private Timer? fireRateTimer;
		private Timer? reloadTimer;

		public override void _Ready()
		{
			this.fireRateTimer = this.GetNode<Timer>(Rifle.FireRateTimerNodePath);
			// this.fireRateTimer!.WaitTime = this.resource!.FireRateWaitTime;
			this.reloadTimer = this.GetNode<Timer>(Rifle.ReloadTimerNodePath);
			// this.reloadTimer.WaitTime = this.resource!.ReloadTime;
			this.hitscan = this.GetNode<Hitscan>(Rifle.HitscanNodePath);

			this.sparkController = this.GetNode<SparkController>(
				Rifle.SparkControllerNodePath
			);

			this.muzzleFlash = this.GetNode<GpuParticles3D>(
				Rifle.MuzzleFlashNodePath
			);
		}

		public void Shoot(Camera3D camera)
		{
			// if (!this.fireRateTimer!.IsStopped() || !this.reloadTimer!.IsStopped())
			// {
			// 	return;
			// }

			// Logger.Print($"Shooting");
			// this.muzzleFlash!.Restart();
			// this.fireRateTimer!.Start();
			// var res = this.hitscan!.Shoot(camera, this.resource!.Range);

			// if (res is not null)
			// {
			// 	this.sparkController!.Create(res.Position);

			// 	if (res.Collider is IWithHealth collider)
			// 	{
			// 		collider.Health.Damage(this.resource!.Damage);
			// 	}
			// }

			// Logger.Print($"Hit something? res: {res}.");
		}

		public void Reload()
		{
			if (!this.reloadTimer!.IsStopped())
			{
				return;
			}

			this.reloadTimer.Start();
			Logger.Print("Reloading");
			this.Visible = false;
		}

		public void OnReloadTimerTimeout()
		{
			Logger.Print("Finish Reload");
			this.Visible = true;
		}
	}
}
