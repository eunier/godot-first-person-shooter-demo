namespace App.Shared.Resources
{
	using Godot;

	[GlobalClass]
	public partial class WeaponResource : Resource
	{
		public WeaponResource()
			: this(0, 0) { }

		public WeaponResource(int damage, int range)
		{
			this.Damage = damage;
			this.Range = range;
		}

		public int Damage { get; set; }
		public int Range { get; set; }
	}
}
