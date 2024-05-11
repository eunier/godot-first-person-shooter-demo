namespace App.Utils.LoggerModule
{
	using System.Runtime.CompilerServices;
	using Godot;

	public static class Logger
	{
		public static void Print(
			string message = "",
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "",
			[CallerLineNumber] int lineNumber = 0
		)
		{
			if (OS.IsDebugBuild())
			{
				GD.Print(
					Logger.GenerateFullMessage(message, filePath, memberName, lineNumber)
				);
			}
		}

		public static void PrintErr(
			string message = "",
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "",
			[CallerLineNumber] int lineNumber = 0
		)
		{
			if (OS.IsDebugBuild())
			{
				GD.PrintErr(
					Logger.GenerateFullMessage(message, filePath, memberName, lineNumber)
				);
			}
		}

		public static void PushErr(
			string message = "",
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "",
			[CallerLineNumber] int lineNumber = 0
		)
		{
			if (OS.IsDebugBuild())
			{
				GD.PushError(
					Logger.GenerateFullMessage(message, filePath, memberName, lineNumber)
				);
			}
		}

		private static string GenerateFullMessage(
			string message,
			string filePath,
			string memberName,
			int lineNumber = 0
		)
		{
			var className = string.IsNullOrEmpty(filePath)
				? string.Empty
				: System.IO.Path.GetFileNameWithoutExtension(filePath);

			var time = Time.GetTicksMsec();
			return $"{time}-[{className}.{memberName}:{lineNumber}]: {message}";
		}
	}
}
