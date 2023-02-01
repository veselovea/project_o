using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject _playerPrefab;
    public bool _isRemotePlayer = false;
    public Action<string> PlayerMoveEventHandler;

    private Camera _cameraMain;
    private PlayerUI _playerUI;
    private PlayerInventory _playerInventory;

    void Awake()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _playerUI = GetComponent<PlayerUI>();
        if (!_isRemotePlayer)
            _cameraMain = Camera.main;
    }

    void Start()
    {
        if (!_isRemotePlayer)
        {
            _playerInventory.ResourceAmountChangedEvent += _playerUI.OnResourceAmountChanged;
            _playerUI.Initialize(_playerInventory.AmountOfGameResources);
        }
        else
        {
            Transform inventoryUI = transform.Find("InventoryUI");
            inventoryUI.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!_isRemotePlayer)
        {
            Move();
            LeftMouseButton();
        }
    }

    void Update()
    {
        if (!_isRemotePlayer)
            OthersButtonsOnKeyboard();
    }

    private void OthersButtonsOnKeyboard()
    {
        bool i = Input.GetKeyDown(KeyCode.I);
        if (i)
        {
#warning Удолить дебаг
            _playerInventory.OpenOrCloseInventory();
            string t = _playerInventory.IsOpen ? "open" : "close";
            Debug.Log($"Inventory is {t}");
        }
    }

    private void LeftMouseButton()
    {
        bool lkm = Input.GetKey(KeyCode.Mouse0);
        if (lkm)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                _cameraMain.ScreenToWorldPoint(Input.mousePosition),
                -Vector3.forward);
            Vector3 pos = _playerPrefab.transform.position;
            float dist = Vector3.Distance(pos, hit.point);
            if (dist > 2)
                return;
            // if selected object is sourceable
            if (hit.transform)
            {
                GameResource resource = hit.transform?.parent?.GetComponent<GameResource>();
                if (resource)
                {
                    Debug.Log("IS RESOURCE");
                    _playerInventory.AddGameResource(GameResources.Wood,
                        resource.GetResource(10));
                }
            }
        }
    }

    private void Move()
    {
        Vector3 pos = _playerPrefab.transform.position;
        float speed = 0.06f;
        bool w, a, s, d;
        d = Input.GetKey(KeyCode.D);
        a = Input.GetKey(KeyCode.A);
        s = Input.GetKey(KeyCode.S);
        w = Input.GetKey(KeyCode.W);

        if (w)
        {
            _playerPrefab.transform.position += new Vector3(0, speed);
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (a)
        {
            _playerPrefab.transform.position += new Vector3(-speed, 0);
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (s)
        {
            _playerPrefab.transform.position += new Vector3(0, -speed);
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (d)
        {
            _playerPrefab.transform.position += new Vector3(speed, 0);
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (w && a)
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 45);
        else if (w && d)
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, -45);
        else if (s && a)
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 135);
        else if (s && d)
            _playerPrefab.transform.rotation = Quaternion.Euler(0, 0, -135);
        Vector3 cameraPos = Vector3.Lerp(
            _cameraMain.transform.position,
            _playerPrefab.transform.position,
            Time.deltaTime * 5);
        cameraPos.z = _cameraMain.transform.position.z;
        _cameraMain.transform.position = cameraPos;

        PlayerMoveEventHandler?.Invoke(_playerPrefab.transform.position.ToString());
    }
}
