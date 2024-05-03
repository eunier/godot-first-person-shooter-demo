namespace App.Modules.WeaponModule
{
	using Godot;

	[GlobalClass]
	public partial class WeaponResource : Resource
	{
		public WeaponResource()
			: this(0, 0, 0) { }

		public WeaponResource(int damage, int range, int fireRatePerMinute)
		{
			this.Damage = damage;
			this.Range = range;
			this.FireRatePerMinute = fireRatePerMinute;
		}

		[Export]
		public int Damage { get; set; }

		[Export]
		public int Range { get; set; }

		[Export]
		public float FireRatePerMinute { get; set; }

		public float FireRateWaitTime
		{
			get => 60 / this.FireRatePerMinute;
			private set { }
		}
	}
}
