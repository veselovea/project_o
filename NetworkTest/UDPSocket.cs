using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor;

internal class State
{
    public byte[] buffer = new byte[UDPSocket.BufferSize];
}
internal class UDPSocket
{
    private Socket _socket;
    private EndPoint _remoteEndPoint;
    private State _state;
    private AsyncCallback _callback;

    public const int BufferSize = 4096;
    public Action<string> LogEvent;
    public bool IsDisposed { get; private set; } = false;

    public UDPSocket()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _state = new State();
        _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    public void Client(string address, ushort port)
    {
        _socket.Connect(IPAddress.Parse(address), port);
        Receive();
    }

    public void Send(string text)
    {
        byte[] data = Encoding.ASCII.GetBytes(text);
        _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket.EndSend(ar);
            LogEvent?.Invoke($"[<<] {text}");
        }, _state);
    }

    public void Dispose()
    {
        IsDisposed = true;
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        _socket.Dispose();
    }

    private void Receive()
    {
        _socket.BeginReceiveFrom(_state.buffer, 0, BufferSize, SocketFlags.None, ref _remoteEndPoint, _callback = (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket.EndReceiveFrom(ar, ref _remoteEndPoint);
            _socket.BeginReceiveFrom(so.buffer, 0, BufferSize, SocketFlags.None, ref _remoteEndPoint, _callback, so);
            string text = Encoding.ASCII.GetString(so.buffer, 0, bytes);
            LogEvent?.Invoke($"[>>] {_remoteEndPoint}: {text}");
            if (text == "Dispose")
            {
                Dispose();
            }
        }, _state);
    }
}
