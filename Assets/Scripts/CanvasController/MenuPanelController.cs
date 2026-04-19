using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 3f;
    private Coroutine _moveCoroutine;
    [SerializeField] private Button _PlayButton;
    [SerializeField] private RectTransform _menuPanelRectTransform;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private float _showMenuPanelY = 0f;
    [SerializeField] private float _hideMenuPanelY = 2000f;
    private bool _isMenuActive = true;
    private void Start()
    {
        _menuPanel.SetActive(true);
        _PlayButton.onClick.RemoveAllListeners();
        _PlayButton.onClick.AddListener(() => OnPlayButtonClicked());
        SetPanelPosition(_menuPanelRectTransform, _showMenuPanelY);
    }

    private void OnPlayButtonClicked()
    {
        Time.timeScale = 1f;
        _isMenuActive = false;
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_menuPanelRectTransform, _hideMenuPanelY));
        _menuPanel.SetActive(false);
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