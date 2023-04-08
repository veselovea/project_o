using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class UDPClientSide
{
    public const int BufferSize = 1024;

    private UdpClient _client;
    private Thread _receiver;
    private IPEndPoint _remote;
    private bool _isFirstInit = true;
    private bool _isStarted = false;
    private bool _isDisposed = false;
    private ushort _localPort;

    public UDPClientSide(string address, ushort remotePort, ushort localPort, Func<byte[], Task> handler = null)
    {
        if (localPort < 1024)
            throw new ArgumentOutOfRangeException("Parameter localPort should be more than 1024");
        _client = new UdpClient(localPort);
        _client.Client.ReceiveBufferSize = BufferSize;
        _client.Client.SendBufferSize = BufferSize;
        _client.Client.Blocking = false;
        _remote = new IPEndPoint(IPAddress.Parse(address), remotePort);
        _receiver = new Thread(async () => await ReceiverAsync(handler is null ? ReceiveHandler : handler));
        _localPort = localPort;
        Logger = new Logger(LogLevel.Simple);
    }

    public Logger Logger { get; set; }
    public bool IsStarted => _isStarted;

    public void Start()
    {
        if (_isStarted)
        {
            Logger.WriteLogMessage("[**] The client is already running", LogLevel.Simple);
            return;
        }
        if (!_isFirstInit)
        {
            _client = new UdpClient(_localPort);
            _client.Client.ReceiveBufferSize = BufferSize;
            _client.Client.SendBufferSize = BufferSize;
            _client.Client.Blocking = false;
        }
        else
            _isFirstInit = false;
        _isDisposed = false;
        _client.Connect(_remote);
        _receiver.Start();
        _isStarted = true;
        Logger.WriteLogMessage("[**] Client created", LogLevel.Simple);
        Logger.WriteLogMessage($"[**] Client connected to {_remote}", LogLevel.Simple);
    }

    public void Stop()
    {
        if (!_isStarted)
        {
            Logger.WriteLogMessage("[**] The client has already been stopped", LogLevel.Simple);
            return;
        }

        _isStarted = false;
        _isDisposed = true;
        _client.Close();
        _client.Dispose();
        Logger.WriteLogMessage($"[**] Client disconnected", LogLevel.Simple);
        Logger.WriteLogMessage("[**] Client stopped", LogLevel.Simple);
    }

    public async Task SendAsync(byte[] data)
    {
        if (!_isDisposed)
        {
            await Logger.WriteLogMessage($"[<<] Sent {data.Length} bytes", LogLevel.Base);
            await _client.SendAsync(data, data.Length);
        }
    }

    private async Task ReceiverAsync(Func<byte[], Task> handler)
    {
        while (_isStarted)
        {
            try
            {
                UdpReceiveResult result = await _client.ReceiveAsync();
                if (!result.RemoteEndPoint.Equals(_remote))
                    continue;
                byte[] buffer = result.Buffer;
                await Logger.WriteLogMessage($"[>>] Received {buffer.Length} bytes", LogLevel.Base);
                await handler?.Invoke(buffer);
            }
            catch (Exception e)
            {
                await Logger.WriteLogMessage($"[XX] {e.Message} {e.Source} {e.StackTrace}", LogLevel.Simple);
            }
        }
    }

    protected virtual Task ReceiveHandler(byte[] data)
    {
        return Task.Run(() => { });
    }
}
