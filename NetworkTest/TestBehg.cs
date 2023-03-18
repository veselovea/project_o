using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestBehg : MonoBehaviour
{
    public GameObject _player;

    private Action _mainThread;
    private Thread _thread;

    private CancellationTokenSource _tokenSource;
    private CancellationToken _token;

    void Awake()
    {
        _thread = new Thread(Test);
        _tokenSource = new CancellationTokenSource();
        _token = _tokenSource.Token;
    }

    void OnDestroy()
    {
        _tokenSource.Cancel();
    }

    void Start()
    {
        _thread.Start();
    }

    void Update()
    {
        if (_mainThread is not null)
        {
            _mainThread.Invoke();
            _mainThread = null;
        }
    }

    private void Test()
    {
        _mainThread += () => _player = GameObject.Instantiate(_player);
        bool isExcecuting = false;
        while (!_token.IsCancellationRequested)
        {
            if (isExcecuting)
                continue;
            isExcecuting = true;
            _mainThread += () =>
            {
                _player.transform.position += new Vector3(2, 2);
                isExcecuting = false;
            };
            Thread.Sleep(2000);
        }
    }
}
