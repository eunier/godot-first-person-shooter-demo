namespace App.Shared
{
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Godot;

	public static class Logger
	{
		private const string DebugConditionString = "DEBUG";

		[Conditional(Logger.DebugConditionString)]
		public static void Print(
			string message = "",
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "",
			[CallerLineNumber] int lineNumber = 0
		)
		{
			GD.Print(
				Logger.GenerateFullMessage(message, filePath, memberName, lineNumber)
			);
		}

		[Conditional(Logger.DebugConditionString)]
		public static void PrintErr(
			string message = "",
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string memberName = "",
			[CallerLineNumber] int lineNumber = 0
		)
		{
			GD.PrintErr(
				Logger.GenerateFullMessage(message, filePath, memberName, lineNumber)
			);
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

			return $"[{className}.{memberName}:line {lineNumber}]: {message}";
		}
	}
}
