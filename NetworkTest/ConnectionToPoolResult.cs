internal enum ConnectionStatus
{
    Failed,
    Success
}
internal enum ConnectionStatusDetails
{
    None,
    CreatedNewRoom,
    ThePoolIsFull,
    InvalidPoolCode
}

internal class ConnectionPoolResult
{
    public string GameCode { get; set; } = null;
    public ConnectionStatus Status { get; set; }
    public ConnectionStatusDetails Details { get; set; }
}