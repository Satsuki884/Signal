using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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
    [SerializeField] private float _animationSpeed = 3f;
    private Coroutine _moveCoroutine;
    [SerializeField] private RectTransform _endPanelRectTransform;
    [SerializeField] private float _showEndPanelY = 0f;
    [SerializeField] private float _hideEndPanelY = 2000f;

    void Start()
    {
        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(RestartGame);
        SetPanelPosition(_endPanelRectTransform, _hideEndPanelY);
        _endPanel.SetActive(false);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        InputManager.Instance.actions.Disable();
        InputManager.Instance.actions.Enable();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(string message)
    {
        
        _endPanel.SetActive(true);
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MovePanel(_endPanelRectTransform, _hideEndPanelY));
        Time.timeScale = 0f;
        _endText.text = message;

    }

    public IEnumerator MovePanel(RectTransform panel, float targetY, float animationSpeed = 2f)
    {
        Vector2 startPos = panel.anchoredPosition;
        Vector2 targetPos = new Vector2(startPos.x, targetY);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * (animationSpeed != 0 ? animationSpeed : _animationSpeed);
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        panel.anchoredPosition = targetPos;
    }
    private void SetPanelPosition(RectTransform panel, float x)
    {
        Vector2 pos = panel.anchoredPosition;
        pos.x = x;
        panel.anchoredPosition = pos;
    }
}