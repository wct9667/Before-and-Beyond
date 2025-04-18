using System.Collections.Generic;
using Ability;
using JetBrains.Annotations;
using Player;

/// <summary>
/// Pause the game
/// </summary>
public struct Pause : IEvent
{
    public bool Paused;
}

/// <summary>
/// Resume the game
/// </summary>
public struct UnPause : IEvent
{
}

/// <summary>
/// Event to call when characters are swapped
/// </summary>
public struct CharacterSwap : IEvent
{
    public Player.CharacterType CharacterType;
}

/// <summary>
/// Unlike the character swap, this event is fired after the character has been swapped.
/// Think order of operations
/// </summary>
public struct AbilitiesSwapped : IEvent
{
    [CanBeNull] public Dictionary<AbstractAbility, AbilityData> cooldowns;
}

public struct HelmetSettingChange : IEvent{}

public struct ChangePlayerHealth : IEvent
{
    public float healthChange;
    [CanBeNull] public float maxHealth;
}

public struct ChangePlayerHealthUI : IEvent
{
    public float playerHealth;
    public float playerMaxHealth;
}
