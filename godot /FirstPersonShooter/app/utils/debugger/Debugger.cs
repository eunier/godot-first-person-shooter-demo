namespace App.Modules
{
	using Godot;

	public static class Debugger
	{
		public static void Breakpoint()
		{
			if (OS.IsDebugBuild())
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}
	}
}
