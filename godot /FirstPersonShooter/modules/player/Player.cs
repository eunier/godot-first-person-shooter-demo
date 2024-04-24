using Godot;

public partial class Player : CharacterBody3D
{
	private const float FallMultiplier = 1.5f;
	private const float JumpHeight = 1f;
	private const float JumpVelocity = 4.5f;
	private const float MaxHitpoints = 100f;
	private const float Speed = 5.0f;
	private Node3D cameraPivot;
	private Vector2 mouseMotion = Vector2.Zero;
	private float gravity = ProjectSettings
		.GetSetting("physics/3d/default_gravity")
		.AsSingle();
	private float hitpoints;
	private float Hitpoints
	{
		get { return this.hitpoints; }
		set
		{
			this.hitpoints = value;
			GD.Print(this.hitpoints);

			if (this.hitpoints <= 0)
			{
				this.GetTree().Quit();
			}
		}
	}

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
		this.MoveAndSlide();
	}

	public override void _Input(InputEvent inputEvent)
	{
		base._Input(inputEvent);

		if (
			inputEvent is InputEventMouseMotion mouseMotionInputEvent
			&& Input.MouseMode == Input.MouseModeEnum.Captured
		)
		{
			this.mouseMotion = -mouseMotionInputEvent.Relative * 0.003f;
		}

		if (inputEvent.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	private void ApplyGravity(double delta, ref Vector3 velocity)
	{
		if (!this.IsOnFloor())
		{
			if (velocity.Y >= 0)
			{
				velocity.Y -= this.gravity * (float)delta;
			}
			else
			{
				velocity.Y -= this.gravity * (float)delta * Player.FallMultiplier;
			}
		}
	}

	private void HandleJump(ref Vector3 velocity)
	{
		if (Input.IsActionJustPressed(action: "jump") && this.IsOnFloor())
		{
			velocity.Y = Player.JumpVelocity;
			velocity.Y = Mathf.Sqrt(Player.JumpHeight * 2 * this.gravity);
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
