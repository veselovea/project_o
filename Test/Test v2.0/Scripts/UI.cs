using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Player Carl;
    public Text Health;

    private void Awake()
    {
        Carl = GetComponent<Player>();
    }

    private void Update()
    {
        Health.text = (Carl.health).ToString();
    }
}
