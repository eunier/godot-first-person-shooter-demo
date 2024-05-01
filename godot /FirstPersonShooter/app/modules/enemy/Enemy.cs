namespace App.Modules.EnemyModule
{
	using App.Modules;
	using App.Modules.HealthModule;
	using App.Modules.PlayerModule;
	using App.Utils.LoggerModule;
	using Godot;

	public partial class Enemy : CharacterBody3D, IWithHealth
	{
		private const float AttackRange = 1.5f;
		private const float FallMultiplier = 1.5f;
		private const float MaxHitpoints = 100;
		private const float Speed = 3.0f;
		private const int AggroRange = 12;
		private const int AttackDamage = 20;
		private const string AnimationPlayerNodePath = "%AnimationPlayer";
		private const string AttackAnimationName = "attack";
		private const string HealthNodePath = "%Health";
		private const string NavigationAgent3DNodePath = "%NavigationAgent3D";
		private bool provoked = false;
		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();
		private float hitpoints = Enemy.MaxHitpoints;
		private AnimationPlayer? animationPlayer;
		private Health? health;
		private NavigationAgent3D? navigationAgent;
		private Player? player;
		public Health Health => this.health!;
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

		public void Attack()
		{
			Logger.Print(message: "Attacking.");
			this.player?.Health.Damage(Enemy.AttackDamage);
		}

		public override void _Ready()
		{
			this.player = (Player?)
				this.GetTree().GetFirstNodeInGroup(GlobalConstants.Groups.Player);

			if (OS.IsDebugBuild())
			{
				Logger.Print(
					this.player is not null ? "Player found." : "Player not found."
				);
			}

			this.animationPlayer = this.GetNode<AnimationPlayer>(
				Enemy.AnimationPlayerNodePath
			);

			this.navigationAgent = this.GetNode<NavigationAgent3D>(
				Enemy.NavigationAgent3DNodePath
			);

			this.health = this.GetNode<Health>(HealthNodePath);
		}

		public override void _Process(double delta)
		{
			if (this.provoked && IsInstanceValid(this.player))
			{
				this.navigationAgent!.TargetPosition = this.player!.GlobalPosition;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			var velocity = this.Velocity;

			if (!this.IsOnFloor())
			{
				velocity.Y -= this.gravity * (float)delta * Enemy.FallMultiplier;
			}

			var nextPosition = this.navigationAgent!.GetNextPathPosition();
			var direction = this.GlobalPosition.DirectionTo(nextPosition);

			if (IsInstanceValid(this.player))
			{
				var distanceToTarget = this.GlobalPosition.DistanceTo(
					this.player!.GlobalPosition
				);

				if (distanceToTarget <= Enemy.AggroRange)
				{
					this.provoked = true;
				}

				if (this.provoked && distanceToTarget <= Enemy.AttackRange)
				{
					this.animationPlayer!.Play(Enemy.AttackAnimationName);
				}
			}

			if (direction != Vector3.Zero)
			{
				this.LookAtTarget(direction);
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

		private void LookAtTarget(Vector3 direction)
		{
			direction.Y = 0;
			this.LookAt(this.GlobalPosition + direction, Vector3.Up, true);
		}
	}
}
