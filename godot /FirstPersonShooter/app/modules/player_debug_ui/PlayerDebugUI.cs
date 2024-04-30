namespace App.Modules.PlayerDebugUIModule
{
	using App.Modules.Utils;
	using Godot;

	public partial class PlayerDebugUI : Control
	{
		public const string ResourcePath =
			"res://app/modules/player_debug_ui/player_debug_ui.tscn";
		private const string LabelNodePath = "%DebugLabel";
		private Label? label;

		public string Text
		{
			get { return this.label?.Text ?? string.Empty; }
			set
			{

				if (this.label is not null)
				{
					Logger.Print(value);
					this.label.Text = value;
				}
			}
		}

		public override void _Ready()
		{
			this.label = this.GetNode<Label>(PlayerDebugUI.LabelNodePath);
		}
	}
}
