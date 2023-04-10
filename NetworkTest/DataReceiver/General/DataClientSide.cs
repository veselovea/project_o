using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;

public class DataClientSide
{
    public const int BufferSize = 1024;

    private TcpClient _client;
    private NetworkStream _stream;
    private bool _isClosed = true;
    private IDBREceiveHandler _dbHandler;

    private bool _isLocked = false;
    private List<byte[]> _sendQueue;
    private byte[][] _toSend;

    public DataClientSide(IDBREceiveHandler dbHandler)
    {
        _dbHandler = dbHandler;
        _sendQueue = new List<byte[]>();
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
        _stream.Write(Encoding.ASCII.GetBytes("BYEFROMCLIENT"));
        Logger.Instance.WriteLogMessage("[**] Stopped", LogLevel.Simple);
        _isClosed = true;
        _client.Close();
    }

    public async ValueTask<bool> Send(byte[] buffer)
    {
        if (_isClosed)
            return false;
        if (_isLocked)
        {
            _sendQueue.Add(buffer);
            return false;
        }
        _isLocked = true;
        byte[] endMarker = Encoding.ASCII.GetBytes("ENDMARKER");
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

        _isLocked = false;
        StartQueue();
        return true;
    }

    private async void Receive()
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
        value = value.Replace("ENDMARKER", "");
        DataNetworkPacket packet = Serializer.GetObject<DataNetworkPacket>(value);
        if (packet.Command == DataCommand.GetFortress)
        {
            FortressData data = Serializer.GetObject<FortressData>(packet.Argument);
            _dbHandler.LoadFortress(data);
        }
    }

    private void StartQueue()
    {
        _toSend = _sendQueue.ToArray();
        _sendQueue = new List<byte[]>();
        foreach (var item in _toSend)
        {
            Send(item);
        }
        _toSend = null;
    }
}
