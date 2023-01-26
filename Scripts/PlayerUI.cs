using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI AmountWoodText { get; private set; }
    public TextMeshProUGUI AmountStoneText { get; private set; }

    void Awake()
    {
        Canvas canvas = transform
            .Find("InventoryUI")
            .Find("GameResources")
            .GetComponent<Canvas>();
        Transform panel = canvas.transform.GetChild(0);
        AmountWoodText = panel
            .Find("Wood")
            .GetComponent<TextMeshProUGUI>();
        AmountStoneText = panel
            .Find("Stone")
            .GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(Dictionary<GameResources, float> resources)
    {
        AmountWoodText.text = $"WOOD: {resources[GameResources.Wood]}";
        AmountStoneText.text = $"STONE: {resources[GameResources.Stone]}";
    }

    public void OnResourceAmountChanged(GameResources resources, float amount)
    {
        switch (resources)
        {
            case GameResources.Wood:
                AmountWoodText.text = $"WOOD: {amount}";
                break;
            case GameResources.Stone:
                break;
            default:
                break;
        }
    }
}
