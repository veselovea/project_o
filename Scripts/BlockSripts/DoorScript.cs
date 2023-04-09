using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public ResourceBlock resourceBlock = new ResourceBlock();

    private void Start()
    {
        resourceBlock.Durability = 50;
        resourceBlock.Type = ResourcesFromBlocks.Door;
    }

    private void Update()
    {
        if (transform.Find("ButtonText").gameObject.activeSelf)
        {
            transform.Find("ButtonText").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("ButtonText").gameObject.SetActive(true);
        }

    }
}
