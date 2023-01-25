using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI AmoutTreeText { get; private set; }

    void Awake()
    {
        GameObject canvas = GameObject.Find("PlayerUI");
        Transform text = canvas.transform.GetChild(0);
        AmoutTreeText = text.GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(Dictionary<GameResources, float> resources)
    {
        AmoutTreeText.text = $"WOOD: {resources[GameResources.Wood]}";
    }

    public void OnResourceAmountChanged(GameResources resources, float amount)
    {
        switch (resources)
        {
            case GameResources.Wood:
                AmoutTreeText.text = $"WOOD: {amount}";
                break;
            case GameResources.Stone:
                break;
            default:
                break;
        }
    }
}
