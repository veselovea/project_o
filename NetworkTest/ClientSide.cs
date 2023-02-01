using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

internal class ClientSide
{
    public const int BufferSize = 4096;
    public Action<string> LogEvent;
    public Action<string> PlayerJoinEventHandler;
    public Action<string> PlayerLeaveEventHandler;
    public Action<string, string> PlayerMoveEventHandler;

    private Socket _socket;
    private EndPoint _remote;
    private bool _isStoped = false;

    public ClientSide(string address, ushort port)
    {
        _remote = new IPEndPoint(IPAddress.Parse(address), port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _socket.Connect(_remote);
    }

    public void Start()
    {
        if (_isStoped)
        {
            _socket.Close();
            _socket.Dispose();

            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(_remote);
            _isStoped = false;
        }
        Task.Run(ReceiveAsync);
    }

    public void Stop()
    {
        _isStoped = true;
        if (_socket.Connected)
            _socket.Shutdown(SocketShutdown.Both);
    }

    public void Dispose()
    {
        _socket.Close();
        _socket.Dispose();
    }

    public async Task SendAsync(string text)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(text);
        await _socket.SendToAsync(buffer, SocketFlags.None, _remote);
        LogEvent?.Invoke($"[<<] {text}");
    }

    private async Task ReceiveAsync()
    {
        while (true)
        {
            byte[] buffer = new byte[BufferSize];
            string text = "";
            do
            {
                await _socket.ReceiveFromAsync(buffer, SocketFlags.None, _remote);
                text += Encoding.ASCII.GetString(buffer);
            } while (_socket.Available > 0);
            LogEvent?.Invoke($"[>>] {text}");
            await ReceiveHandlerAsync(text);
        }
    }

    private Task ReceiveHandlerAsync(string text)
    {
        return Task.Run(() =>
        {
            GameNetworkObject networkObject = Serializer.GetObject<GameNetworkObject>(text);
            switch (networkObject.Event)
            {
                case GameEvents.None:
                    break;
                case GameEvents.Join:
                    PlayerJoinEventHandler?.Invoke(networkObject.OwnerName);
                    break;
                case GameEvents.Leave:
                    PlayerLeaveEventHandler?.Invoke(networkObject.OwnerName);
                    break;
                case GameEvents.GameCommand:
                    switch (networkObject.Command)
                    {
                        case GameCommands.None:
                            break;
                        case GameCommands.Move:
                            break;
                        case GameCommands.Attack:
                            break;
                        default:
                            LogEvent?.Invoke("[XX] Unknow command");
                            break;
                    }
                    break;
                default:
                    LogEvent?.Invoke("[XX] Unknow event");
                    break;
            }
        });
    }
}