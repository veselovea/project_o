using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BotManager
{
    private List<Bot> _bots;
    private Thread _botHandler;
    private bool _stopMarker = false;
    private bool _isStarted = false;

    public BotManager(int botsCount, ushort startPort)
    {
        _bots = new List<Bot>(botsCount);
        _botHandler = new Thread(HandleBots);
        for (int i = 0; i < botsCount; i++)
            _bots.Add(new Bot($"Bot #{i}", (ushort)(startPort + i)));
    }

    public bool IsStarted => _isStarted;

    public async void StartSession()
    {
        if (_isStarted)
            return;
        _isStarted = true;
        _stopMarker = false;
        foreach (var bot in _bots)
            await bot.CreateBot();
        _botHandler.Start();
    }

    public void StopSession()
    {
        _stopMarker = true;
        foreach (var bot in _bots)
            bot.DestroyBot();
        _isStarted = false;
    }

    private async void HandleBots()
    {
        while (!_stopMarker)
        {
            foreach (var bot in _bots)
            {
                await bot.SendBotPostiton();
                if (_stopMarker)
                    break;
            }
        }
    }
}

public class Bot : UDPClientSide
{
    private Vector3 _botPosition;
    private PlayerInfo _botInfo;

    private System.Random _rnd = new System.Random();

    public Bot(string name, ushort port) : base("192.168.0.2", 4000, port)
    {
        _botPosition = Vector3.zero;
        _botInfo = new PlayerInfo()
        {
            Name = name
        };
    }

    public async Task CreateBot()
    {
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _botInfo,
            Command = GeneralCommand.PlayerInfo,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await base.SendAsync(buffer);
        packet.Command = GeneralCommand.Connect;
        json = Serializer.GetJson(packet);
        buffer = Encoding.ASCII.GetBytes(json);
        await base.SendAsync(buffer);
    }

    public void DestroyBot() =>
        base.Stop();

    public async Task SendBotPostiton()
    {
        await BotMove();
        PlayerTransform transform = new PlayerTransform()
        {
            PositionX = _botPosition.x,
            PositionY = _botPosition.y,
            PositionZ = _botPosition.z,
            RotationX = 0,
            RotationY = 0,
            RotationZ = 0,
        };
        GameNetworkObject move = new GameNetworkObject()
        {
            Command = GameCommand.Move,
            CommandArgument = Serializer.GetJson(transform)
        };
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _botInfo,
            Command = GeneralCommand.GameCommand,
            Type = NetworkObjectType.GameNetworkObject,
            Data = Serializer.GetJson(move)
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await base.SendAsync(buffer);
    }

    private Task BotMove()
    {   
        float x = _rnd.Next(-2, 2);
        float y = _rnd.Next(-2, 2);
        _botPosition += new Vector3(x, y);
        return Task.CompletedTask;
    }
}
