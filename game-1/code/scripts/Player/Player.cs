namespace FlappyDragon.Player;

using FlappyDragon.Player.Abilities;

using Godot;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void PlayerDiedEventHandler(Player player);

	[Signal]
	public delegate void PlayerActivatedPrimaryAbilityEventHandler();

	[Signal]
	public delegate void PlayerActivatedSecondaryAbilityEventHandler();

	[Signal]
	public delegate void PlayerActivatedTertiaryAbilityEventHandler();

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private readonly float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private float _pseudoMoveSpeed = CustomProjectSettings.MovementSpeed;

	[Export]
	public bool IsGravityEnabled = true;

	[ExportGroup("Debug")]
	[Export]
	private bool _isInvincible;

	public AnimationTreeEx AnimationTree;
	private AbilityController _abilityController;
	private Area2D _obstacleDetector;

	private bool _isDead;

	public override void _Ready()
	{
		AnimationTree = GetNode<AnimationTreeEx>(nameof(AnimationTree));
		_abilityController = GetNode<AbilityController>(nameof(AbilityController));
		_obstacleDetector = GetNode<Area2D>("ObstacleDetector");

		_obstacleDetector.BodyEntered += OnEnvironmentOverlap;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isDead) return;

		var velocity = Velocity;

		if (IsGravityEnabled)
			velocity.Y += _gravity * (float)delta;
		else
			velocity.Y = 0;

		HandleInput();

		Velocity = velocity;
		LookAtRelativePoint(new Vector2(_pseudoMoveSpeed * -1, velocity.Y));
		MoveAndSlide();
	}

	public void ActivatePrimaryAbility()
	{
		if (_abilityController.ActivateAbility(Ability.AbilitySlot.Primary))
		{
			EmitSignal(SignalName.PlayerActivatedPrimaryAbility);
		}
	}

	public void ActivateSecondaryAbility()
	{
		if (_abilityController.ActivateAbility(Ability.AbilitySlot.Secondary))
		{
			EmitSignal(SignalName.PlayerActivatedSecondaryAbility);
		}
	}

	public void ActivateTertiaryAbility()
	{
		if (_abilityController.ActivateAbility(Ability.AbilitySlot.Tertiary))
		{
			EmitSignal(SignalName.PlayerActivatedTertiaryAbility);
		}
	}

	private void HandleInput()
	{
		if (Input.IsActionJustPressed("primary_ability"))
		{
			ActivatePrimaryAbility();
		}

		if (Input.IsActionJustPressed("secondary_ability"))
		{
			ActivateSecondaryAbility();
		}

		if (Input.IsActionJustPressed("tertiary_ability"))
		{
			ActivateTertiaryAbility();
		}
	}

	private void OnEnvironmentOverlap(Node2D body)
	{
		if (_isInvincible) return;
		if (_isDead) return;

		_isDead = true;
		EmitSignal(SignalName.PlayerDied, this);
	}

	private new void LookAtRelativePoint(Vector2 point)
	{
		LookAt(GlobalPosition + (point * -1));
	}
}