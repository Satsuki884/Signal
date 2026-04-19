using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocatorController : MonoBehaviour
{    
    [Header("Locator Panels")]
    [SerializeField] private float _animationSpeed = 5f;
    private Coroutine _moveCoroutine;
    [SerializeField] private RectTransform _locatorPanel;
    [SerializeField] private float _showLocatorPanelX = 0f;
    [SerializeField] private float _hideLocatorPanelX = 325f;
    [SerializeField] private PlayerSO _playerSO;

    private IEnumerator MovePanel(RectTransform panel, float targetX)
    {
        Vector2 startPos = panel.anchoredPosition;
        Vector2 targetPos = new Vector2(targetX, startPos.y);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * _animationSpeed;
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        panel.anchoredPosition = targetPos;
    }
    public void SetLocatorPanelPosition(bool show)
    {
        Vector2 pos = _locatorPanel.anchoredPosition;
        pos.x = show ? _showLocatorPanelX : _hideLocatorPanelX;
        _locatorPanel.anchoredPosition = pos;

        float targetX = show ? _showLocatorPanelX : _hideLocatorPanelX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_locatorPanel, targetX));
    }
}