using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectTemporary
{
    List<BaseBlockTemporary> BaseBlocks { get; set; }
}

public class BaseBlockTemporary
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public Vector3 Position { get; set; }
}
