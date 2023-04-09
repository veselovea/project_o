﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NetworkHandlerRemotePlayer : ExecuteTasksInMainThread, IDBREceiveHandler
{
    private GameObject _playerPrefub;
    private List<RemotePlayer> _remotePlayers;

    private BaseInitializer _baseInitializer;
    private DataClientSide _dataSide;

    public NetworkHandlerRemotePlayer(GameObject playerPrefub, BaseInitializer baseInitializer)
    {
        _playerPrefub = playerPrefub;
        _remotePlayers = new List<RemotePlayer>(8);
        _baseInitializer = baseInitializer;
        _dataSide = new DataClientSide(this);
        _dataSide.Start();
    }

    public void OnDestroy()
    {
        _dataSide.Stop();
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
        RemotePlayer player = _remotePlayers.Find(x => x.Info.Name == data.PlayerName);
        PlayerBaseObject playerBase = new PlayerBaseObject
        {
            Player = player.Prefub,
            PlayerBaseBlocks = eblocks
        };
        player.StartPosition = _baseInitializer.SetupBase(playerBase);
    }

    public void Connect(PlayerInfo playerInfo)
    {
        RemotePlayer player = new RemotePlayer
        {
            Info = playerInfo
        };
        _remotePlayers.Add(player);
        Born(playerInfo);
    }

    public void Disconnect(PlayerInfo playerInfo)
    {
        RemotePlayer player = _remotePlayers.Find(x => x.Info.Name == playerInfo.Name);
        Dead(player.Prefub);
        _remotePlayers.Remove(player);
    }

    public void Born(PlayerInfo playerInfo)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            RemotePlayer player = _remotePlayers.Find(x => x.Info.Name == playerInfo.Name);
            if (player.Prefub is null)
            {
                player.Prefub = GameObject.Instantiate<GameObject>(_playerPrefub);
                player.Prefub.GetComponent<PlayerScript>()._isRemotePlayer = true;
                player.Prefub.name = playerInfo.Name;
                OnPlayerConnected(playerInfo.Name);
            }
            else
            {
                player.Prefub.transform.position = player.StartPosition;
                player.Prefub.SetActive(true);
            }
        };
        base.AddTaskToQueue(action);
    }
    // Можно не удалять объект, а просто скрывать и перемещать в начальную позицию

    public void Dead(PlayerInfo playerInfo)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            RemotePlayer player = _remotePlayers.Find(x => x.Info.Name == playerInfo.Name);
            if (player is null)
                return;
            player.Prefub.SetActive(false);
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
            RemotePlayer player = _remotePlayers.Find(x => x.Info.Name == playerInfo.Name);
            player.Prefub.transform.position = position;
            player.Prefub.transform.rotation = new Quaternion(transform.RotationX, transform.RotationY, transform.RotationZ, 1);
        };
        base.AddTaskToQueue(action);
    }

    public void Attack(PlayerInfo playerInfo, string data)
    {

    }

    public void HittedAttack(PlayerInfo playerInfo, string data)
    {

    }

    private void OnPlayerConnected(string playerName)
    {
        DataNetworkPacket packet = new DataNetworkPacket()
        {
            Command = DataCommand.GetFortress,
            Argument = playerName
        };
        byte[] buffer = Encoding.ASCII.GetBytes(
            Serializer.GetJson(packet));
        _dataSide.Send(buffer);
    }

    private void Dead(GameObject prefub)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            GameObject.Destroy(prefub);
        };
        base.AddTaskToQueue(action);
    }
}


