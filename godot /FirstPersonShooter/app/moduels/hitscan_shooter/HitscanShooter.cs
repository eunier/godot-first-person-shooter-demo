namespace App.Modules.HitscanShooterModule
{
	using System.Linq;
	using App.Modules.Utils.LoggerModule;
	using Godot;
	using Godot.Collections;

	public partial class HitscanShooter : Node3D
	{
		public Dictionary Shoot(Camera3D camera, int range)
		{
			var cameraCenter = camera.GetViewport().GetVisibleRect().Size / 2;
			var rayOrigin = camera.ProjectRayOrigin(cameraCenter);
			var rayEnd = rayOrigin + (camera.ProjectRayNormal(cameraCenter) * range);
			var rayQuery = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
			var rayInterception = this.GetWorld3D()
				.DirectSpaceState.IntersectRay(rayQuery);

			if (rayInterception.Any())
			{
				Logger.Print($"Rifle hit: {rayInterception}");
			}
			else
			{
				Logger.Print("Rifle miss.");
			}

			return rayInterception;
		}
	}
}
