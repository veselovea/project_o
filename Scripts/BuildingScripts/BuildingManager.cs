using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    private Camera cameraMain;
    private bool buildMode = false;
    private Vector2 cameraMoveVector;
    private float cameraSpeed;
    private GameObject curentBlock;

    public GameObject[] blockPrefabs;
    public string baseSceneName;
    public string caveSceneName;

    void Start()
    {
        cameraMain = Camera.main;
        buildMode = false;
        baseSceneName = "DNA_Scene";
        caveSceneName = "ChunkTestScene";
        cameraSpeed = 10f;
        curentBlock = blockPrefabs[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !buildMode) buildMode = true;
        else if (Input.GetKeyDown(KeyCode.Tab) && buildMode) buildMode = false;

        if (SceneManager.GetActiveScene().name == baseSceneName && buildMode == true) SwitchBlock();
        WriteDebug();
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == baseSceneName && buildMode == true)
        {
            transform.GetComponent<PlayerScript>().enabled = false;
            transform.Find("InventoryUI").gameObject.SetActive(false);

            CameraMove();
            PlaceBlock();
            RemoveBlock();
        }
        else if (SceneManager.GetActiveScene().name == caveSceneName && buildMode == true)
        {
            Debug.Log("��� �������, ���� ���������� ��*�� �� �����!");
        }
        else
        {
            transform.GetComponent<PlayerScript>().enabled = true;
            transform.Find("InventoryUI").gameObject.SetActive(false);
        }
    }

    private void WriteDebug()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("BaseCore").gameObject.GetComponent<BaseCore>().WriteDebug();
            //Debug.Log(baseSceneName);
            //Debug.Log(buildMode);
            //Debug.Log(blockPrefabs);
            //Debug.Log(curentBlock);
        }
    }

    private void CameraMove()
    {
        cameraMoveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            cameraMoveVector.y = cameraSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            cameraMoveVector.y = -cameraSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            cameraMoveVector.x = cameraSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraMoveVector.x = -cameraSpeed;
        }

        cameraMain.transform.Translate(cameraMoveVector * Time.fixedDeltaTime);
    }

    private void PlaceBlock()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 click = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(click, -Vector3.forward);

            if (hit.transform == null || hit.transform.name != "HitBox")
            {
                Vector3 position;

                position.x = (float)Math.Floor(click.x) + 0.5f;

                position.y = (float)Math.Floor(click.y) + 0.5f;

                position.z = 0;

                GameObject.Find("BaseCore").gameObject.GetComponent<BaseCore>().AddBlock(curentBlock.name, position);

                Instantiate(curentBlock, position, Quaternion.identity).name = curentBlock.name;
            }
        }
    }

    private void RemoveBlock()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 click = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(click, -Vector3.forward);

            if (hit.transform != null && hit.transform.name == "HitBox")
            {
                GameObject.Find("BaseCore").gameObject.GetComponent<BaseCore>().RemoveBlock(hit.transform.parent.gameObject.name, hit.transform.parent.gameObject.transform.position);

                Destroy(hit.transform.parent.gameObject);
            }
        }
    }

    private void SwitchBlock()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curentBlock = blockPrefabs[0];
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curentBlock = blockPrefabs[1];
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            curentBlock = blockPrefabs[2];
        }
    }
}
