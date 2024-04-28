namespace App.Modules.EnemyModule
{
	using App.Modules.PlayerModule;
	using App.Shared;
	using App.Shared.Utils;
	using Godot;

	public partial class Enemy : CharacterBody3D
	{
		private const float JumpVelocity = 4.5f;
		private const float Speed = 5.0f;
		private const float FallMultiplier = 1.5f;
		private const float AttackRange = 1.5f;
		private const int AttackDamage = 20;
		private const int MaxHitpoints = 100;
		private const int AggroRange = 12;
		private bool provoked = false;
		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();

		private Player? player;

		private float hitpoints;
		private float Hitpoints
		{
			get { return this.hitpoints; }
			set
			{
				this.hitpoints = value;
				Logger.Print(this.hitpoints.ToString());

				if (this.hitpoints <= 0)
				{
					this.GetTree().Quit();
				}
			}
		}

		public override void _Ready()
		{
			this.player = (Player?)
				this.GetTree().GetFirstNodeInGroup(Constants.Groups.Player);
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector3 velocity = this.Velocity;

			// Add the gravity.
			if (!this.IsOnFloor())
			{
				velocity.Y -= this.gravity * (float)delta;
			}

			if (Input.IsActionJustPressed("ui_accept") && this.IsOnFloor())
			{
				velocity.Y = Enemy.JumpVelocity;
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 inputDir = Input.GetVector(
				"ui_left",
				"ui_right",
				"ui_up",
				"ui_down"
			);
			Vector3 direction = (
				this.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)
			).Normalized();
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(this.Velocity.X, 0, Speed);
				velocity.Z = Mathf.MoveToward(this.Velocity.Z, 0, Speed);
			}

			this.Velocity = velocity;
			this.MoveAndSlide();
		}
	}
}
