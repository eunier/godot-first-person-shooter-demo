namespace App.Modules.ProjectileModule
{
	using App.Modules.HealthModule;
	using Godot;

	public partial class Projectile : RigidBody3D
	{
		public int Damage = 0;
		public int Speed = 0;

		public override void _Ready()
		{
			this.TopLevel = true;
		}

		public override void _PhysicsProcess(double delta)
		{
			this.ApplyImpulse(-this.Basis.Z, this.Basis.Z * this.Speed);
		}

		public void OnBodyEntered(Node body)
		{
			if (
				body.IsInGroup(GlobalConstants.Groups.WithHealth)
				&& body is IWithHealth b
			)
			{
				b.Health.Damage(this.Damage);
			}

			this.QueueFree();
		}
	}
}
