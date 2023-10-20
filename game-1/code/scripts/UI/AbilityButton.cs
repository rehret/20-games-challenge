namespace FlappyDragon.UI;

using FlappyDragon.Player.Abilities;

using Godot;

public partial class AbilityButton : Button
{
	[Export]
	private Ability _ability;

	private Timer _timer;

	public override void _Ready()
	{
		_timer = GetNode<Timer>(nameof(Timer));
		SetAbility(_ability);
		Pressed += OnPressed;
		_timer.Timeout += OnTimerTimeout;
	}

	public void SetAbility(Ability ability)
	{
		if (ability == null) return;

		_timer.Stop();
		_ability = ability;
		Icon = _ability.ButtonIcon;
		_timer.WaitTime = _ability.Cooldown;
		Text = string.Empty;
	}

	public void SetAbilityActivated()
	{
		if (!_timer.IsStopped()) return;

		_timer.Start();
		Disabled = true;
	}

	public override void _Process(double delta)
	{
		if (_timer.IsStopped()) return;

		Text = Mathf.CeilToInt(_timer.TimeLeft).ToString();
	}

	private void OnPressed()
	{
		SetAbilityActivated();
	}

	private void OnTimerTimeout()
	{
		Text = string.Empty;
		Disabled = false;
	}
}