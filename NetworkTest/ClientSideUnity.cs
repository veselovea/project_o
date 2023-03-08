using System.Threading.Tasks;

public class ClientSideUnity : ClientSide
{
    private INetworkModule _networkModule;

    public ClientSideUnity(string address, ushort port, INetworkModule networkModule) : base(address, port)
    {
        _networkModule = networkModule;
    }

    protected override Task ReceiveHandler(string text)
    {
        return null;
    }
}
