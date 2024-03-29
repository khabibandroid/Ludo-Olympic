public class Enums
{

}


public enum AdLocation
{
    GameStart,
    GameOver,
    LevelComplete,
    Pause,
    FacebookFriends,
    GameFinishWindow,
    StoreWindow,
    GamePropertiesWindow

};

public enum MyGameType
{
    TwoPlayer, FourPlayer, Private
};

public enum MyGameMode
{
    Classic, Master, Quick
}

public enum EnumPhoton
{
    BeginPrivateGame = 171,
    NextPlayerTurn = 172,
    StartWithBots = 173,
    StartGame = 174,
    SendChatMessage = 175,
    SendChatEmojiMessage = 176,
    AddFriend = 177,
    FinishedGame = 178,
    SynchronizeTurn = 179,
    SetDuration = 180,
    OnlineGameFinished = 181,
    NextPlayerTurnWithName = 182,
    NeedDuration = 183,
    NeedSynchronize = 184,
    SynchronizeRecentPlayer = 185,
    DisconnectPlayer = 186,
    SynchronizeScore = 187,
}

public enum EnumGame
{
    DiceRoll = 50,
    PawnMove = 51,
    PawnRemove = 52,
    playerLeft = 53,
    SetPawns = 55,
}

public enum MyPawnColor
{
    Blue,
    Red,
    Green,
    Yellow
}