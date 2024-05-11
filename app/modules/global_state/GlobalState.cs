namespace App.Global.GlobalStateModule
{
	using App.Utils.LoggerModule;
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
				Logger.Print($"Setting PlayerCamera with value: {value}");
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
