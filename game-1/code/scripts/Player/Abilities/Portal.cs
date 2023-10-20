namespace FlappyDragon.Player.Abilities;

using Godot;

public partial class Portal : Ability
{
    [Export]
    private string _portalCondition = "portal";

    [Export]
    private float _portalDistance = 400.0f;

    /// <summary>
    /// This value controls when the player is teleported upward. It should be used to sync the movement with the animation.
    /// </summary>
    [Export]
    private float _teleportDelay = 0.5f;

    protected override State ExecutePhysicsAbility(Player player, double delta)
    {
        var portalDelayTimer = new Timer
        {
            Name = "PortalDelayTimer",
            Autostart = true,
            OneShot = true,
            WaitTime = _teleportDelay
        };
        portalDelayTimer.Timeout += () =>
        {
            var playerPosition = player.GlobalPosition;
            playerPosition.Y -= _portalDistance;
            player.GlobalPosition = playerPosition;

            portalDelayTimer.QueueFree();
        };
        player.AddChild(portalDelayTimer);

        player.AnimationTree.TriggerCondition(_portalCondition);

        return State.Inactive;
    }
}