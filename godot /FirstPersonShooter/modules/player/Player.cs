using Godot;

public partial class Player : CharacterBody3D
{
	private const float FALL_MULTIPLIER = 1.5f;
	private const float JUMP_HEIGHT = 1f;
	private const float JUMP_VELOCITY = 4.5f;
	private const float MAX_HITPOINTS = 100f;
	private const float SPEED = 5.0f;
	private Node3D cameraPivot;
	private Vector2 mouseMotion = Vector2.Zero;

	private float _gravity = ProjectSettings
		.GetSetting("physics/3d/default_gravity")
		.AsSingle();

	// TODO: var hitpoints

	public override void _Ready()
	{
		base._Ready();
		Input.MouseMode = Input.MouseModeEnum.Captured;
		this.cameraPivot = this.GetNode<Node3D>("CameraPivot");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = this.Velocity;
		this.HandleCameraRotation();
		this.ApplyGravity(delta, ref velocity);
		this.HandleJump(ref velocity);
		this.HandleMovement(ref velocity);
		this.Velocity = velocity;
		_ = this.MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (
			@event is InputEventMouseMotion mouseMotionInputEvent
			&& Input.MouseMode == Input.MouseModeEnum.Captured
		)
		{
			this.mouseMotion = -mouseMotionInputEvent.Relative * 0.003f;
		}
	}

	private void ApplyGravity(double delta, ref Vector3 velocity)
	{
		if (!this.IsOnFloor())
		{
			if (velocity.Y >= 0)
			{
				velocity.Y -= this._gravity * (float)delta;
			}
			else
			{
				velocity.Y -= this._gravity * (float)delta * Player.FALL_MULTIPLIER;
			}
		}
	}

	private void HandleJump(ref Vector3 velocity)
	{
		if (Input.IsActionJustPressed(action: "jump") && this.IsOnFloor())
		{
			velocity.Y = Player.JUMP_VELOCITY;
			velocity.Y = Mathf.Sqrt(Player.JUMP_HEIGHT * 2 * this._gravity);
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
			velocity.X = direction.X * SPEED;
			velocity.Z = direction.Z * SPEED;
		}
		else
		{
			velocity.X = Mathf.MoveToward(this.Velocity.X, 0, Player.SPEED);
			velocity.Z = Mathf.MoveToward(this.Velocity.Z, 0, Player.SPEED);
		}
	}

	private void HandleCameraRotation()
	{
		this.RotateY(this.mouseMotion.X);
		this.cameraPivot.RotateX(this.mouseMotion.Y);

		this.cameraPivot.RotationDegrees = new Vector3(
			(float)Mathf.Clamp(this.cameraPivot.RotationDegrees.X, -90.0, 90.0),
			this.cameraPivot.RotationDegrees.Y,
			this.cameraPivot.RotationDegrees.Z
		);

		this.mouseMotion = Vector2.Zero;
	}
}
