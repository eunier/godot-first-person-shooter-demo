namespace App.Modules.WeaponControllerModule
{
	using System.Collections.Generic;
	using System.Linq;
	using App.Modules.CameraModule;
	using App.Modules.HealthModule;
	using App.Modules.ProjectileModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class WeaponController : Node3D
	{
		private const string FireRateTimerNodePath = "%FireRateTimer";
		private const string ReloadTimerNodePath = "%ReloadTimer";
		private readonly List<Node3D> weaponNodes = new();
		private readonly List<int> weaponMagazines = new();
		private readonly List<int> weaponMagazinesAmmo = new();

		[Export]
		private Camera3D? camera;

		[Export]
		private WeaponResource[]? weaponResources;

		private int currentWeaponIndex = 0;
		private Timer? fireRateTimer;
		private Timer? reloadTimer;

		private WeaponResource CurrentWeaponResource
		{
			get => this.weaponResources![this.currentWeaponIndex];
		}

		private Node3D CurrentWeaponNode
		{
			get => this.weaponNodes[this.currentWeaponIndex];
		}

		private int WeaponCount
		{
			get => this.weaponResources!.Length;
		}

		private int CurrentWeaponMagazines
		{
			get => this.weaponMagazines[this.currentWeaponIndex];
			set => this.weaponMagazines[this.currentWeaponIndex] = value;
		}
		private int CurrentWeaponCurrentMagazineAmmo
		{
			get => this.weaponMagazinesAmmo[this.currentWeaponIndex];
			set => this.weaponMagazinesAmmo[this.currentWeaponIndex] = value;
		}

		public override void _Ready()
		{
			this.fireRateTimer = this.GetNode<Timer>(
				WeaponController.FireRateTimerNodePath
			);

			this.reloadTimer = this.GetNode<Timer>(
				WeaponController.ReloadTimerNodePath
			);

			foreach (var weaponResource in this.weaponResources!)
			{
				var weapon = (Node3D)weaponResource.Scene!.Instantiate();
				weapon.Visible = false;
				weapon.SetProcess(false);
				this.weaponNodes.Add(weapon);
				this.weaponMagazines.Add(weaponResource.Magazines);
				this.weaponMagazinesAmmo.Add(weaponResource.MagazineSize);
				this.AddChild(weapon);
			}

			this.EquipWeapon(this.currentWeaponIndex);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (Input.MouseMode != Input.MouseModeEnum.Captured)
			{
				return;
			}

			if (
				@event.IsActionPressed(App.Modules.GlobalConstants.InputMap.NextWeapon)
			)
			{
				this.EquipNextWeapon();
			}

			if (
				@event.IsActionPressed(
					App.Modules.GlobalConstants.InputMap.PreviousWeapon
				)
			)
			{
				this.EquipPreviousWeapon();
			}
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionPressed(App.Modules.GlobalConstants.InputMap.Shoot))
			{
				this.Shoot();
			}

			if (Input.IsActionPressed(GlobalConstants.InputMap.Reload))
			{
				this.Reload();
			}
		}

		public void OnReloadTimerTimeout()
		{
			this.CurrentWeaponNode.Visible = true;
			this.CurrentWeaponMagazines--;

			this.CurrentWeaponCurrentMagazineAmmo =
				this.CurrentWeaponResource.MagazineSize;

			Logger.Print("Finish Reload.");
			Logger.Print($"Current magazines count: {this.CurrentWeaponMagazines}.");
		}

		private void EquipWeapon(int index)
		{
			for (int i = 0; i < this.WeaponCount; i++)
			{
				var weaponNode = this.weaponNodes[i];

				if (index == i)
				{
					Logger.Print($"Unequipped weapon {this.CurrentWeaponResource.Name}.");

					weaponNode.Visible = true;
					weaponNode.SetProcess(true);

					this.fireRateTimer!.WaitTime =
						this.CurrentWeaponResource.FireRateWaitTime;

					this.reloadTimer!.WaitTime = this.CurrentWeaponResource.ReloadTime;

					Logger.Print($"Equipped weapon {this.CurrentWeaponResource.Name}.");
				}
				else
				{
					weaponNode.Visible = false;
					weaponNode.SetProcess(false);
				}
			}

			this.currentWeaponIndex = index;
		}

		private void EquipNextWeapon()
		{
			var nextWeaponIndex = Mathf.Wrap(
				this.currentWeaponIndex + 1,
				0,
				this.WeaponCount
			);

			// var nextWeaponEnum = (WeaponEnum)nextWeaponIndex;
			// Logger.Print($"Equipping preview weapon: {nextWeaponEnum}.");
			this.EquipWeapon(nextWeaponIndex);
		}

		private void EquipPreviousWeapon()
		{
			var nextWeaponIndex = Mathf.Wrap(
				this.currentWeaponIndex - 1,
				0,
				this.WeaponCount
			);

			this.EquipWeapon(nextWeaponIndex);
		}

		private void Shoot()
		{
			switch (this.CurrentWeaponResource.ProjectileEnum)
			{
				case WeaponProjectileEnum.Hitscan:
					this.HandleHitscanShoot();
					break;

				case WeaponProjectileEnum.Physical:
					this.HandlePhysicalProjectileShoot();
					break;

				default:
					break;
			}
		}

		private void HandleHitscanShoot()
		{
			if (
				!this.fireRateTimer!.IsStopped()
				|| !this.reloadTimer!.IsStopped()
				|| this.CurrentWeaponCurrentMagazineAmmo == 0
			)
			{
				return;
			}

			this.CreateShootMuzzleEffect();
			this.fireRateTimer!.Start();
			this.CurrentWeaponCurrentMagazineAmmo--;

			Logger.Print("Shoot hitscan weapon.");

			Logger.Print(
				$"Current Ammo: {this.CurrentWeaponCurrentMagazineAmmo} / {this.CurrentWeaponMagazines * this.CurrentWeaponResource.MagazineSize}  | {this.CurrentWeaponMagazines} / {this.CurrentWeaponResource.MagazineSize}."
			);

			var res = Camera.GetCameraRayInterception(
				this,
				this.CurrentWeaponResource.Range
			);

			if (res.Collider is not null)
			{
				this.CreateHitEffect(res.Position);

				if (res.Collider is IWithHealth collider)
				{
					collider.Health.Damage(this.CurrentWeaponResource.Damage);

					Logger.Print(
						$"Damage {collider} by {this.CurrentWeaponResource.Damage} hitpoints."
					);
				}
			}
		}

		private void HandlePhysicalProjectileShoot()
		{
			if (
				!this.fireRateTimer!.IsStopped()
				|| !this.reloadTimer!.IsStopped()
				|| this.CurrentWeaponCurrentMagazineAmmo == 0
			)
			{
				return;
			}

			this.CreateShootMuzzleEffect();
			this.fireRateTimer!.Start();

			this.CurrentWeaponCurrentMagazineAmmo--;

			Logger.Print("Shoot hitscan weapon.");

			Logger.Print(
				$"Current Ammo: {this.CurrentWeaponCurrentMagazineAmmo} / {this.CurrentWeaponMagazines * this.CurrentWeaponResource.MagazineSize}  | {this.CurrentWeaponMagazines} / {this.CurrentWeaponResource.MagazineSize}."
			);

			var cameraRayInterception = Camera.GetCameraRayInterception(
				this,
				this.CurrentWeaponResource.Range
			);

			var projectilePoint = this.GetNode<Node3D>(
				$"{this.CurrentWeaponNode.GetPath()}/{this.CurrentWeaponResource.ProjectilePointNodePath}"
			);

			var direction = (
				cameraRayInterception.Position - projectilePoint.GlobalTransform.Origin
			).Normalized();

			Logger.Print(
				"Projectile Point Position: " + projectilePoint.GlobalTransform.Origin
			);
			Logger.Print(
				"Camera Ray Interception Position: " + cameraRayInterception.Position
			);

			var projectile =
				this.CurrentWeaponResource.ProjectileScene!.Instantiate<Projectile>();

			if (OS.IsDebugBuild())
			{
				var projectileScript = (Script)projectile.GetScript();

				if (projectileScript is null)
				{
					Logger.PushErr(
						$"No script attacked to script of projectile for weapon {this.CurrentWeaponResource.Name}."
					);
				}
			}

			projectilePoint.AddChild(projectile);
			projectile.LookAt(cameraRayInterception.Position, Vector3.Up);
			projectile.TopLevel = true;
			projectile.Damage = this.CurrentWeaponResource.Damage;
			projectile.Speed = this.CurrentWeaponResource.ProjectileSpeed;
		}

		private void OnProjectileBodyEntered(Node node)
		{
			Logger.Print(node.ToString());
		}

		private void Reload()
		{
			if (!this.reloadTimer!.IsStopped() || this.CurrentWeaponMagazines == 0)
			{
				return;
			}

			this.reloadTimer.Start();
			Logger.Print("Reloading...");
			this.CurrentWeaponNode.Visible = false;
		}

		private void CreateShootMuzzleEffect()
		{
			var muzzleFlashNode = this.GetNode<GpuParticles3D>(
				$"{this.CurrentWeaponNode.GetPath()}/{this.CurrentWeaponResource.MuzzleFashNodePath}"
			);

			muzzleFlashNode.Restart();
		}

		private void CreateHitEffect(Vector3 position)
		{
			var effectNode =
				this.CurrentWeaponResource.HitScene!.Instantiate<Node3D>();

			this.AddChild(effectNode);
			effectNode.GlobalPosition = position;
		}
	}
}
