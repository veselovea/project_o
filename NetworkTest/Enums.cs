public enum GeneralCommand 
{
    PlayerInfo,
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
    None,
    CreatedNewRoom,
    ThePoolIsFullOrGameIsStarted,
    InvalidPoolCode,
    PlayerIsNull
}
public enum NetworkObjectType
{
    GameNetworkObject,
    ConnectionToPoolResult
}