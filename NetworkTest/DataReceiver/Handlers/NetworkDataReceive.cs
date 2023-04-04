using System;
using System.Text;
using UnityEngine;

public class NetworkDataReceive : MonoBehaviour, IFortressHandler, IDBREceiveHandler
{
    private DataClientSide _client;
    private IFortressHandler _playerBase;

    void Awake()
    {
        _client = new DataClientSide(this);
        _playerBase = GetComponent<IFortressHandler>();
    }

    public void SaveFortress(Eblock[] blocks)
    {
        FortressBlock[] fBlocks = new FortressBlock[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            fBlocks[i] = new FortressBlock
            {
                Name = blocks[i].BlockName,
                Position = blocks[i].BlockPosition
                .ToString()
                .Replace("(", "")
                .Replace(")", "")
            };
        }
        FortressData data = new FortressData()
        {
            PlayerName = "???",
            Blocks = fBlocks
        };
        string json = Serializer.GetJson(data);
        DataNetworkPacket packet = new DataNetworkPacket(DataCommand.SaveFortress, json);
        byte[] buffer = Encoding.ASCII.GetBytes(Serializer.GetJson(packet));
        _client.Send(buffer);
    }

    public void LoadFortress(Eblock[] blocks)
        => _playerBase.LoadFortress(blocks);

    public void LoadFortress(FortressData data)
    {
        Eblock[] eblocks = new Eblock[data.Blocks.Length];
        for (int i = 0; i < eblocks.Length; i++)
        {
            string[] temp = data.Blocks[i].Position.Split(',');
            float x = Convert.ToSingle(temp[0].Replace('.', ','));
            float y = Convert.ToSingle(temp[1].Replace('.', ','));
            float z = Convert.ToSingle(temp[2].Replace('.', ','));
            Vector3 position = new Vector3(x, y, z);
            eblocks[i] = new Eblock(data.Blocks[i].Name, position);
        }
        this.LoadFortress(eblocks);
    }
}
