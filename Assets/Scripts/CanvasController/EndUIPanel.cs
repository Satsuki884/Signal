using UnityEngine;
using TMPro;

public class EndUIPanel : MonoBehaviour
{
    public static EndUIPanel Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TMP_Text _endText;

    void Start()
    {
        _endPanel.SetActive(false);
    }
    
    public void GameOver(string message)
    {
        _endPanel.SetActive(true);
        _endText.text = message;
        Time.timeScale = 0f;
    }
}