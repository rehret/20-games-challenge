namespace FlappyDragon;

using Godot;

public partial class Obstacle : Node2D
{
	[Signal]
	public delegate void ObstacleLeftScreenEventHandler(Obstacle obstacle);

	[Signal]
	public delegate void PlayerClearedObstacleEventHandler();

	[Export]
	public float Spread
	{
		get => _spread;
		set
		{
			_spread = value;

			if (_top != null)
			{
				_top.Position = new Vector2(_top.Position.X, -(_spread / 2.0f));
			}

			if (_bottom != null)
			{
				_bottom.Position = new Vector2(_bottom.Position.X, _spread / 2.0f);
			}
		}
	}
	private float _spread;

	public bool Enabled = false;
	private float _speed = CustomProjectSettings.MovementSpeed;

	private Node2D _top;
	private Node2D _bottom;
	private Area2D _scoreBoundary;
	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	public override void _Ready()
	{
		_top = GetNode<Node2D>("Top");
		_bottom = GetNode<Node2D>("Bottom");
		_scoreBoundary = GetNode<Area2D>("ScoreBoundary");
		_visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>(nameof(VisibleOnScreenNotifier2D));
		_visibleOnScreenNotifier2D.ScreenExited += () => EmitSignal(SignalName.ObstacleLeftScreen, this);

		_top.Position = new Vector2(_top.Position.X, -(_spread / 2.0f));
		_bottom.Position = new Vector2(_bottom.Position.X, _spread / 2.0f);

		_scoreBoundary.BodyEntered += body =>
		{
			if (body is Player.Player)
			{
				EmitSignal(SignalName.PlayerClearedObstacle);
			}
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;

		var xPos = (float) (Position.X + (_speed * delta));
		Position = new Vector2(xPos, Position.Y);
	}
}