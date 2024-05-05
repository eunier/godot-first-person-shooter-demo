namespace App.Modules.WeaponModule
{
	using Godot;

	public abstract partial class Weapon : Node3D, IWeapon // TODO: abstract?
	{
		public static GpuParticles3D GetMuzzleFlashNode(
			Node3D caller,
			Node3D weaponNode,
			WeaponResource weaponResource
		)
		{
			return caller.GetNode<GpuParticles3D>(
				$"{weaponNode!.GetPath()}/{weaponResource!.MuzzleFashNodePath}"
			);
		}

		public static Node3D GetProjectilePointNode(
			Node3D caller,
			Node3D weaponNode,
			WeaponResource weaponResource
		)
		{
			return caller.GetNode<Node3D>(
				$"{weaponNode!.GetPath()}/{weaponResource!.ProjectilePointNodePath}"
			);
		}
	}
}
