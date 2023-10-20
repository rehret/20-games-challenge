namespace FlappyDragon;

using System.Collections.Generic;

using Godot;

public partial class ObstacleSpawner : Node
{
	[Signal]
	public delegate void PlayerClearedObstacleEventHandler();

	[Export]
	private PackedScene ObstacleScene;

	[Export]
	private uint _initialPoolSize;

	[Export]
	private float _minObstacleSpread = 120.0f;

	[Export]
	private float _maxObstacleSpread = 240.0f;

	[Export]
	private float _minObstacleYPosition = -246.0f;

	[Export]
	private float _maxObstacleYPosition = 246.0f;

	private Timer _timer;
	private Node _obstacleCollectionNode;

	private Stack<Obstacle> _obstaclePool;
	private RandomNumberGenerator _randomNumberGenerator;
	private uint _obstaclePoolSize;

	public override void _Ready()
	{
		_timer = GetNode<Timer>(nameof(Timer));
		_obstacleCollectionNode = new Node();
		_obstacleCollectionNode.Name = "Obstacles";
		CallDeferred(Node.MethodName.AddChild, _obstacleCollectionNode);

		_randomNumberGenerator = new RandomNumberGenerator();
		_randomNumberGenerator.Randomize();

		_obstaclePool = new Stack<Obstacle>();
		for (var i = 0; i < _initialPoolSize; i++)
		{
			_obstaclePool.Push(CreateNewObstacleInstance());
		}

		_timer.Timeout += OnSpawnTimerTimeout;
		_timer.Start();
		OnSpawnTimerTimeout();
	}

	private void OnObstacleLeftScreen(Obstacle obstacle)
	{
		obstacle.Position = new Vector2(float.MinValue, 0.0f);
		obstacle.Spread = 0.0f;
		obstacle.Enabled = false;
		_obstaclePool.Push(obstacle);
	}

	private void OnSpawnTimerTimeout()
	{
		if (!_obstaclePool.TryPop(out var obstacle))
		{
			obstacle = CreateNewObstacleInstance();
		}

		obstacle.Position = new Vector2(-200.0f, _randomNumberGenerator.RandfRange(_minObstacleYPosition, _maxObstacleYPosition));
		obstacle.Spread = _randomNumberGenerator.RandfRange(_minObstacleSpread, _maxObstacleSpread);
		obstacle.Enabled = true;
	}

	private Obstacle CreateNewObstacleInstance()
	{
		var obstacle = ObstacleScene.Instantiate() as Obstacle;
		obstacle.Name = $"Obstacle{_obstaclePoolSize++}";
		obstacle.Position = new Vector2(float.MinValue, 0.0f);
		obstacle.Spread = 0.0f;
		obstacle.Enabled = false;
		obstacle.ObstacleLeftScreen += OnObstacleLeftScreen;
		obstacle.PlayerClearedObstacle += OnPlayerClearedObstacle;
		_obstacleCollectionNode.AddChild(obstacle);

		return obstacle;
	}

	private void OnPlayerClearedObstacle()
	{
		EmitSignal(SignalName.PlayerClearedObstacle);
	}
}