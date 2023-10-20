namespace FlappyDragon.Player.Abilities;

using Godot;

public partial class Jump : Ability
{
    [Export]
    private float _jumpVelocity = -400.0f;

    [Export]
    private string _jumpCondition = "flap";

    protected override State ExecutePhysicsAbility(Player player, double delta)
    {
        var velocity = player.Velocity;
        velocity.Y = _jumpVelocity;
        player.Velocity = velocity;

        player.AnimationTree.TriggerCondition(_jumpCondition);

        return State.Inactive;
    }
}