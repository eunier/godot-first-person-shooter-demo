namespace App.Modules.WeaponModule
{
	using Godot;

	[GlobalClass]
	public partial class WeaponResource : Resource
	{
		public WeaponResource()
			: this(0, 0, 0, 0, 0, 0) { }

		public WeaponResource(
			int damage,
			int range,
			int fireRatePerMinute,
			int magazineSize,
			int magazines,
			float reloadTime
		)
		{
			this.Damage = damage;
			this.Range = range;
			this.FireRatePerMinute = fireRatePerMinute;
			this.MagazineSize = magazineSize;
			this.Magazines = magazines;
			this.ReloadTime = reloadTime;
		}

		[Export]
		public int Damage { get; set; }

		[Export]
		public int Range { get; set; }

		[Export]
		public int FireRatePerMinute { get; set; }

		[Export]
		public int MagazineSize { get; set; }

		[Export]
		public int Magazines { get; set; }

		[Export]
		public float ReloadTime { get; set; }

		public float FireRateWaitTime
		{
			get => 60 / this.FireRatePerMinute;
			private set { }
		}

		public int MaxAmmo
		{
			get => this.MagazineSize * this.Magazines;
			private set { }
		}
	}
}
