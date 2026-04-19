using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 3f;
    private Coroutine _moveCoroutine;

    [SerializeField] private Button _playButton;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private GameObject _menuPanel;

    [SerializeField] private float _showY = 0f;
    [SerializeField] private float _hideY = 2000f;

    private void Start()
    {
        _menuPanel.SetActive(true);

        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(OnPlay);

        SetY(_panel, _showY);
    }

    private void OnPlay()
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(HidePanel());
    }

    private IEnumerator HidePanel()
    {
        yield return MovePanel(_panel, _hideY);

        _menuPanel.SetActive(false);
    }

    private IEnumerator MovePanel(RectTransform panel, float targetY)
    {
        Vector2 start = panel.anchoredPosition;
        Vector2 target = new Vector2(start.x, targetY);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * _animationSpeed;
            panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        panel.anchoredPosition = target;
    }

    private void SetY(RectTransform panel, float y)
    {
        Vector2 pos = panel.anchoredPosition;
        pos.y = y;
        panel.anchoredPosition = pos;
    }
}