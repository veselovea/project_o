using UnityEngine;

public interface INetworkModule
{
    public INetworkHandlerPlayer RemotePlayer { get; set; }
    public INetworkHandlerPlayer LocalPlayer { get; set; }
}

public class NetworkModule : MonoBehaviour, INetworkModule
{
    private ClientSideUnity _client;

    public INetworkHandlerPlayer RemotePlayer { get; set; }
    public INetworkHandlerPlayer LocalPlayer { get; set; }
}

public interface INetworkHandlerPlayer
{
    public void Attack();
    public void Connect();
    public void Disconnect();
    public void Move();
}

public class NetworkHandlerRemotePlayer : INetworkHandlerPlayer
{
    public void Attack()
    {

    }

    public void Connect()
    {

    }

    public void Disconnect()
    {

    }

    public void Move()
    {

    }
}

public class NetworkHandlerLocalPlayer : INetworkHandlerPlayer
{
    public void Attack()
    {

    }
    public void Connect()
    {

    }

    public void Disconnect()
    {

    }

    public void Move()
    {

    }
}
