namespace App.Modules.EnemyModule
{
	using App.Modules.PlayerModule;
	using App.Shared;
	using App.Shared.Utils;
	using Godot;

	public partial class Enemy : CharacterBody3D
	{
		private const float Speed = 3.0f;
		private const float FallMultiplier = 1.5f;
		private const float AttackRange = 1.5f;
		private const int AttackDamage = 20;
		private const int AggroRange = 12;
		private const float MaxHitpoints = 100;
		private bool provoked = false;
		private Player? player;
		private AnimationPlayer? animationPlayer;
		private NavigationAgent3D? navigationAgent;

		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();

		private float hitpoints = Enemy.MaxHitpoints;
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
			Logger.Print("Attacking.");
			this.player?.Damage(Enemy.AttackDamage);
		}

		public override void _Ready()
		{
			this.player = (Player?)
				this.GetTree().GetFirstNodeInGroup(Constants.Groups.Player);

#if DEBUG
			Logger.Print(
				this.player is not null ? "Player found." : "Player not found."
			);
#endif

			this.animationPlayer = this.GetNode<AnimationPlayer>(
				EnemyConstants.NodePaths.AnimationPlayer
			);

			this.navigationAgent = this.GetNode<NavigationAgent3D>(
				EnemyConstants.NodePaths.NavigationAgent3D
			);
		}

		public override void _Process(double delta)
		{
			if (this.provoked && this.player is not null)
			{
				this.navigationAgent!.TargetPosition = this.player.GlobalPosition;
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

			if (this.player is not null)
			{
				var distanceToTarget = this.GlobalPosition.DistanceTo(
					this.player.GlobalPosition
				);

				if (distanceToTarget <= Enemy.AggroRange)
				{
					this.provoked = true;
				}

				if (this.provoked && distanceToTarget <= Enemy.AttackRange)
				{
					this.animationPlayer!.Play(EnemyConstants.Animations.Attack);
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
