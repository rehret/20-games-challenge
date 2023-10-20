namespace FlappyDragon.UI;

using FlappyDragon.Player;

using Godot;

public partial class UIController : Control
{
	[Signal]
	public delegate void RestartButtonPressedEventHandler();

	[Export]
	private Label _scoreLabel;

	[Export]
	private Control _gameOverContainer;

	[Export]
	private Player _player;

	private AbilityButton _secondaryAbilityButton;
	private AbilityButton _tertiaryAbilityButton;

	public override void _Ready()
	{
		_gameOverContainer.Visible = false;

		_secondaryAbilityButton = GetNode<AbilityButton>("SecondaryAbilityButton");
		_tertiaryAbilityButton = GetNode<AbilityButton>("TertiaryAbilityButton");

		_secondaryAbilityButton.Pressed += OnSecondaryAbilityButtonPressed;
		_tertiaryAbilityButton.Pressed += OnTertiaryAbilityButtonPressed;

		_player.PlayerActivatedSecondaryAbility += OnSecondaryAbilityActivated;
		_player.PlayerActivatedTertiaryAbility += OnTertiaryAbilityActivated;
	}

	private void OnScoreChanged(uint score)
	{
		_scoreLabel.Text = score.ToString();
	}

	private void OnPlayerDied()
	{
		_gameOverContainer.Visible = true;
		_secondaryAbilityButton.Visible = false;
		_tertiaryAbilityButton.Visible = false;
	}

	private void OnRetryButtonPressed()
	{
		EmitSignal(SignalName.RestartButtonPressed);
	}

	private void OnSecondaryAbilityActivated()
	{
		_secondaryAbilityButton.SetAbilityActivated();
	}

	private void OnTertiaryAbilityActivated()
	{
		_tertiaryAbilityButton.SetAbilityActivated();
	}

	private void OnSecondaryAbilityButtonPressed()
	{
		_player.ActivateSecondaryAbility();
	}

	private void OnTertiaryAbilityButtonPressed()
	{
		_player.ActivateTertiaryAbility();
	}
}