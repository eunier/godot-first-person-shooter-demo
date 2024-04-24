using System;
using Godot;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	[Export]
	private float _jumpHeight = 1;

	[Export]
	private double _fallMultiplier = 1.5;

	[Export]
	private float _maxHitpoints = 100;

	[Export]
	private Node3D _cameraPivot;

	private Vector2 _mouseMotion = Vector2.Zero;

	public float gravity = ProjectSettings
		.GetSetting("physics/3d/default_gravity")
		.AsSingle();

	public override void _Ready()
	{
		base._Ready();
		Input.MouseMode = Input.MouseModeEnum.Captured;
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

	public override void _Input(InputEvent inputEvent)
	{
		base._Input(inputEvent);

		if (
			inputEvent is InputEventMouseMotion motion
			&& Input.MouseMode == Input.MouseModeEnum.Captured
		)
		{
			this._mouseMotion = -motion.Relative * 0.003f;
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
				velocity.Y -= this.gravity * (float)delta * (float)this._fallMultiplier;
			}
		}
	}

	private void HandleJump(ref Vector3 velocity)
	{
		if (Input.IsActionJustPressed(action: "jump") && this.IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			velocity.Y = Mathf.Sqrt(this._jumpHeight * 2 * this.gravity);
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
		this.RotateY(this._mouseMotion.X);
		this._cameraPivot.RotateX(this._mouseMotion.Y);

		this._cameraPivot.RotationDegrees = new Vector3(
			(float)Mathf.Clamp(this._cameraPivot.RotationDegrees.X, -90.0, 90.0),
			this._cameraPivot.RotationDegrees.Y,
			this._cameraPivot.RotationDegrees.Z
		);

		this._mouseMotion = Vector2.Zero;
	}
}
