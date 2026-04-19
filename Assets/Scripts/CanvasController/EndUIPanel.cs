using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Button _restartButton;

    void Start()
    {
        _endPanel.SetActive(false);
        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _endPanel.SetActive(false);
    }

    public void GameOver(string message)
    {
        _endPanel.SetActive(true);
        _endText.text = message;
        Time.timeScale = 0f;
    }
}