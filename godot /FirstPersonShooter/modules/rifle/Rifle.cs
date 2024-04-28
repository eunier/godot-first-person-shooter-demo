namespace App.Modules.Rifle
{
	using System.Linq;
	using System.Reflection.Metadata;
	using App.Global;
	using App.Shared;
	using App.Shared.Abstract;
	using App.Shared.Components;
	using App.Shared.Utils;
	using Godot;

	public partial class Rifle : Weapon
	{
		private HitscanShootComponent? shootComponent;

		public override void _Ready()
		{
			this.shootComponent = this.GetNode<HitscanShootComponent>(
				RifleConstants.NodePaths.HitscanShootComponent
			);
		}

		public void Shoot(Camera3D? camera)
		{
			Logger.Print($"Shooting");

			if (camera is not null)
			{
				var cameraCenter = camera.GetViewport().GetVisibleRect().Size / 2;
				var rayOrigin = camera.ProjectRayOrigin(cameraCenter);
				var rayEnd = rayOrigin + (camera.ProjectRayNormal(cameraCenter) * 2);
				var rayQuery = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
				var rayInterception = this.GetWorld3D()
					.DirectSpaceState.IntersectRay(rayQuery);
				if (rayInterception.Any()) { }
				else
				{
					Logger.Print("Rifle miss.");
				}
				// var center =
				// var origin = camera.ProjectRayOrigin(camera.GlobalTransform);
				// var a = PhysicsRayQueryParameters3D.Create()

				// var rayCast = new RayCast3D
				// {
				// 	GlobalPosition = camera.GlobalPosition,
				// 	TargetPosition = new Vector3(0, 0, -5),
				// };

				// var rayCast = new RayCast3D();
				// camera.AddChild(rayCast);

				// Logger.Print($"Rifle RayCast.GlobalPosition: {rayCast.GlobalPosition}");

				// var collider = this.shootComponent?.Shoot(rayCast);

				// if (collider is not null)
				// {
				// 	Logger.Print($"Rifle hit: {collider}");
				// }
				// else
				// {
				// 	Logger.Print("Rifle miss.");
				// }
			}
			else
			{
				Logger.Print("No camera passed");
			}
		}
	}
}
