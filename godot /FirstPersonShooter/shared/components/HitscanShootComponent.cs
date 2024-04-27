namespace App.Shared.Components
{
	using System;
	using App.Shared.Utils;
	using Godot;

	public partial class HitscanShootComponent : Node3D
	{
		[Export]
		private NodePath rayCastNodePath;
		private RayCast3D rayCast;

		public override void _Ready()
		{
			this.rayCast = this.GetNode<RayCast3D>(this.rayCastNodePath);

			if (this.rayCast == null)
			{
				var msg = "RayCast3D not found.";
				Logger.Print(msg);
				throw new Exception(msg);
			}
		}

		public override void _Process(double delta) { }
	}
}
