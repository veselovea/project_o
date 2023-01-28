using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Для теста
    public GameObject network;

    public GameObject playerPrefab;
    public Action<Vector3> PlayerChanegPositionEvent;

    private Camera cameraMain;
    private PlayerUI playerUI;
    private PlayerInventory playerInventory;

    void Awake()
    {
        cameraMain = Camera.main;
        playerInventory = GetComponent<PlayerInventory>();
        playerUI = GetComponent<PlayerUI>();
        playerInventory.ResourceAmountChangedEvent += playerUI.OnResourceAmountChanged;
        playerUI.Initialize(playerInventory.AmountOfGameResources);

        // Можно удалить
        Network network = this.network.GetComponent<Network>();
        PlayerChanegPositionEvent += network.OnPlayerPositionChanged;
    }

    void FixedUpdate()
    {
        Move();
        LeftMouseButton();
    }

    void Update()
    {
        OthersButtonsOnKeyboard();
    }

    private void OthersButtonsOnKeyboard()
    {
        bool i = Input.GetKeyDown(KeyCode.I);
        if (i)
        {
#warning Удолить дебаг
            playerInventory.OpenOrCloseInventory();
            string t = playerInventory.IsOpen ? "open" : "close";
            Debug.Log($"Inventory is {t}");
        }
    }

    private void LeftMouseButton()
    {
        bool lkm = Input.GetKey(KeyCode.Mouse0);
        if (lkm)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                cameraMain.ScreenToWorldPoint(Input.mousePosition),
                -Vector3.forward);
            Vector3 pos = playerPrefab.transform.position;
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
                    playerInventory.AddGameResource(GameResources.Wood,
                        resource.GetResource(10));
                }
            }
        }
    }

    private void Move()
    {
        Vector3 pos = playerPrefab.transform.position;
        float speed = 0.06f;
        bool w, a, s, d;
        d = Input.GetKey(KeyCode.D);
        a = Input.GetKey(KeyCode.A);
        s = Input.GetKey(KeyCode.S);
        w = Input.GetKey(KeyCode.W);

        if (w)
        {
            playerPrefab.transform.position += new Vector3(0, speed);
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (a)
        {
            playerPrefab.transform.position += new Vector3(-speed, 0);
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (s)
        {
            playerPrefab.transform.position += new Vector3(0, -speed);
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (d)
        {
            playerPrefab.transform.position += new Vector3(speed, 0);
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (w && a)
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 45);
        else if (w && d)
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, -45);
        else if (s && a)
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, 135);
        else if (s && d)
            playerPrefab.transform.rotation = Quaternion.Euler(0, 0, -135);
        Vector3 cameraPos = Vector3.Lerp(
            cameraMain.transform.position,
            playerPrefab.transform.position,
            Time.deltaTime * 5);
        cameraPos.z = cameraMain.transform.position.z;
        cameraMain.transform.position = cameraPos;

        // Удолить
        PlayerChanegPositionEvent?.Invoke(playerPrefab.transform.position);
    }
}
