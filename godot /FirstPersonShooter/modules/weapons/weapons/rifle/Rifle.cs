namespace App.Modules.Weapons
{
	using Godot;

	public partial class Rifle : Weapon
	{
		private GpuParticles3D muzzleFlash;

		public override void _Ready()
		{
			this.muzzleFlash = this.GetNode<GpuParticles3D>("MuzzleFlash");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta) { }

		public void Shoot()
		{
			this.muzzleFlash.Restart();
		}
	}
}
