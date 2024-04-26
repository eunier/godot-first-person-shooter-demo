namespace App.Modules.Weapons
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using App.Shared;
	using Godot;

	public partial class WeaponSystem : Node3D
	{
		private readonly Dictionary<WeaponType, Node3D> weapons = new();
		private KeyValuePair<WeaponType, Node3D> currentWeapon;

		public override void _Ready()
		{
			this.GetChildrenWeapons();
			this.Equip(WeaponType.Rifle);
		}

		public override void _PhysicsProcess(double delta)
		{
			if (Input.IsActionPressed("shoot"))
			{
				this.Shoot();
			}
		}

		private void GetChildrenWeapons()
		{
			var children = this.GetChildren();

			var allWeaponsTypes = new WeaponType[]
			{
				WeaponType.Rifle,
				WeaponType.Cannon,
			};

			if (children.Count != allWeaponsTypes.Length)
			{
				var msg = "Missing weapon(s).";
				Logger.PrintErr(msg);
				throw new Exception(msg);
			}

			foreach (var child in children)
			{
				if (child is not Node3D)
				{
					var msg = "child is not a `Godot.Node3D`.";
					Logger.PrintErr(msg);
					throw new Exception(msg);
				}
			}

			var zip = children.Zip(allWeaponsTypes);

			foreach (var (child, weaponType) in zip)
			{
				this.weapons.Add(weaponType, (Node3D)child);
			}
		}

		private void Equip(WeaponType weaponType)
		{
			foreach (var kvp in this.weapons)
			{
				var (k, v) = kvp;

				if (k == weaponType)
				{
					this.currentWeapon = kvp;
					v.Visible = true;
					v.SetProcess(true);
				}
				else
				{
					v.Visible = false;
					v.SetProcess(false);
				}
			}
		}

		private void Shoot()
		{
			switch (this.currentWeapon.Key)
			{
				case WeaponType.Rifle:
					((Rifle)this.currentWeapon.Value).Shoot();
					break;

				case WeaponType.Cannon:
					((Cannon)this.currentWeapon.Value).Shoot();
					break;

				default:
					break;
			}
		}
	}
}
