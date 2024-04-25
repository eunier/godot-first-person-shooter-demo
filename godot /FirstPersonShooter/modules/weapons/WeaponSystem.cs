using System;
using System.Linq;
using Godot;

public partial class WeaponSystem : Node3D
{
	private readonly Godot.Collections.Dictionary<WeaponType, Node3D> weapons =
		new();

	private System.Collections.Generic.KeyValuePair<
		WeaponType,
		Node3D
	> currentWeapon;

	public override void _Ready()
	{
		this.GetChildrenWeapons();
		this.Equip(WeaponType.Rifle);
	}

	public override void _Process(double delta) { }

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
			GD.PrintErr(msg);
			throw new Exception(msg);
		}

		foreach (var child in children)
		{
			if (child is not Node3D)
			{
				var msg = "child is not a Node3D.";
				GD.PrintErr(msg);
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
}
