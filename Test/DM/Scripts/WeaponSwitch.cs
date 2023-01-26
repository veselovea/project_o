using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject sword;
    public GameObject axe;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.V))
        {
            if (sword.activeInHierarchy == true)
            {
                sword.SetActive(false);
                axe.SetActive(true);
            }
            else if (axe.activeInHierarchy == true)
            {
                axe.SetActive(false);
                sword.SetActive(true);
            }
        }   
    }
}
