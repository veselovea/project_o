using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInitializer : MonoBehaviour
{
    private GameObject[] baseSpots;
    private List<GameObject> blockList = new();
    // Start is called before the first frame update
    void Awake()
    {
        blockList.AddRange(Resources.LoadAll<GameObject>("Blocks"));
        baseSpots = GameObject.FindGameObjectsWithTag("Respawn");
        //StartCoroutine(DelayedBasesRotation());
    }

    private IEnumerator DelayedBasesRotation(GameObject baseSpot)
    {
        yield return new WaitForSeconds(5f);

        if (baseSpot.transform.position.x == 115)
        {
            baseSpot.transform.eulerAngles = new Vector3(180, 0, 0);
        }

        if (baseSpot.transform.position.x == -115)
        {
            baseSpot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (baseSpot.transform.position.y == 115)
        {
            baseSpot.transform.eulerAngles = new Vector3(90, 0, 0);
        }

        if (baseSpot.transform.position.y == -115)
        {
            baseSpot.transform.eulerAngles = new Vector3(-90, 0, 0);
        }
    }

    public void SetupBase(PlayerBaseObject playerBase)
    {
        if(baseSpots.Length > 0)
        {
            try
            {
                GameObject currentBaseSpot = baseSpots[baseSpots.Length - 1];
                Array.Resize(ref baseSpots, baseSpots.Length - 1);

                foreach (Eblock block in playerBase.PlayerBaseBlocks)
                {
                    Instantiate(
                        blockList.Find(bl => bl.gameObject.name == block.BlockName),
                        currentBaseSpot.transform.TransformPoint(block.BlockPosition),
                        Quaternion.identity,
                        currentBaseSpot.transform);
                }

                StartCoroutine(DelayedBasesRotation(currentBaseSpot));
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
