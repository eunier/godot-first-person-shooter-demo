namespace App.Modules.PlayerModule
{
	using App.Global;
	using App.Shared;
	using App.Shared.Utils;
	using Godot;

	public partial class Player : CharacterBody3D
	{
		private const float FallMultiplier = 1.5f;
		private const float JumpHeight = 1f;
		private const float JumpVelocity = 4.5f;
		private const float Speed = 5.0f;
		private const float MaxHitpoints = 100;
		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();
		private Node3D? cameraPivot;
		private Camera3D? camera;
		private Vector2 mouseMotion = Vector2.Zero;
		private GlobalState? globalState;
		private Label? debugLabel1;

		private float hitpoints = Player.MaxHitpoints;
		public float Hitpoints
		{
			get { return this.hitpoints; }
			set
			{
				this.hitpoints = value;
				Logger.Print(this.hitpoints.ToString());

				if (this.hitpoints <= 0)
				{
					this.GetTree().Quit();
				}
			}
		}

		public static Player? GetPlayer(Node caller)
		{
			var player = (Player?)
				caller.GetTree().GetFirstNodeInGroup(Constants.Groups.Player);

			if (player is null)
			{
				Logger.Print($"No node found in group '{Constants.Groups.Player}'.");
				return null;
			}
			else
			{
				Logger.Print($"Found player: {player}.");
			}

			return player;
		}

		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;

			this.cameraPivot = this.GetNode<Node3D>(
				PlayerConstants.NodePaths.CameraPivot
			);

			this.globalState = this.GetNode<GlobalState>(
				Constants.NodePaths.GlobalState
			);

			this.camera = this.GetNode<Camera3D>(PlayerConstants.NodePaths.Camera3D);

#if DEBUG
			var debugPanel = this.GetNode<PanelContainer>(
				PlayerConstants.NodePaths.DebugPanel
			);

			if (debugPanel is not null)
			{
				debugPanel.Visible = true;
			}

			this.debugLabel1 = this.GetNode<Label>(
				PlayerConstants.NodePaths.DebugLabel1
			);

#endif
		}

		public override void _PhysicsProcess(double delta)
		{
			var velocity = this.Velocity;
			this.ProcessCameraRotation();
			this.ProcessGravity(delta, ref velocity);
			this.ProcessJump(ref velocity);
			this.ProcessMovement(ref velocity);

#if DEBUG
			if (this.debugLabel1 is not null)
			{
				this.debugLabel1.Text =
					$"Player Camera Global Position: {this.camera?.GlobalPosition.ToString()}";
			}
#endif
			this.Velocity = velocity;
			this.MoveAndSlide();
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

			if (@event.IsActionPressed(Shared.Constants.InputMap.UiCancel))
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}

		private void ProcessGravity(double delta, ref Vector3 velocity)
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

		private void ProcessJump(ref Vector3 velocity)
		{
			if (
				Input.IsActionJustPressed(action: Shared.Constants.InputMap.Jump)
				&& this.IsOnFloor()
			)
			{
				velocity.Y = Player.JumpVelocity;
				velocity.Y = Mathf.Sqrt(Player.JumpHeight * 2 * this.gravity);
			}
		}

		private void ProcessMovement(ref Vector3 velocity)
		{
			var inputDir = Input.GetVector(
				Shared.Constants.InputMap.MoveLeft,
				Shared.Constants.InputMap.MoveRight,
				Shared.Constants.InputMap.MoveForward,
				Shared.Constants.InputMap.MoveBackward
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

		private void ProcessCameraRotation()
		{
			this.RotateY(this.mouseMotion.X);
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
