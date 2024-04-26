namespace App.Modules.Weapons.Resource
{
	using Godot;

	[GlobalClass]
	public partial class WeaponResource : Resource
	{
		[Export]
		public int Damage { get; set; }

		[Export]
		public int Range { get; set; }
	}
}
