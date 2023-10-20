namespace FlappyDragon;

using System.Collections.Generic;

using Godot;

public partial class AnimationTreeEx : AnimationTree
{
    private readonly Queue<string> _triggeredConditions = new();

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (ProcessCallback == AnimationProcessCallback.Idle)
        {
            ResetTriggeredConditions();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (ProcessCallback == AnimationProcessCallback.Physics)
        {
            ResetTriggeredConditions();
        }
    }

    public void TriggerCondition(string condition)
    {
        SetCondition(condition, true);
        _triggeredConditions.Enqueue(condition);
    }

    public void SetCondition(string condition, bool value)
    {
        Set($"parameters/conditions/{condition}", value);
    }

    private void ResetTriggeredConditions()
    {
        while (_triggeredConditions.TryDequeue(out var condition))
        {
            SetCondition(condition, false);
        }
    }
}