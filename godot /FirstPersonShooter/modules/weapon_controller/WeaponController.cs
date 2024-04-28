namespace App.Modules.WeaponController
{
	using System.Collections.Generic;
	using App.Modules.Cannon;
	using App.Modules.Rifle;
	using App.Modules.WeaponController.Constants;
	using App.Shared.Abstract;
	using App.Shared.Enums;
	using App.Shared.Utils;
	using Godot;

	public partial class WeaponController : Node3D
	{
		private readonly Dictionary<WeaponEnum, Weapon> weapons = new();

		[Export]
		private Came? camera;
		private KeyValuePair<WeaponEnum, Weapon> currentWeapon;
		private Rifle rifle;
		private Cannon cannon;

		public override void _Ready()
		{
			this.rifle = this.GetNode<Rifle>(NodePaths.Rifle);
			this.cannon = this.GetNode<Cannon>(NodePaths.Cannon);
			this.weapons.Add(WeaponEnum.Rifle, this.rifle);
			this.weapons.Add(WeaponEnum.Cannon, this.cannon);
			this.EquipWeapon(WeaponEnum.Rifle);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed(Shared.Constants.InputMap.NextWeapon))
			{
				this.EquipNextWeapon();
			}

			if (@event.IsActionPressed(Shared.Constants.InputMap.PreviousWeapon))
			{
				this.EquipPreviousWeapon();
			}

			if (@event.IsActionPressed(Shared.Constants.InputMap.Shoot))
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
			Input.MouseMode = Input.MouseModeEnum.Visible;
			switch (this.currentWeapon.Value)
			{
				case Rifle rifle:
					rifle.Shoot(this.camera);
					break;

				default:
					break;
			}
		}
	}
}
