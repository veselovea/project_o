using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObject
{
    public WeightedObject(GameObject obj, int weight)
    {
        Object = obj;
        Weight = weight;
    }
    public GameObject Object { get; set; }
    public int Weight;
}
