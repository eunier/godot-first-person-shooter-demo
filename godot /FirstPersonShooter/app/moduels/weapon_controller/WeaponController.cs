namespace App.Modules.WeaponControllerModule
{
	using System.Collections.Generic;
	using App.Modules.CannonModule;
	using App.Modules.GlobalConstantsModule;
	using App.Modules.RifleModule;
	using App.Modules.Utils.LoggerModule;
	using App.Modules.WeaponModule;
	using Godot;

	public partial class WeaponController : Node3D
	{
		private readonly Dictionary<WeaponEnum, Node3D> weapons = new();

		[Export]
		private Camera3D? camera;
		private KeyValuePair<WeaponEnum, Node3D> currentWeapon;
		private Rifle? rifle;
		private Cannon? cannon;

		public override void _Ready()
		{
			this.rifle = this.GetNode<Rifle>(
				WeaponControllerConstants.NodePaths.Rifle
			);

			this.cannon = this.GetNode<Cannon>(
				WeaponControllerConstants.NodePaths.Cannon
			);

			this.weapons.Add(WeaponEnum.Rifle, this.rifle);
			this.weapons.Add(WeaponEnum.Cannon, this.cannon);
			this.EquipWeapon(WeaponEnum.Rifle);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed(GlobalConstants.InputMap.NextWeapon))
			{
				this.EquipNextWeapon();
			}

			if (@event.IsActionPressed(GlobalConstants.InputMap.PreviousWeapon))
			{
				this.EquipPreviousWeapon();
			}

			if (@event.IsActionPressed(GlobalConstants.InputMap.Shoot))
			{
				this.Shoot();
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
			switch (this.currentWeapon.Key)
			{
				case WeaponEnum.Rifle:
					this.rifle!.Shoot(this.camera!);
					break;

				default:
					break;
			}
		}
	}
}
