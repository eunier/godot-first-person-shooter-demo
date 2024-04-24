using Godot;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = this.Velocity;

		// Add the gravity.
		if (!this.IsOnFloor())
		{
			velocity.Y -= this.gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && this.IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		var inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		Vector3 direction = (
			this.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)
		).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(this.Velocity.X, 0, Player.Speed);
			velocity.Z = Mathf.MoveToward(this.Velocity.Z, 0, Player.Speed);
		}

		this.Velocity = velocity;
		_ = this.MoveAndSlide();
	}

	// private float ApplyGravity(float delta, Vector3 velocity)
	// {
	// 	if (!this.IsOnFloor())
	// 	{
	// 		velocity.Y -= this.gravity * (float)delta;
	// 	}

	// 	return 0.0
	// }
}
