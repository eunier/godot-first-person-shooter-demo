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
		// private const string RifleNodePath = "Rifle";
		// private const string CannonNodePath = "Cannon";
		private const string FireRateTimerNodePath = "%FireRateTimer";
		private const string ReloadTimerNodePath = "%ReloadTimer";

		// private readonly Dictionary<WeaponEnum, Weapon> weapons = new();

		private readonly List<Node3D> weaponNodes = new();
		private readonly List<int> weaponMagazines = new();
		private readonly List<int> weaponMagazinesAmmo = new();

		[Export]
		private Camera3D? camera;

		[Export]
		private WeaponResource[]? weaponResources;

		private int currentWeaponIndex = 0;
		private WeaponResource? currentWeaponResource; // TODO use setter and currentWeaponIndex
		private Node3D? currentWeaponNode;
		private Timer? fireRateTimer;
		private Timer? reloadTimer;

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

		private int WeaponCount
		{
			get => this.weaponResources!.Length;
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

			this.currentWeaponNode = this.weaponNodes.First(); // TODO do i need this is the is  a EquipWeapon call below?
			this.currentWeaponResource = this.weaponResources.First(); // TODO do i need this is the is  a EquipWeapon call below?
			this.EquipWeapon(this.currentWeaponIndex);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			// TODO where should this be.
			if (
				@event.IsActionPressed(App.Modules.GlobalConstants.InputMap.NextWeapon)
				&& Input.MouseMode == Input.MouseModeEnum.Captured
			)
			{
				this.EquipNextWeapon();
			}

			if (
				@event.IsActionPressed(
					App.Modules.GlobalConstants.InputMap.PreviousWeapon
				)
				&& Input.MouseMode == Input.MouseModeEnum.Captured
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
			this.currentWeaponNode!.Visible = true;
			this.CurrentWeaponMagazines--;

			this.CurrentWeaponCurrentMagazineAmmo =
				this.currentWeaponResource!.MagazineSize;

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
					Logger.Print(
						$"Unequipped weapon {this.currentWeaponResource!.Name}."
					);

					weaponNode.Visible = true;
					weaponNode.SetProcess(true);

					this.currentWeaponNode = weaponNode;
					this.currentWeaponResource = this.weaponResources![i];

					this.fireRateTimer!.WaitTime =
						this.currentWeaponResource.FireRateWaitTime;

					this.reloadTimer!.WaitTime = this.currentWeaponResource.ReloadTime;

					Logger.Print($"Equipped weapon {this.currentWeaponResource!.Name}.");
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
			switch (this.currentWeaponResource!.ProjectileEnum)
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
				$"Current Ammo: {this.CurrentWeaponCurrentMagazineAmmo} / {this.CurrentWeaponMagazines * this.currentWeaponResource!.MagazineSize}  | {this.CurrentWeaponMagazines} / {this.currentWeaponResource!.MagazineSize}."
			);

			var res = Camera.GetCameraRayInterception(
				this,
				this.currentWeaponResource!.Range
			);

			if (res.Collider is not null)
			{
				this.CreateHitEffect(res.Position);

				if (res.Collider is IWithHealth collider)
				{
					collider.Health.Damage(this.currentWeaponResource.Damage);

					Logger.Print(
						$"Damage {collider} by {this.currentWeaponResource.Damage} hitpoints."
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
				$"Current Ammo: {this.CurrentWeaponCurrentMagazineAmmo} / {this.CurrentWeaponMagazines * this.currentWeaponResource!.MagazineSize}  | {this.CurrentWeaponMagazines} / {this.currentWeaponResource!.MagazineSize}."
			);

			var cameraRayInterception = Camera.GetCameraRayInterception(
				this,
				this.currentWeaponResource!.Range
			);

			var projectilePoint = this.GetNode<Node3D>(
				$"{this.currentWeaponNode!.GetPath()}/{this.currentWeaponResource!.ProjectilePointNodePath}"
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
				this.currentWeaponResource!.ProjectileScene!.Instantiate<Projectile>();

			if (OS.IsDebugBuild())
			{
				var projectileScript = (Script)projectile.GetScript();

				if (projectileScript is null)
				{
					GD.PushError(
						$"No script attacked to script of projectile for weapon {this.currentWeaponResource.Name}." // TODO use node.name
					); // TODO: throw vs log error vs push error
				}
			}

			projectilePoint.AddChild(projectile);
			projectile.LookAt(cameraRayInterception.Position, Vector3.Up);
			projectile.TopLevel = true;
			projectile.Damage = this.currentWeaponResource.Damage;
			projectile.Speed = this.currentWeaponResource.ProjectileSpeed;
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
			this.currentWeaponNode!.Visible = false;
		}

		private void CreateShootMuzzleEffect()
		{
			var muzzleFlashNode = this.GetNode<GpuParticles3D>(
				$"{this.currentWeaponNode!.GetPath()}/{this.currentWeaponResource!.MuzzleFashNodePath}"
			);

			muzzleFlashNode.Restart();
		}

		private void CreateHitEffect(Vector3 position)
		{
			var effectNode =
				this.currentWeaponResource!.HitScene!.Instantiate<Node3D>();

			this.AddChild(effectNode);
			effectNode.GlobalPosition = position;
		}
	}
}
