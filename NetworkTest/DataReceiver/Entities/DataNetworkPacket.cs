public class DataNetworkPacket
{
    public DataNetworkPacket()
    {

    }

    public DataNetworkPacket(DataCommand command)
    {
        Command = command;
    }

    public DataNetworkPacket(DataCommand command, string argument)
    {
        Command = command;
        Argument = argument;
    }

    public DataCommand Command { get; set; }
    public string Argument { get; set; }
}
