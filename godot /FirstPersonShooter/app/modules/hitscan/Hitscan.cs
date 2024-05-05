namespace App.Modules.HitscanModule
{
	using System.Linq;
	using App.Utils.CameraUtilsModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Hitscan : Node3D // TODO should this be a Node instead? test it out.
	{
		public ShootResult? Shoot(Camera3D camera, int range)
		{
			// TODO use CameraUtils
			var cameraCenter = camera.GetViewport().GetVisibleRect().Size / 2;
			var rayOrigin = camera.ProjectRayOrigin(cameraCenter);
			var rayEnd = rayOrigin + (camera.ProjectRayNormal(cameraCenter) * range);
			var rayQuery = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);

			var rayInterception = this.GetWorld3D()
				.DirectSpaceState.IntersectRay(rayQuery);

			if (rayInterception.Any())
			{
				Logger.Print($"Hit {rayInterception}");

				var res = new ShootResult(
					(GodotObject)rayInterception["collider"],
					(Vector3)rayInterception["position"],
					(Vector3)rayInterception["normal"]
				);

				return res;
			}

			Logger.Print("Miss.");
			return null;
		}
	}
}
