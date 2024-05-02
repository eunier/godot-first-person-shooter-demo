namespace App.Modules.SparkControllerModule
{
	using App.Modules.SparkModule;
	using Godot;

	public partial class SparkController : Node3D
	{
		[Export]
		private PackedScene? sparkScene;

		public override void _Ready()
		{
			this.sparkScene = ResourceLoader.Load<PackedScene>(Spark.ResourcePath);
		}

		public void Create(Vector3 position)
		{
			var spark = this.sparkScene!.Instantiate<Spark>();
			this.AddChild(spark);
			spark.GlobalPosition = position;
		}
	}
}
