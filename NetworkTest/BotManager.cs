using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
        {
            Bot bot = new Bot($"Bot #{i}", (ushort)(startPort + i));
            bot.LogEvent += Console.WriteLine;
            _bots.Add(bot);
        }
    }

    public bool IsStarted => _isStarted;

    public void StartSession(string address)
    {
        if (_isStarted)
            return;
        _isStarted = true;
        _stopMarker = false;
        foreach (var bot in _bots)
        {
            bot.CreateBot(address);
            Thread.Sleep(200);
        }
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
                if (bot.IsCreated)
                    await bot.SendBotPostiton();
                if (_stopMarker)
                    break;
            }
            Thread.Sleep(50);
        }
    }
}

public class Bot
{
    private UdpClient _bot;

    private Vector3 _botPosition;
    private PlayerInfo _botInfo;
    private bool _isCreated = false;

    private System.Random _rnd = new System.Random();

    public Bot(string name, ushort port)
    {
        _bot = new UdpClient(port);
        _botPosition = Vector3.zero;
        _botInfo = new PlayerInfo()
        {
            Name = name
        };
    }

    public bool IsCreated => _isCreated;
    public Action<string> LogEvent { get; set; }

    public async void CreateBot(string address)
    {
        _bot.Connect(address, 4000);
        var packet = new GameNetworkPacket()
        {
            Player = _botInfo,
            Command = GeneralCommand.PlayerInfo,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await _bot.SendAsync(buffer, buffer.Length);
        LogEvent?.Invoke($"[<<] {_botInfo.Name} requested player info (ip)");

        UdpReceiveResult receive = await _bot.ReceiveAsync();
        buffer = receive.Buffer;
        json = Encoding.ASCII.GetString(buffer);
        var comingPacket = Serializer.GetObject<GameNetworkPacket>(json);
        _botInfo = comingPacket.Player;
        LogEvent?.Invoke($"[>>] {_botInfo.Name} result: {_botInfo.IP}");

        packet.Player = comingPacket.Player;
        packet.Command = GeneralCommand.Connect;
        json = Serializer.GetJson(packet);
        buffer = Encoding.ASCII.GetBytes(json);
        await _bot.SendAsync(buffer, buffer.Length);
        LogEvent?.Invoke($"[<<] {_botInfo.Name} request connect to game");

        receive = await _bot.ReceiveAsync();
        buffer = receive.Buffer;
        json = Encoding.ASCII.GetString(buffer);
        comingPacket = Serializer.GetObject<GameNetworkPacket>(json);
        var connectionResult = Serializer.GetObject<ConnectionPoolResult>(comingPacket.Data);
        _botInfo.GameCode = connectionResult.GameCode;
        LogEvent?.Invoke($"[>>] {_botInfo.Name} result: {_botInfo.GameCode}");

        _isCreated = true;
    }

    public void DestroyBot()
    {
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _botInfo,
            Command = GeneralCommand.Disconnect,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        _bot.SendAsync(buffer, buffer.Length);
        _bot.Close();
        _bot.Dispose();
        _bot = null;
    }

    public async Task SendBotPostiton()
    {
        BotMove();
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
        await _bot.SendAsync(buffer, buffer.Length);
        LogEvent?.Invoke($"[<<] {_botInfo.Name} sent his position");
    }

    private void BotMove()
    {
        float x = _rnd.Next(-4, 4) / 10f;
        float y = _rnd.Next(-4, 4) / 10f;
        _botPosition += new Vector3(x, y, 0);
    }
}