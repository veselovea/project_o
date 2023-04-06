using System;
using System.Linq;
using System.Text;
using UnityEngine;

public class NetworkDataReceive : MonoBehaviour, IDBREceiveHandler
{
    public string _playerName;
    public static event Action<Eblock[]> OnLoadFortress;

    private DataClientSide _client;

    void Awake()
    {
        BaseCore.OnSaveFortress += SaveFortress;
        _client = new DataClientSide(this);
        _client.Start();
    }

    void OnDestroy()
    {
        _client.Stop();
    }

    void Start()
    {
        DataNetworkPacket packet = new DataNetworkPacket()
        {
            Command = DataCommand.GetFortress,
            Argument = _playerName
        };
        byte[] buffer = Encoding.ASCII.GetBytes(
            Serializer.GetJson(packet));
        _client.Send(buffer);
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
            PlayerName = _playerName,
            Blocks = fBlocks
        };
        string json = Serializer.GetJson(data);
        DataNetworkPacket packet = new DataNetworkPacket(DataCommand.SaveFortress, json);
        byte[] buffer = Encoding.ASCII.GetBytes(Serializer.GetJson(packet));
        _client.Send(buffer);
    }

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
        OnLoadFortress?.Invoke(eblocks);
    }
}
