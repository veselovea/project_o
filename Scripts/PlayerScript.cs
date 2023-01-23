using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public int amoutTree = 0;
    private bool isEnter = false;
    private TextMeshProUGUI amoutTreeText;
    private Camera cameraMain;

    void Start()
    {
        cameraMain = Camera.main;
        GameObject canvas = GameObject.Find("Canvas");
        var text = canvas.transform.GetChild(0);
        amoutTreeText = text.GetComponent<TextMeshProUGUI>();
        amoutTreeText.text = amoutTree.ToString();
    }

    void FixedUpdate()
    {
        Move();
        bool lkm = Input.GetKey(KeyCode.Mouse0);
        if (lkm && isEnter)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                cameraMain.ScreenToWorldPoint(Input.mousePosition),
                -Vector2.up);
            var tree = hit.transform.parent.GetComponent<TreeScript>();
            tree.health -= 10;
            amoutTree += 10;
            amoutTreeText.text = amoutTree.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        isEnter = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
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
    }
}
