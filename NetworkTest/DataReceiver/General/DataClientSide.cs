using System;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;


public class DataClientSide
{
    public const int BufferSize = 1024;

    private TcpClient _client;
    private NetworkStream _stream;
    private bool _isClosed = true;
    private IDBREceiveHandler _dbHandler;

    public DataClientSide(IDBREceiveHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }

    public void Start()
    {
        Logger.Instance.WriteLogMessage("[**] Connected to 192.168.0.5:4050", LogLevel.Simple);
        _isClosed = false;
        _client = new TcpClient();
        _client.ReceiveBufferSize = BufferSize;
        _client.SendBufferSize = BufferSize;
        _client.Connect("192.168.0.5", 4050);
        _stream = _client.GetStream();
    }
    public void Stop()
    {
        Logger.Instance.WriteLogMessage("[**] Stopped", LogLevel.Simple);
        _isClosed = true;
        _client.Client.Shutdown(SocketShutdown.Both);
        _client.Client.Disconnect(false);
        _client.Close();
    }

    public async void Send(byte[] buffer)
    {
        byte[] endMarker = Encoding.ASCII.GetBytes("ENDMARKER");
        if (_isClosed)
            return;
        if (buffer.Length < BufferSize)
            await _stream.WriteAsync(buffer, 0, buffer.Length);
        else
        {
            int offset = 0;
            while (offset != buffer.Length)
            {
                int count = buffer.Length - offset > BufferSize ? BufferSize : buffer.Length - offset;
                await _stream.WriteAsync(buffer, offset, count);
                offset += count;
            }
        }
        await _stream.WriteAsync(endMarker);
        await Logger.Instance.WriteLogMessage($"[>>] Sent {buffer.Length} bytes", LogLevel.Advanced);
        Receive();
    }

    public async void Receive()
    {
        byte[] buffer = new byte[BufferSize];
        int bytes = 0;
        string value = string.Empty;
        while ((bytes = await _stream.ReadAsync(buffer, 0, BufferSize)) != 0)
        {
            value += Encoding.ASCII.GetString(buffer, 0, bytes);
            buffer = new byte[BufferSize];
            await Logger.Instance.WriteLogMessage($"[>>] Received \"{value}\" ({bytes} bytes)", LogLevel.Advanced);
            if (value.Contains("ENDMARKER"))
                break;
        }
        DataNetworkPacket packet = Serializer.GetObject<DataNetworkPacket>(value);
        FortressData data = Serializer.GetObject<FortressData>(packet.Argument);
        _dbHandler.LoadFortress(data);
    }
}
