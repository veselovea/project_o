using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        if (transform.Find("ButtonText").gameObject.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            //закрыть
            
        }
        else if(!transform.Find("ButtonText").gameObject.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            //открыть
            
        }
    }

/*    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            transform.Find("ButtonText").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("ButtonText").gameObject.SetActive(false);
        }
    }*/
}
