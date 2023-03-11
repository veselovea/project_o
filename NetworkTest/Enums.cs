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
    Born,
    Dead,
    Move,
    Attack
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