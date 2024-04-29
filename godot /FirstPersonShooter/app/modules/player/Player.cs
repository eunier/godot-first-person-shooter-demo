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
#if DEBUG
			if (this.debugLabel1 is not null)
			{
				this.debugLabel1.Text =
					$"Player Camera Global Position: {this.camera?.GlobalPosition.ToString()}";
			}
#endif
		}

		public void Damage(float damageAmount)
		{
			this.health!.Damage(damageAmount);
		}
	}
}
