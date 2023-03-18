using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IDConverter : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public List<GameObject> Gameobjects = new();

    public GameObject IDToGameobjectConverter(string id)
    {
        return Gameobjects.Find(go => go.name == id);
    }

    public string GameobjectToIDConverter(GameObject gameObject)
    {
        try
        {
            return Gameobjects.Find(go => go == gameObject).name;
        }
        catch
        {
            return null;
        }
    }
}
