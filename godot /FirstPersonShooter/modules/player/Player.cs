namespace App.Modules.Player
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Reflection.Metadata;
	using App.Modules.Weapons;
	using App.Shared;
	using Godot;

	public partial class Player : CharacterBody3D
	{
		private const float FallMultiplier = 1.5f;
		private const float JumpHeight = 1f;
		private const float JumpVelocity = 4.5f;
		private const float Speed = 5.0f;

		[Export]
		[Required]
		private Rifle rifle;

		[Export]
		[Required]
		private Cannon cannon;

		private float gravity = ProjectSettings
			.GetSetting("physics/3d/default_gravity")
			.AsSingle();

		private Node3D cameraPivot;
		private Vector2 mouseMotion = Vector2.Zero;
		private Dictionary<WeaponEnum, Weapon> weapons = new();
		private KeyValuePair<WeaponEnum, Weapon> currentWeapon;

		private float hitpoints;
		private float Hitpoints
		{
			get { return this.hitpoints; }
			set
			{
				this.hitpoints = value;
				GD.Print(this.hitpoints);

				if (this.hitpoints <= 0)
				{
					this.GetTree().Quit();
				}
			}
		}

		public override void _Ready()
		{
			base._Ready();
			Input.MouseMode = Input.MouseModeEnum.Captured;
			this.cameraPivot = this.GetNode<Node3D>("CameraPivot");
			this.weapons.Add(WeaponEnum.Rifle, this.rifle);
			this.weapons.Add(WeaponEnum.Cannon, this.cannon);
			this.EquipWeapon(WeaponEnum.Rifle);
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			var velocity = this.Velocity;
			this.HandleCameraRotation();
			this.ApplyGravity(delta, ref velocity);
			this.HandleJump(ref velocity);
			this.HandleMovement(ref velocity);
			this.Velocity = velocity;
			this.MoveAndSlide();
		}

		public override void _Input(InputEvent @event)
		{
			base._Input(@event);

			if (
				@event is InputEventMouseMotion mouseMotionInputEvent
				&& Input.MouseMode == Input.MouseModeEnum.Captured
			)
			{
				this.mouseMotion = -mouseMotionInputEvent.Relative * 0.003f;
			}

			if (@event.IsActionPressed(Shared.Constants.InputMap.UiCancel))
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}

			if (@event.IsActionPressed(Shared.Constants.InputMap.NextWeapon))
			{
				this.EquipNextWeapon();
			}

			if (@event.IsActionPressed(Shared.Constants.InputMap.PreviousWeapon))
			{
				this.EquipPreviousWeapon();
			}
		}

		private void ApplyGravity(double delta, ref Vector3 velocity)
		{
			if (!this.IsOnFloor())
			{
				if (velocity.Y >= 0)
				{
					velocity.Y -= this.gravity * (float)delta;
				}
				else
				{
					velocity.Y -= this.gravity * (float)delta * Player.FallMultiplier;
				}
			}
		}

		private void HandleJump(ref Vector3 velocity)
		{
			if (
				Input.IsActionJustPressed(action: Shared.Constants.InputMap.Jump)
				&& this.IsOnFloor()
			)
			{
				velocity.Y = Player.JumpVelocity;
				velocity.Y = Mathf.Sqrt(Player.JumpHeight * 2 * this.gravity);
			}
		}

		private void HandleMovement(ref Vector3 velocity)
		{
			var inputDir = Input.GetVector(
				Shared.Constants.InputMap.MoveLeft,
				Shared.Constants.InputMap.MoveRight,
				Shared.Constants.InputMap.MoveForward,
				Shared.Constants.InputMap.MoveBackward
			);

			var direction = (
				this.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)
			).Normalized();

			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(this.Velocity.X, 0, Player.Speed);
				velocity.Z = Mathf.MoveToward(this.Velocity.Z, 0, Player.Speed);
			}
		}

		private void HandleCameraRotation()
		{
			this.RotateY(this.mouseMotion.X);
			this.cameraPivot.RotateX(this.mouseMotion.Y);

			this.cameraPivot.RotationDegrees = new Vector3(
				(float)Mathf.Clamp(this.cameraPivot.RotationDegrees.X, -90.0, 90.0),
				this.cameraPivot.RotationDegrees.Y,
				this.cameraPivot.RotationDegrees.Z
			);

			this.mouseMotion = Vector2.Zero;
		}

		private void EquipWeapon(WeaponEnum weaponType)
		{
			Logger.Print($"Equipping {weaponType}");

			foreach (var kvp in this.weapons)
			{
				var (k, v) = kvp;

				if (k == weaponType)
				{
					this.currentWeapon = kvp;
					v.Visible = true;
					v.SetProcess(true);
					Logger.Print($"Equipped {k}.");
				}
				else
				{
					v.Visible = false;
					v.SetProcess(false);
					Logger.Print($"Unequipped {k}.");
				}
			}
		}

		private void EquipNextWeapon()
		{
			var nextWeaponIndex = Mathf.Wrap(
				(int)this.currentWeapon.Key + 1,
				0,
				this.weapons.Count
			);

			var nextWeaponType = (WeaponEnum)nextWeaponIndex;
			Logger.Print($"Equipping preview weapon: {nextWeaponType}.");
			this.EquipWeapon((WeaponEnum)nextWeaponIndex);
		}

		private void EquipPreviousWeapon()
		{
			var nextWeaponIndex = Mathf.Wrap(
				(int)this.currentWeapon.Key - 1,
				0,
				this.weapons.Count
			);

			var previousWeaponType = (WeaponEnum)nextWeaponIndex;
			Logger.Print($"Equipping preview weapon: {previousWeaponType}.");
			this.EquipWeapon((WeaponEnum)nextWeaponIndex);
		}
	}
}
