﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class ClientSide
{
    public const int BufferSize = 4096;

    private Socket _socket;
    private EndPoint _remote;
    private bool _isStoped = false;

    public Logger Logger { get; set; }

    public ClientSide(string address, ushort port)
    {
        Logger = new Logger(LogLevel.Simple);
        _remote = new IPEndPoint(IPAddress.Parse(address), port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _socket.Connect(_remote);
        Logger.PrintLog("[**] Client created", LogLevel.Simple);
        Logger.PrintLog($"[**] Client connected to {address}", LogLevel.Simple);
    }

    public void Start()
    {
        if (_isStoped)
        {
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
        _socket.Close();
        _socket.Dispose();
        Logger.PrintLog($"[**] Client disconnected", LogLevel.Simple);
        Logger.PrintLog("[**] Client stopped", LogLevel.Simple);
    }

    public async Task SendAsync(string text)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(text);
        await _socket.SendToAsync(buffer, SocketFlags.None, _remote);
        Logger.PrintLog($"[**] Sent {buffer.Length} bytes", LogLevel.Base);
        Logger.PrintLog($"[**] Sent {text}", LogLevel.Advanced);
    }

    private async Task ReceiveAsync()
    {
        while (true)
        {
            byte[] buffer = new byte[BufferSize];
            string text = "";
            int bytesAmount = 0;
            do
            {
                await _socket.ReceiveFromAsync(buffer, SocketFlags.None, _remote);
                text += Encoding.ASCII.GetString(buffer);
                bytesAmount += buffer.Length;
            } while (_socket.Available > 0);
            await ReceiveHandler(text);
            Logger.PrintLog($"[**] Received {bytesAmount} bytes", LogLevel.Base);
            Logger.PrintLog($"[**] Received {text}", LogLevel.Advanced);
        }
    }

    protected virtual Task ReceiveHandler(string text)
    {
        return null;
    }
}
