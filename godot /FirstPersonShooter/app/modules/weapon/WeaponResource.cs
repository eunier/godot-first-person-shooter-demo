namespace App.Modules.WeaponModule
{
	using Godot;

	[GlobalClass]
	public partial class WeaponResource : Resource
	{
		public WeaponResource()
			: this(string.Empty, null, 0, 0, 0, 0, 0, 0, WeaponProjectileEnum.Hitscan)
		{ }

		public WeaponResource(
			string name,
			PackedScene? scene,
			int damage,
			int range,
			float fireRatePerMinute,
			int magazineSize,
			int magazines,
			float reloadTime,
			WeaponProjectileEnum projectileEnum
		)
		{
			this.Name = name;
			this.Scene = scene;
			this.Damage = damage;
			this.ReloadTime = reloadTime;
			this.Range = range;
			this.FireRatePerMinute = fireRatePerMinute;
			this.MagazineSize = magazineSize;
			this.Magazines = magazines;
			this.ProjectileEnum = projectileEnum;
		}

		[Export]
		public string Name { get; set; }

		[Export]
		public PackedScene? Scene { get; set; }

		[Export]
		public int Damage { get; set; }

		[Export]
		public int Range { get; set; }

		[Export]
		public float FireRatePerMinute { get; set; }

		[Export]
		public int MagazineSize { get; set; }

		[Export]
		public int Magazines { get; set; }

		[Export]
		public float ReloadTime { get; set; }

		[Export]
		public WeaponProjectileEnum ProjectileEnum { get; set; }

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
