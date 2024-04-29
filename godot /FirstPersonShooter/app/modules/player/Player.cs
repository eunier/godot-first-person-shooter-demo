namespace App.Modules.PlayerModule
{
	using App.Modules;
	using App.Modules.GlobalStateModule;
	using App.Modules.HealthModule;
	using App.Modules.Utils;
	using Godot;

	public partial class Player : CharacterBody3D
	{
		private const float FallMultiplier = 1.5f;
		private const float JumpHeight = 1f;
		private const float JumpVelocity = 4.5f;
		private const float MaxHealth = 100;
		private const float Speed = 5.0f;
		private const string Camera3DNodePath = "%Camera3D";
		private const string CameraPivotNodePath = "%CameraPivot";
		private const string DebugLabel1NodePath = "%DebugLabel";
		private const string DebugPanelNodePath = "%DebugPanel";
		private const string HealthNodePath = "%Health";
		private float currentHealth = Player.MaxHealth;
		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();
		private Camera3D? camera;
		private GlobalState? globalState;
		private Health? health;
		private Label? debugLabel1;
		private Node3D? cameraPivot;
		private Vector2 mouseMotion = Vector2.Zero;

		public static Player? GetPlayer(Node caller)
		{
			var player = (Player?)
				caller.GetTree().GetFirstNodeInGroup(GlobalConstants.Groups.Player);

			if (player is null)
			{
				Logger.Print(
					$"No node found in group '{GlobalConstants.Groups.Player}'."
				);
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
			this.cameraPivot = this.GetNode<Node3D>(Player.CameraPivotNodePath);

			this.globalState = this.GetNode<GlobalState>(
				GlobalConstants.NodePaths.GlobalState
			);

			this.health = this.GetNode<Health>(Player.HealthNodePath);
			this.camera = this.GetNode<Camera3D>(Player.Camera3DNodePath);

#if DEBUG
			var debugPanel = this.GetNode<PanelContainer>(DebugPanelNodePath);

			if (debugPanel is not null)
			{
				debugPanel.Visible = true;
			}

			this.debugLabel1 = this.GetNode<Label>(DebugLabel1NodePath);
#endif
		}

		public override void _PhysicsProcess(double delta)
		{
			// var velocity = this.Velocity;
			// this.ProcessCameraRotation();
			// this.ProcessGravity(delta, ref velocity);
			// this.ProcessJump(ref velocity);
			// this.ProcessMovement(ref velocity);

#if DEBUG
			if (this.debugLabel1 is not null)
			{
				this.debugLabel1.Text =
					$"Player Camera Global Position: {this.camera?.GlobalPosition.ToString()}";
			}
#endif
			// this.Velocity = velocity;
			// this.MoveAndSlide();
		}

		// public override void _Input(InputEvent @event)
		// {
		// 	if (
		// 		@event is InputEventMouseButton
		// 		&& Input.MouseMode is not Input.MouseModeEnum.Captured
		// 	)
		// 	{
		// 		Input.MouseMode = Input.MouseModeEnum.Captured;
		// 	}

		// 	if (
		// 		@event is InputEventMouseMotion mouseMotionInputEvent
		// 		&& Input.MouseMode == Input.MouseModeEnum.Captured
		// 	)
		// 	{
		// 		this.mouseMotion = -mouseMotionInputEvent.Relative * 0.003f;
		// 	}

		// 	if (@event.IsActionPressed(App.Modules.GlobalConstants.InputMap.UiCancel))
		// 	{
		// 		Input.MouseMode = Input.MouseModeEnum.Visible;
		// 	}
		// }

		public void Damage(float damageAmount)
		{
			this.health!.Damage(damageAmount);
		}

		// private void ProcessGravity(double delta, ref Vector3 velocity)
		// {
		// 	if (!this.IsOnFloor())
		// 	{
		// 		if (velocity.Y >= 0)
		// 		{
		// 			velocity.Y -= this.gravity * (float)delta;
		// 		}
		// 		else
		// 		{
		// 			velocity.Y -= this.gravity * (float)delta * Player.FallMultiplier;
		// 		}
		// 	}
		// }

		// private void ProcessJump(ref Vector3 velocity)
		// {
		// 	if (
		// 		Input.IsActionJustPressed(
		// 			action: App.Modules.GlobalConstants.InputMap.Jump
		// 		) && this.IsOnFloor()
		// 	)
		// 	{
		// 		velocity.Y = Player.JumpVelocity;
		// 		velocity.Y = Mathf.Sqrt(Player.JumpHeight * 2 * this.gravity);
		// 	}
		// }

		// private void ProcessMovement(ref Vector3 velocity)
		// {
		// 	var inputDir = Input.GetVector(
		// 		App.Modules.GlobalConstants.InputMap.MoveLeft,
		// 		App.Modules.GlobalConstants.InputMap.MoveRight,
		// 		App.Modules.GlobalConstants.InputMap.MoveForward,
		// 		App.Modules.GlobalConstants.InputMap.MoveBackward
		// 	);

		// 	var direction = (
		// 		this.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)
		// 	).Normalized();

		// 	if (direction != Vector3.Zero)
		// 	{
		// 		velocity.X = direction.X * Speed;
		// 		velocity.Z = direction.Z * Speed;
		// 	}
		// 	else
		// 	{
		// 		velocity.X = Mathf.MoveToward(this.Velocity.X, 0, Player.Speed);
		// 		velocity.Z = Mathf.MoveToward(this.Velocity.Z, 0, Player.Speed);
		// 	}
		// }

		// private void ProcessCameraRotation()
		// {
		// 	this.RotateY(this.mouseMotion.X);
		// 	if (this.cameraPivot is not null)
		// 	{
		// 		this.cameraPivot.RotateX(this.mouseMotion.Y);

		// 		this.cameraPivot.RotationDegrees = new Vector3(
		// 			(float)Mathf.Clamp(this.cameraPivot.RotationDegrees.X, -90.0, 90.0),
		// 			this.cameraPivot.RotationDegrees.Y,
		// 			this.cameraPivot.RotationDegrees.Z
		// 		);
		// 	}

		// 	this.mouseMotion = Vector2.Zero;
		// }
	}
}
