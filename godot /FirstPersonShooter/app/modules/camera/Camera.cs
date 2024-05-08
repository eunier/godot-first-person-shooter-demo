namespace App.Modules.CameraModule
{
	using System.Linq;
	using App.Utils.LoggerModule;
	using Godot;

	public static class Camera
	{
		public static CameraRayInterceptionResult GetCameraRayInterception(
			Node3D caller,
			int range
		)
		{
			var camera = caller.GetViewport().GetCamera3D();
			var cameraCenter = camera.GetViewport().GetVisibleRect().Size / 2;
			var rayOrigin = camera.ProjectRayOrigin(cameraCenter);
			var rayEnd = rayOrigin + (camera.ProjectRayNormal(cameraCenter) * range);
			var rayQuery = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);

			var rayInterception = caller
				.GetWorld3D()
				.DirectSpaceState.IntersectRay(rayQuery);

			var res = new CameraRayInterceptionResult { Position = rayEnd };

			if (rayInterception.Any())
			{
				Logger.Print($"Camera intercepted {rayInterception}");
				res.Collider = (GodotObject)rayInterception["collider"];
				res.Normal = (Vector3)rayInterception["normal"];
				res.Position = (Vector3)rayInterception["position"];
			}

			return res;
		}
	}
}
