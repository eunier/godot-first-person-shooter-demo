namespace App.Modules.PlayerModule
{
	using App.Modules;
	using App.Modules.GlobalStateModule;
	using App.Modules.HealthModule;
	using App.Modules.PlayerDebugUIModule;
	using App.Modules.Utils;
	using Godot;

	public partial class Player : CharacterBody3D
	{
		private const string Camera3DNodePath = "%Camera3D";
		private const string CameraPivotNodePath = "%CameraPivot";
		private const string HealthNodePath = "%Health";
		private Camera3D? camera;
		private GlobalState? globalState;
		private Health? health;
		private Label? debugLabel1;
		private Node3D? cameraPivot;
		private PlayerDebugUI? debugUI;

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
			this.health = this.GetNode<Health>(Player.HealthNodePath);
			this.camera = this.GetNode<Camera3D>(Player.Camera3DNodePath);
			this.cameraPivot = this.GetNode<Node3D>(Player.CameraPivotNodePath);

			this.globalState = this.GetNode<GlobalState>(
				GlobalConstants.NodePaths.GlobalState
			);

			if (OS.IsDebugBuild())
			{
				var debugUIScene = ResourceLoader.Load<PackedScene>(
					PlayerDebugUI.ResourcePath
				);

				this.debugUI = debugUIScene.Instantiate<PlayerDebugUI>();
				this.AddChild(this.debugUI);
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			if (OS.IsDebugBuild())
			{
				this.debugUI!.Text =
					$"Player.Camera3D.GlobalPosition:{this.camera?.GlobalPosition.ToString()}";
			}
		}

		public void Damage(float damageAmount)
		{
			this.health!.Damage(damageAmount);
		}
	}
}
