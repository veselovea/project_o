public enum GeneralCommand 
{
    PlayerInfo,
    ConnectedPlayersInfo,
    GameCommand,
    Connect,
    Disconnect,
}

public enum GameCommand
{
    CreateBase,
    Born,
    Dead,
    Move,
    Attack,
    HittedAttack
}
public enum ConnectionStatus
{
    Failed,
    Success
}
public enum ConnectionStatusDetails
{
    Connected,
    CreatedNewRoom,
    ThePoolIsFullOrGameIsStarted,
    InvalidPoolCode,
    PlayerIsNull
}
public enum NetworkObjectType
{
    None,
    PlayerInfo,
    GameNetworkObject,
    ConnectionToPoolResult
}