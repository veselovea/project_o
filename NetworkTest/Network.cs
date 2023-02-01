using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Network : MonoBehaviour
{
    public GameObject _playerPrefub;

    private Action _executeMethodInMainThread;
    private bool _isSending = false;

    private ClientSide _client;
    private GameObject _currentPlayer;
    private List<GameObject> _remotePlayers;
    private GameNetworkObject _networkObject;

    void Awake()
    {
        _networkObject = new GameNetworkObject();
        _networkObject.OwnerName = "UNITY_PLAYER";
        _networkObject.Event = GameEvents.Join;
        _networkObject.Command = GameCommands.None;
        _currentPlayer = GameObject.Instantiate(_playerPrefub, new Vector3(0, 0, 0), Quaternion.identity);
        _currentPlayer.name = "UNITY_PLAYER";
        _currentPlayer.GetComponent<PlayerScript>().PlayerMoveEventHandler += PlayerMoving;
        _remotePlayers = new List<GameObject>();
        _client = new ClientSide("127.0.0.1", 4000);
        _client.LogEvent += Debug.Log;
        _client.PlayerJoinEventHandler += PlayerJoining;
        _client.PlayerLeaveEventHandler += PlayerLeaving;
        _client.PlayerMoveEventHandler += PlayerMoving;
        _client.Start();
        _ = _client.SendAsync(Serializer.GetJson(_networkObject));
    }

    void OnDestroy()
    {
        _client.Stop();
        _client.Dispose();
    }

    void Update()
    {
        if (_executeMethodInMainThread != null)
        {
            _executeMethodInMainThread.Invoke();
            _executeMethodInMainThread = null;
        }
    }

    private void PlayerJoining(string name)
    {
        _executeMethodInMainThread += () =>
        {
            GameObject newPlayer = GameObject.Instantiate(_playerPrefub, new Vector3(0, 0, 0), Quaternion.identity);
            newPlayer.name = name;
            newPlayer.GetComponent<PlayerScript>()._isRemotePlayer = true;
            _remotePlayers.Add(newPlayer);
        };
    }

    private void PlayerLeaving(string name)
    {
        _executeMethodInMainThread += () =>
        {
            GameObject player = _remotePlayers.Find(x => x.name == name);
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
        GameObject player = _remotePlayers.Find(x => x.name == name);
        Debug.Log(pos.Trim());
    }

    private IEnumerator PlayerMovingCoroutine(string pos)
    {
        _isSending = true;
        yield return new WaitForSecondsRealtime(10);
        _networkObject.Event = GameEvents.GameCommand;
        _networkObject.Command = GameCommands.Move;
        _networkObject.CommandArgument = pos;
        string json = Serializer.GetJson(_networkObject);
        _ = _client.SendAsync(json);
        _isSending = false;
    }

}
