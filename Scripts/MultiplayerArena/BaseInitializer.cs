using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInitializer : MonoBehaviour
{
    public List<GameObject> baseSpots;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedBasesRotation());
    }

    IEnumerator DelayedBasesRotation()
    {
        yield return new WaitForSeconds(5f);

        foreach (GameObject baseSpot in baseSpots)
        {
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
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
