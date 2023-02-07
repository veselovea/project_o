using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour
{
    public GameObject _playerPrefub;
    public string _playerName;

    private Action _executeInMainThread;
    private bool _isSending = false;

    private ClientSide _client;
    private GameObject _currentPlayer;
    private List<GameObject> _remotePlayers;
    private GameNetworkObject _networkObject;

    void Awake()
    {
        _currentPlayer = GameObject.Instantiate(_playerPrefub, new Vector3(0, 0, 0), Quaternion.identity);
        _currentPlayer.name = _playerName;
        _currentPlayer.GetComponent<PlayerScript>().PlayerMoveEventHandler += PlayerMoving;

        _remotePlayers = new List<GameObject>();

        _client = new ClientSide("192.168.0.2", 4000);
        _client.LogEvent += Debug.Log;
        _client.PlayerJoinEventHandler += PlayerJoining;
        _client.PlayerLeaveEventHandler += PlayerLeaving;
        _client.PlayerMoveEventHandler += PlayerMoving;
    }

    void OnDestroy()
    {
        _client.Stop();
        _client.Dispose();
    }

    void Update()
    {
        if (_executeInMainThread != null)
        {
            _executeInMainThread.Invoke();
            _executeInMainThread = null;
        }
    }

    private void PlayerJoining(string name)
    {
        _executeInMainThread += () =>
        {
            GameObject newPlayer = GameObject.Instantiate(_playerPrefub, new Vector3(0, 0, 0), Quaternion.identity);
            newPlayer.name = name;
            newPlayer.GetComponent<PlayerScript>()._isRemotePlayer = true;
            _remotePlayers.Add(newPlayer);
        };
    }

    private void PlayerLeaving(string name)
    {
        _executeInMainThread += () =>
        {
            GameObject player = _remotePlayers.Find(x => x.name == name);
            _remotePlayers.Remove(player);
            Destroy(player);
        };
    }

    private void PlayerMoving(string pos)
    {
        if (!_isSending)
            StartCoroutine(PlayerMovingCoroutine(pos));
    }

    private void PlayerMoving(string name, string pos)
    {
        _executeInMainThread += () =>
        {
            GameObject player = _remotePlayers.Find(x => x.name == name);
            string[] data = pos.Replace("(", "").Replace(")", "").Split(',');
            float x = Convert.ToSingle(data[0]), y = Convert.ToSingle(data[1]);
            Vector3 posPlayer = new Vector3(x, y, 0);
            player.transform.position = posPlayer;
        };
    }

    private IEnumerator PlayerMovingCoroutine(string pos)
    {
        _isSending = true;
        yield return new WaitForSecondsRealtime(10);
        _networkObject.Command = GameCommands.Move;
        _networkObject.CommandArgument = pos;
        //string json = Serializer.GetJson(_networkObject);
        //_ = _client.SendAsync(json);
        _isSending = false;
    }

}
