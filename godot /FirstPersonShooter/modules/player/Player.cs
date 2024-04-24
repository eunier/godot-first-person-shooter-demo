using Godot;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	[Export]
	private float _jumpHeight;

	[Export]
	private float _fallMultiplier;

	[Export]
	private float _maxHitpoints;

	private Vector2 _mouseMotion = Vector2.Zero;

	public float gravity = ProjectSettings
		.GetSetting("physics/3d/default_gravity")
		.AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = this.Velocity;
		this.ApplyGravity(delta, ref velocity);
		this.HandleJump(ref velocity);
		this.HandleMovement(ref velocity);
		this.Velocity = velocity;
		_ = this.MoveAndSlide();
	}

	private void ApplyGravity(double delta, ref Vector3 velocity)
	{
		if (!this.IsOnFloor())
		{
			velocity.Y -= this.gravity * (float)delta;
		}
	}

	private void HandleJump(ref Vector3 velocity)
	{
		if (Input.IsActionJustPressed(action: "ui_accept") && this.IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
	}

	private void HandleMovement(ref Vector3 velocity)
	{
		var inputDir = Input.GetVector(
			"move_left",
			"move_right",
			"move_forward",
			"move_backward"
		);

		var direction = (
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
	}
}
