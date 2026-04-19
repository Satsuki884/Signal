using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 3f;
    private Coroutine _moveCoroutine;
    [SerializeField] private Button _openPauseButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private RectTransform _leftPausePanelRectTransform;
    [SerializeField] private RectTransform _rightPausePanelRectTransform;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private float _showLeftPausePanelY = 0f;
    [SerializeField] private float _showRightPausePanelY = 0f;
    [SerializeField] private float _hideLeftPausePanelY = 2000f;
    [SerializeField] private float _hideRightPausePanelY = 2000f;
    private void Start()
    {
        
        _openPauseButton.onClick.RemoveAllListeners();
        _openPauseButton.onClick.AddListener(() => OnOpenPauseButtonClicked());
        _toMenuButton.onClick.RemoveAllListeners();
        _toMenuButton.onClick.AddListener(() => OnToMenuButtonClicked());
        _resumeButton.onClick.RemoveAllListeners();
        _resumeButton.onClick.AddListener(() => OnResumeButtonClicked());
        SetPanelPosition(_leftPausePanelRectTransform, _hideLeftPausePanelY);
        SetPanelPosition(_rightPausePanelRectTransform, _hideRightPausePanelY);
        _pausePanel.SetActive(false);
    }

    private void OnOpenPauseButtonClicked()
    {
        
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        _pausePanel.SetActive(true);

        _moveCoroutine = StartCoroutine(MovePanel(_leftPausePanelRectTransform, _showLeftPausePanelY));
        _moveCoroutine = StartCoroutine(MovePanel(_rightPausePanelRectTransform, _showRightPausePanelY));
        Time.timeScale = 0f;
    }


    private void OnResumeButtonClicked()
    {
        Time.timeScale = 1f;
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_leftPausePanelRectTransform, _hideLeftPausePanelY));
        _moveCoroutine = StartCoroutine(MovePanel(_rightPausePanelRectTransform, _hideRightPausePanelY));
        _pausePanel.SetActive(false);
    }

    private void OnToMenuButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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