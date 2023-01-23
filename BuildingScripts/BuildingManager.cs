using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Camera cameraMain;
    public GameObject blockPrefab;
    // Start is called before the first frame update
    void Start()
    {
        cameraMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 click = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position;
            position.x = (float)((float)Math.Round(click.x / 1) * 1);
            position.y = (float)((float)Math.Round(click.y / 1) * 1);
            position.z = 0;

            Instantiate(blockPrefab, position, Quaternion.identity);
        }
    }
}
