namespace App.Modules.CannonProjectileModule
{
	using App.Modules.HealthModule;
	using Godot;

	public partial class CannonProjectile : RigidBody3D
	{
		public override void _Ready()
		{
			this.TopLevel = true;
		}

		public override void _Process(double delta) { }

		public void Shoot()
		{
			this.ApplyImpulse(this.Transform.Basis.Z, -this.Transform.Basis.Z);
		}

		public void OnArea3DBodyEntered(Node3D body)
		{
			if (
				body.IsInGroup(GlobalConstants.Groups.WithHealth)
				&& body is IWithHealth b
			)
			{
				b.Health.Damage(5); // TODO use resource for damage
			}

			this.QueueFree();
		}
	}
}
