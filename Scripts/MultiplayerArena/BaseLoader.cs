using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLoader : MonoBehaviour
{
    public List<BaseBlockTemporary> Base { get; set; } = new();

    // Start is called before the first frame update
    void Start()
    {
        foreach (BaseBlockTemporary block in Base)
        {
            Instantiate(block.Original, block.Position, Quaternion.identity, this.transform);
        }
    }
}
