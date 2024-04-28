namespace App.Global
{
	using System;
	using App.Shared.Utils;
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
#if DEBUG
				Logger.Print($"Setting PlayerCamera with value: {value}");
#endif
				this.playerCamera = value;
			}
		}

		public Vector3? PlayerCameraGlobalPosition
		{
			get
			{
#if DEBUG
				if (this.PlayerCamera is null)
				{
					var msg =
						"Cannot access PlayerCameraGlobalPosition if PlayerCamera is null.";
					Logger.PrintErr(msg);
				}
#endif
				return this.PlayerCamera?.GlobalPosition;
			}
			private set { }
		}
	}
}
