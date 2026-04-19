using UnityEngine;

public class LocatorController : MonoBehaviour
{

    public static LocatorController Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    [Header("Locator Panels")]
    private Coroutine _moveCoroutine;
    [SerializeField] private RectTransform _locatorPanel;
    [SerializeField] private float _showLocatorPanelX = 0f;
    [SerializeField] private float _hideLocatorPanelX = -425f;

    void Start()
    {
        SetLocatorPanelPosition(false);
    }
    public void SetLocatorPanelPosition(bool show)
    {
        Vector2 pos = _locatorPanel.anchoredPosition;
        pos.x = show ? _showLocatorPanelX : _hideLocatorPanelX;
        _locatorPanel.anchoredPosition = pos;

        float targetX = show ? _showLocatorPanelX : _hideLocatorPanelX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(UIManager.Instance.MovePanel(_locatorPanel, targetX));
    }
}