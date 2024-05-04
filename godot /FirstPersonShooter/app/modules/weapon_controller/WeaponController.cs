namespace App.Modules.WeaponControllerModule
{
	using System.Collections.Generic;
	using App.Modules.CannonModule;
	using App.Modules.RifleModule;
	using App.Modules.WeaponModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class WeaponController : Node3D
	{
		private const string RifleNodePath = "Rifle";
		private const string CannonNodePath = "Cannon";
		private readonly Dictionary<WeaponEnum, Weapon> weapons = new();

		[Export]
		private Camera3D? camera;
		private KeyValuePair<WeaponEnum, Weapon> currentWeapon;
		private Rifle? rifle;
		private Cannon? cannon;

		public override void _Ready()
		{
			this.rifle = this.GetNode<Rifle>(WeaponController.RifleNodePath);
			this.cannon = this.GetNode<Cannon>(WeaponController.CannonNodePath);
			this.weapons.Add(WeaponEnum.Rifle, this.rifle);
			this.weapons.Add(WeaponEnum.Cannon, this.cannon);
			this.EquipWeapon(WeaponEnum.Rifle);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
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

		private void EquipWeapon(WeaponEnum weaponEnum)
		{
			Logger.Print($"Equipping {weaponEnum}");

			foreach (var kvp in this.weapons)
			{
				var (k, v) = kvp;

				if (k == weaponEnum)
				{
					this.currentWeapon = kvp;
					v.Visible = true;
					v.SetProcess(true);
					Logger.Print($"Equipped {k}.");
				}
				else
				{
					v.Visible = false;
					v.SetProcess(false);
					Logger.Print($"Unequipped {k}.");
				}
			}
		}

		private void EquipNextWeapon()
		{
			var nextWeaponIndex = Mathf.Wrap(
				(int)this.currentWeapon.Key + 1,
				0,
				this.weapons.Count
			);

			var nextWeaponEnum = (WeaponEnum)nextWeaponIndex;
			Logger.Print($"Equipping preview weapon: {nextWeaponEnum}.");
			this.EquipWeapon((WeaponEnum)nextWeaponIndex);
		}

		private void EquipPreviousWeapon()
		{
			var nextWeaponIndex = Mathf.Wrap(
				(int)this.currentWeapon.Key - 1,
				0,
				this.weapons.Count
			);

			var previousWeaponEnum = (WeaponEnum)nextWeaponIndex;
			Logger.Print($"Equipping preview weapon: {previousWeaponEnum}.");
			this.EquipWeapon((WeaponEnum)nextWeaponIndex);
		}

		private void Shoot()
		{
			switch (this.currentWeapon.Value)
			{
				case Rifle rifle:
					rifle.Shoot(this.camera!);
					break;

				case Cannon cannon:
					cannon.Shoot(this.camera!);
					break;

				default:
					break;
			}
		}

		private void Reload()
		{
			switch (this.currentWeapon.Value)
			{
				case Rifle rifle:
					rifle.Reload();
					break;

				case Cannon cannon:
					cannon.Reload();
					break;

				default:
					break;
			}
		}
	}
}
