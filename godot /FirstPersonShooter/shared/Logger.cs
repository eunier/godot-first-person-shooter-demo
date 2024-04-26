namespace App.Shared
{
	using System.Runtime.CompilerServices;
	using Godot;

	public static class Logger
	{
		public static void Log(
			string message,
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "",
			[CallerLineNumber] int lineNumber = 0
		)
		{
#if DEBUG
			var className = string.IsNullOrEmpty(filePath)
				? string.Empty
				: System.IO.Path.GetFileNameWithoutExtension(filePath);

			GD.Print($"[{className}.{memberName} at line {lineNumber}]: {message}");
#endif
		}
	}
}
