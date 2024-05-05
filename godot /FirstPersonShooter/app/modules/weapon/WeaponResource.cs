namespace App.Modules.WeaponModule
{
	using Godot;

	[GlobalClass]
	public partial class WeaponResource : Resource
	{
		public WeaponResource()
			: this(
				0,
				0,
				0,
				0,
				0,
				0,
				null,
				null,
				null,
				null,
				string.Empty,
				WeaponProjectileEnum.Hitscan
			) { }

		public WeaponResource(
			float fireRatePerMinute,
			float reloadTime,
			int damage,
			int magazines,
			int magazineSize,
			int range,
			NodePath? muzzleFashNodePath,
			PackedScene? hitScene,
			PackedScene? projectileScene,
			PackedScene? scene,
			string name,
			WeaponProjectileEnum projectileEnum
		)
		{
			this.Damage = damage;
			this.FireRatePerMinute = fireRatePerMinute;
			this.HitScene = hitScene;
			this.Magazines = magazines;
			this.MagazineSize = magazineSize;
			this.MuzzleFashNodePath = muzzleFashNodePath;
			this.Name = name;
			this.ProjectileEnum = projectileEnum;
			this.ProjectileScene = projectileScene;
			this.Range = range;
			this.ReloadTime = reloadTime;
			this.Scene = scene;
		}

		[Export]
		public string Name { get; set; }

		[Export]
		public PackedScene? Scene { get; set; }

		[Export]
		public WeaponProjectileEnum ProjectileEnum { get; set; }

		/// <summary>
		/// 	 Gets or sets `PackedScene`.
		/// 	 Node type is `GpuParticles3D`.
		/// </summary>
		[Export]
		public NodePath? MuzzleFashNodePath { get; set; }

		/// <summary>
		/// 	 Gets or sets `NodePath`.
		/// 	 Node type is `Node3D`.
		/// </summary>
		[Export(PropertyHint.None, "Only for WeaponProjectileEnum.Physical.")]
		public NodePath? ProjectilePointNodePath { get; set; }

		[Export]
		public PackedScene? HitScene { get; set; }

		/// <summary>
		/// 	 Gets or sets `PackedScene`.
		/// 	 Node type is `Projectile`.
		/// </summary>
		[Export]
		public PackedScene? ProjectileScene { get; set; }

		[ExportGroup("Stats")]
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
		public int ProjectileSpeed { get; set; }

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
