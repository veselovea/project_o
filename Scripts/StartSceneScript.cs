using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneScript : MonoBehaviour
{
    public GameObject _playerNameObject;
    public static string PlayerName { get; private set; }
    
    private TextMeshProUGUI _textMeshPro;

    void Awake()
    {
        _textMeshPro = _playerNameObject
            .transform
            .Find("Text Area")
            .Find("Text")
            .GetComponent<TextMeshProUGUI>();
    }

    public void PlayerNameChanged()
    {
        PlayerName = _textMeshPro.GetParsedText().ToLower();
    }

    public async void Play()
    {
        if (PlayerName is null || PlayerName is "")
            return;
        PlayerName = PlayerName.Remove(PlayerName.Length - 1, 1);
        SceneManager.LoadSceneAsync("DNA_Scene", LoadSceneMode.Single);
    }

    public void FastGame()
    {
        if (PlayerName is null || PlayerName is "")
            return;
        PlayerName = PlayerName.Remove(PlayerName.Length - 1, 1);
        SceneManager.LoadSceneAsync("MultiplayerArenaScene", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Debug.Log("Exit");
    }
}
