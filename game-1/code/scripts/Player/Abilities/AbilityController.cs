namespace FlappyDragon.Player.Abilities;

using System.Collections.Generic;

using Godot;

public partial class AbilityController : Node
{
    [Export]
    private Player _playerNode;

    [Export]
    private Ability PrimaryAbility
    {
        get => GetAbility(Ability.AbilitySlot.Primary);
        set => SetAbility(Ability.AbilitySlot.Primary, value);
    }

    [Export]
    private Ability SecondaryAbility
    {
        get => GetAbility(Ability.AbilitySlot.Secondary);
        set => SetAbility(Ability.AbilitySlot.Secondary, value);
    }

    [Export]
    private Ability TertiaryAbility
    {
        get => GetAbility(Ability.AbilitySlot.Tertiary);
        set => SetAbility(Ability.AbilitySlot.Tertiary, value);
    }

    private readonly Dictionary<Ability.AbilitySlot, Ability> _abilities = new();
    private readonly Dictionary<Ability.AbilitySlot, Timer> _abilityTimers = new();

    public override void _Ready()
    {
        foreach (var slot in new[] { Ability.AbilitySlot.Primary, Ability.AbilitySlot.Secondary, Ability.AbilitySlot.Tertiary })
        {
            var timer = new Timer
            {
                Name = $"{slot.ToString()}AbilityTimer",
                Autostart = false,
                OneShot = true,
                WaitTime = 1.0f
            };
            _abilityTimers.Add(slot, timer);
            AddChild(timer);

            if (_abilities.TryGetValue(slot, out var ability) && ability.Cooldown > 0.0f)
            {
                timer.WaitTime = ability.Cooldown;
            }
        }
    }

    public bool ActivateAbility(Ability.AbilitySlot slot)
    {
        if (_abilities.TryGetValue(slot, out var ability) && _abilityTimers.TryGetValue(slot, out var timer) && timer.IsStopped())
        {
            if (ability.Cooldown > 0)
            {
                timer.Start();
            }
            ability.Activate(_playerNode);
            return true;
        }

        return false;
    }

    public Ability GetAbility(Ability.AbilitySlot slot)
    {
        if (_abilities.TryGetValue(slot, out var ability))
        {
            return ability;
        }

        return null;
    }

    public void SetAbility(Ability.AbilitySlot slot, Ability ability)
    {
        _abilities[slot] = ability;
        if (_abilityTimers.TryGetValue(ability.Slot, out var timer))
        {
            timer.Stop();
            timer.WaitTime = ability.Cooldown;
        }
    }

    public override void _Process(double delta)
    {
        foreach (var ability in _abilities.Values)
        {
            ability.Process(_playerNode, delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var ability in _abilities.Values)
        {
            ability.PhysicsProcess(_playerNode, delta);
        }
    }

    public override void _ExitTree()
    {
        // Clean up after the slow-motion ability if the player died during the ability activation
        Engine.TimeScale = 1.0f;
    }
}