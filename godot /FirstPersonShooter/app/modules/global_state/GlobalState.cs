namespace App.Modules.GlobalStateModule
{
	using App.Modules.Utils;
	using Godot;

	public partial class GlobalState : Node
	{
		private Node3D? playerCamera;

		[Export]
		public Node3D? PlayerCamera
		{
			private get => this.playerCamera;
			set
			{
				if (OS.IsDebugBuild())
				{
					Logger.Print($"Setting PlayerCamera with value: {value}");
				}

				this.playerCamera = value;
			}
		}

		public Vector3? PlayerCameraGlobalPosition
		{
			get
			{
				if (OS.IsDebugBuild())
				{
					if (this.PlayerCamera is null)
					{
						var msg =
							"Cannot access PlayerCameraGlobalPosition if PlayerCamera is null.";
						Logger.PrintErr(msg);
					}
					else
					{
						Logger.Print(this.PlayerCamera.GlobalPosition.ToString());
					}
				}
				return this.PlayerCamera?.GlobalPosition;
			}
			private set { }
		}
	}
}
