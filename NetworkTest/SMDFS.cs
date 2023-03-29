using System;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Send My Data For Server
/// Класс для отправки ваших великолептных данных серверу.
/// </summary>
public class SMDFS<T> where T : class
{
    private UDPClientSide _client;
	private NetworkObjectType _type;
    private Action<T> _receiveCallback;

    /// <summary>
    /// Для базы игрока - PlayerBaseInfo
    /// Для подземелья - CaveGenerationInfo
    /// </summary>
    /// <param name="type">Тип отправляемого объекта</param>
    /// <param name="receiveCallback">Метод для обработки ответа от сервера</param>
    public SMDFS(NetworkObjectType type, Action<T> receiveCallback)
	{
		_type = type;
        _client = new UDPClientSide("90.188.226.136", 4500, 5130, Receive);
        _receiveCallback = receiveCallback;
    }

    public NetworkObjectType Type => _type;

    public void Send(T obj)
    {
        if (obj is null)
            throw new ArgumentException("Parameter obj has been null");
    }

    private Task Receive(byte[] buffer)
    {
        string json = Encoding.ASCII.GetString(buffer);
        T obj = Serializer.GetObject<T>(json);
        _receiveCallback.Invoke(obj);
        return Task.CompletedTask;
    }
}
