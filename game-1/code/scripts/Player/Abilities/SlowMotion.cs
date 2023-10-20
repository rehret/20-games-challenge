namespace FlappyDragon.Player.Abilities;

using System;

using Godot;

public partial class SlowMotion : Ability
{
    [Export]
    private float _slowMotionTimeScale = 0.5f;

    private const float NormalTimeScale = 1.0f;

    [Export]
    private float _slowMotionDuration = 5.0f;

    [Export]
    private float _timeScaleLerpWeight = 0.1f;

    [Export]
    private float _timeScaleLerpInterval = 0.1f;

    protected override State ExecuteAbility(Player player, double delta)
    {
        var slowMotionTimer = new Timer
        {
            Name = "SlowMotionTimer",
            Autostart = true,
            OneShot = true,
            WaitTime = _slowMotionDuration,
            ProcessMode = Node.ProcessModeEnum.Always
        };
        slowMotionTimer.Timeout += () =>
        {
            slowMotionTimer.QueueFree();
            SetUpLerpTimer(player, NormalTimeScale,
                () =>
                {
                    Engine.TimeScale = NormalTimeScale;
                });
        };
        player.AddChild(slowMotionTimer);

        SetUpLerpTimer(player, _slowMotionTimeScale,
            () =>
            {
                Engine.TimeScale = _slowMotionTimeScale;
            });

        return State.Inactive;
    }

    private void SetUpLerpTimer(Node owner, float endTimeScale, Action callback)
    {
        var timer = new Timer
        {
            Name = "SlowMotionLerpTimer",
            Autostart = true,
            OneShot = false,
            WaitTime = _timeScaleLerpInterval,
            ProcessMode = Node.ProcessModeEnum.Always
        };
        timer.Timeout += () =>
        {
            var newTimeScale = Mathf.Lerp(Engine.TimeScale, endTimeScale, _timeScaleLerpWeight);
            Engine.TimeScale = newTimeScale;

            if (Mathf.Abs(endTimeScale - newTimeScale) < 0.01f)
            {
                callback();
                timer.QueueFree();
            }
        };

        owner.AddChild(timer);
    }
}