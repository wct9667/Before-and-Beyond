public struct Pause : IEvent
{
    public bool Paused;
}

//for the UI - resume
public struct UnPause : IEvent
{
}

public struct CharacterSwap : IEvent
{
    public Player.CharacterType CharacterType;
}