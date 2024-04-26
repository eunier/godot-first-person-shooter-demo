namespace App.Modules.Weapons
{
	using System;
	using App.Shared;
	using Godot;

	public partial class Cannon : Node3D
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready() { }

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta) { }

		public void Shoot()
		{
			Logger.Print();
		}
	}
}
