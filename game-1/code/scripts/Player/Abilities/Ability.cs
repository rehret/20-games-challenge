namespace FlappyDragon.Player.Abilities;

using Godot;

public abstract partial class Ability : Resource
{
    [Signal]
    public delegate void AbilityActivationStartedEventHandler(Player player, Ability ability);

    [Signal]
    public delegate void AbilityActivationEndedEventHandler(Player player, Ability ability);

    [Export]
    public AbilitySlot Slot;

    [Export]
    public float Cooldown;

    [Export]
    public Texture2D ButtonIcon;

    private State _state = State.Inactive;

    public void Activate(Player player)
    {
        _state = State.Active;
        EmitSignal(SignalName.AbilityActivationStarted, player, this);
    }

    public void Process(Player player, double delta)
    {
        if (_state != State.Active) return;

        _state = ExecuteAbility(player, delta);
        if (_state == State.Inactive)
        {
            EmitSignal(SignalName.AbilityActivationEnded, player, this);
        }
    }

    public void PhysicsProcess(Player player, double delta)
    {
        if (_state != State.Active) return;

        _state = ExecutePhysicsAbility(player, delta);
        if (_state == State.Inactive)
        {
            EmitSignal(SignalName.AbilityActivationEnded, player, this);
        }
    }

    protected virtual State ExecuteAbility(Player player, double delta) => State.Active;
    protected virtual State ExecutePhysicsAbility(Player player, double delta) => State.Active;

    public enum AbilitySlot
    {
        Primary,
        Secondary,
        Tertiary
    }

    protected enum State
    {
        Active,
        Inactive
    }
}
