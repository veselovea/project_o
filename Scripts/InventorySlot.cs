using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : ScriptableObject
{
    public InventorySlot()
    {

    }

    public InventorySlot(int index, string name, Image img, GameObject gameObject)
    {
        Index = index;
        Name = name;
        Image = img;
        GameObject = gameObject;
    }

    public int Index { get; }
    public string Name { get; }
    public Image Image { get; }
    public GameObject GameObject { get; }
}
