using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.PackageManager;
using UnityEngine;

public class NetworkHandlerRemotePlayer : ExecuteTasksInMainThread, IDBREceiveHandler
{
    private GameObject _playerPrefub;
    private List<GameObject> _remotePlayers;

    private object _baseInitializer;
    private DataClientSide _dataSide;

    public NetworkHandlerRemotePlayer(GameObject playerPrefub, object baseInitializer)
    {
        _playerPrefub = playerPrefub;
        _remotePlayers = new List<GameObject>(8);
        _baseInitializer = baseInitializer;
        _dataSide = new DataClientSide(this);
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

    }

    public void Connect(PlayerInfo playerInfo)
    {
        Born(playerInfo);
        DataNetworkPacket packet = new DataNetworkPacket()
        {
            Command = DataCommand.GetFortress,
            Argument = playerInfo.Name
        };
        byte[] buffer = Encoding.ASCII.GetBytes(
            Serializer.GetJson(packet));
        _dataSide.Send(buffer);
    }

    public void Disconnect(PlayerInfo playerInfo)
    {
        Dead(playerInfo);

    }

    public void Born(PlayerInfo playerInfo)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            if (_remotePlayers.Find(x => x.name == playerInfo.Name) is not null)
                return;
            GameObject player = GameObject.Instantiate<GameObject>(_playerPrefub);
            player.GetComponent<PlayerScript>()._isRemotePlayer = true;
            player.name = playerInfo.Name;
            _remotePlayers.Add(player);
        };
        base.AddTaskToQueue(action);

    }
    // Можно не удалять объект, а просто скрывать и перемещать в начальную позицию

    public void Dead(PlayerInfo playerInfo)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
            if (player is null)
                return;
            GameObject.Destroy(player);
            _remotePlayers.Remove(player);
        };
        base.AddTaskToQueue(action);
    }

    public void Move(PlayerInfo playerInfo, string pos)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            PlayerTransform transform = Serializer.GetObject<PlayerTransform>(pos);
            Vector3 position = new Vector3(transform.PositionX, transform.PositionY, transform.PositionZ);
            GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
            player.transform.position = position;
            player.transform.rotation = new Quaternion(transform.RotationX, transform.RotationY, transform.RotationZ, 1);
        };
        base.AddTaskToQueue(action);
    }

    public void Attack(PlayerInfo playerInfo, string data)
    {

    }

    public void HittedAttack(PlayerInfo playerInfo, string data)
    {

    }
}


