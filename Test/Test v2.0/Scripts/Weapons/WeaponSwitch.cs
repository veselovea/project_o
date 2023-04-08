using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] Weapons;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.V))
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i].activeInHierarchy == true)
                {
                    Weapons[i].SetActive(false);

                    if (i != 0)
                    {
                        Weapons[i - 1].SetActive(true);
                    }
                    else
                    {
                        Weapons[Weapons.Length - 1].SetActive(true);
                    }
                    break;
                }
            }
        }   
    }
}
