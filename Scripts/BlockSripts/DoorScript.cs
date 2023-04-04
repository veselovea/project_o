using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    private void Start()
    {
        transform.FindChild("ButtonText").gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.FindChild("ButtonText").gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
