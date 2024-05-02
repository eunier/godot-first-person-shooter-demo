namespace App.Modules.SparksControllerModule
{
	using App.Modules.SparksModule;
	using Godot;

	public partial class SparksController : Node3D
	{
		[Export]
		private PackedScene? sparksScene;

		public override void _Ready()
		{
			this.sparksScene = ResourceLoader.Load<PackedScene>(Sparks.ResourcePath);
		}

		public void Create(Vector3 position)
		{
			var spark = this.sparksScene!.Instantiate<Sparks>();
			this.AddChild(spark);
			spark.GlobalPosition = position;
		}
	}
}
