namespace App.Modules.GlobalConstantsModule
{
	public static class GlobalConstants
	{
		public static class InputMap
		{
			public const string Exit = "exit";
			public const string Jump = "jump";
			public const string MoveBackward = "move_backward";
			public const string MoveForward = "move_forward";
			public const string MoveLeft = "move_left";
			public const string MoveRight = "move_right";
			public const string NextWeapon = "next_weapon";
			public const string PreviousWeapon = "previous_weapon";
			public const string Shoot = "shoot";
			public const string UiCancel = "ui_cancel";
		}

		public static class Groups
		{
			public const string Player = "player";
		}

		public static class NodePaths
		{
			public const string GlobalState = "/root/GlobalState";
		}
	}
}
