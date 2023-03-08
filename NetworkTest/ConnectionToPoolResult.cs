public class ConnectionPoolResult
{
    public string GameCode { get; set; } = null;
    public ConnectionStatus Status { get; set; }
    public ConnectionStatusDetails Details { get; set; }
}
