namespace FlappyDragon;

using FlappyDragon.UI;

using Godot;

public partial class GameMode : Node
{
	[Signal]
	public delegate void ScoreChangedEventHandler(uint score);

	[Signal]
	public delegate void PlayerDiedEventHandler();

	[Export]
	private uint _pointsPerObstacle = 1;

	[Export(PropertyHint.File, "*.tscn")]
	private string _gameScenePath;

	[Export]
	private UIController _uiController;

	private uint _score;

	public override void _Ready()
	{
		_uiController.RestartButtonPressed += RestartGame;
	}

	public void RestartGame()
	{
		GetTree().ChangeSceneToFile(_gameScenePath);
	}

	private void OnObstacleCleared()
	{
		_score += _pointsPerObstacle;
		EmitSignal(SignalName.ScoreChanged, _score);
	}

	private void OnPlayerDied(Player.Player player)
	{
		EmitSignal(SignalName.PlayerDied);
		player.QueueFree();
	}
}