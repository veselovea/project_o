using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

public class Network : MonoBehaviour
{
    private UDPSocket socket;
    private bool isSending = false;

    void Start()
    {
        socket = new UDPSocket();
        socket.LogEvent += Debug.Log;
        socket.Client("127.0.0.1", 4000);
        socket.Send("Test connection");
    }

    public void OnPlayerPositionChanged(Vector3 pos)
    {
        if (!isSending && !socket.IsDisposed)
            StartCoroutine(SendPosition(pos));
    }

    private IEnumerator SendPosition(Vector3 pos)
    {
        isSending = true;
        yield return new WaitForSecondsRealtime(10);
        isSending = false;
        socket.Send(pos.ToString());
    }
}
