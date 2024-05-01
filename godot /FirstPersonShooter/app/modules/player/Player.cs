namespace App.Modules.PlayerModule
{
	using App.Global.GlobalStateModule;
	using App.Modules;
	using App.Modules.HealthModule;
	using App.Modules.PlayerDebugUIModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Player : CharacterBody3D, IWithHealth
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
		public Health Health => this.health!;

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
					$"{nameof(Player)}.{nameof(Player.camera)}.{nameof(Player.camera.GlobalPosition)}:{this.camera?.GlobalPosition.ToString()}";
			}
		}

		// // TODO: remove this method
		// public void Damage(float damageAmount)
		// {
		// 	this.health!.Damage(damageAmount);
		// }
	}
}
