namespace App.Modules.FpsController
{
	using Godot;

	public partial class FpsController : Node3D
	{
		[Export]
		private CharacterBody3D? characterBody;

		[Export]
		private Node3D? cameraPivot;

		[Export]
		private float speed = 5.0f;

		[Export]
		private float jumpHeight = 1f;

		[Export]
		private float fallMultiplier = 1.5f;

		[Export]
		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();

		private Vector2 mouseMotion = Vector2.Zero;

		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

		public override void _Input(InputEvent @event)
		{
			if (
				@event is InputEventMouseButton
				&& Input.MouseMode is not Input.MouseModeEnum.Captured
			)
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}

			if (
				@event is InputEventMouseMotion mouseMotionInputEvent
				&& Input.MouseMode == Input.MouseModeEnum.Captured
			)
			{
				this.mouseMotion = -mouseMotionInputEvent.Relative * 0.003f;
			}

			if (@event.IsActionPressed(App.Modules.GlobalConstants.InputMap.UiCancel))
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			var velocity = this.characterBody!.Velocity;
			this.ProcessCameraRotation();
			this.ProcessGravity(delta, ref velocity);
			this.ProcessJump(ref velocity);
			this.ProcessMovement(ref velocity);
			this.characterBody.Velocity = velocity;
			this.characterBody.MoveAndSlide();
		}

		private void ProcessGravity(double delta, ref Vector3 velocity)
		{
			if (!this.characterBody!.IsOnFloor())
			{
				if (velocity.Y >= 0)
				{
					velocity.Y -= this.gravity * (float)delta;
				}
				else
				{
					velocity.Y -= this.gravity * (float)delta * this.fallMultiplier;
				}
			}
		}

		private void ProcessMovement(ref Vector3 velocity)
		{
			var inputDir = Input.GetVector(
				GlobalConstants.InputMap.MoveLeft,
				GlobalConstants.InputMap.MoveRight,
				GlobalConstants.InputMap.MoveForward,
				GlobalConstants.InputMap.MoveBackward
			);

			var direction = (
				this.characterBody!.Transform.Basis
				* new Vector3(inputDir.X, 0, inputDir.Y)
			).Normalized();

			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * this.speed;
				velocity.Z = direction.Z * this.speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(
					this.characterBody!.Velocity.X,
					0,
					this.speed
				);

				velocity.Z = Mathf.MoveToward(
					this.characterBody!.Velocity.Z,
					0,
					this.speed
				);
			}
		}

		private void ProcessJump(ref Vector3 velocity)
		{
			if (
				Input.IsActionJustPressed(
					action: App.Modules.GlobalConstants.InputMap.Jump
				) && this.characterBody!.IsOnFloor()
			)
			{
				velocity.Y = Mathf.Sqrt(this.jumpHeight * 2 * this.gravity);
			}
		}

		private void ProcessCameraRotation()
		{
			this.characterBody!.RotateY(this.mouseMotion.X);

			if (this.cameraPivot is not null)
			{
				this.cameraPivot.RotateX(this.mouseMotion.Y);

				this.cameraPivot.RotationDegrees = new Vector3(
					(float)Mathf.Clamp(this.cameraPivot.RotationDegrees.X, -90.0, 90.0),
					this.cameraPivot.RotationDegrees.Y,
					this.cameraPivot.RotationDegrees.Z
				);
			}

			this.mouseMotion = Vector2.Zero;
		}
	}
}
