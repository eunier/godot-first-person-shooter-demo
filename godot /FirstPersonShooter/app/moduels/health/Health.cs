namespace App.Modules.HealthModule
{
	using System;
	using App.Modules.Utils.LoggerModule;
	using Godot;

	public partial class Health : Node
	{
		[Export]
		private float maxHealth = 100;

		private float currentHealth;

		[Signal]
		public delegate void DiedEventHandler();

		public override void _Ready()
		{
			this.currentHealth = this.maxHealth;
		}

		public void Damage(float damageAmount)
		{
			Logger.Print(
				$"Current health: {this.currentHealth}. Taking {damageAmount} hitpoints of damage."
			);

			this.currentHealth = Math.Max(this.currentHealth - damageAmount, 0);

			if (this.currentHealth == 0)
			{
				Logger.Print($"Emitting signal {SignalName.Died}");
				this.EmitSignal(SignalName.Died);
				Logger.Print("Deleting owner");
				this.Owner.QueueFree();
			}
		}
	}
}
