using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private float _animationSpeed = 2f;
    private Coroutine _moveCoroutine;

    [Header("BigSonar Panels")]
    [SerializeField] private Button _bigSonarButton;
    [SerializeField] private RectTransform _bigSonarPanelRectTransform;
    [SerializeField] private float _showSonarPanelX = 0f;
    [SerializeField] private float _hideSonarPanelX = 325f;
    [SerializeField] private float _healthPanelX = 475f;
    private bool _isBigSonarActive = false;

    [Header("Volume Panels")]
    [SerializeField] private Button _volumeButton;
    [SerializeField] private RectTransform _volumePanel;
    [SerializeField] private float _showVolumeX = 0f;
    [SerializeField] private float _hideVolumeX = -550f;
    private bool _isVolumeActive = false;

    [Header("SmallSonar Button")]
    [SerializeField] private Button _smallSonarButton;
    [SerializeField] private Color _smallSonarActiveColor = Color.green;
    [SerializeField] private Color _smallSonarInactiveColor = Color.white;

    private void Start()
    {
        _bigSonarButton.onClick.RemoveAllListeners();
        _smallSonarButton.onClick.RemoveAllListeners();
        _volumeButton.onClick.RemoveAllListeners();

        _bigSonarButton.onClick.AddListener(() => OnButtonSonarClicked(_showSonarPanelX, _hideSonarPanelX));
        _volumeButton.onClick.AddListener(() => OnButtonVolumeClicked(_showVolumeX, _hideVolumeX));

        _smallSonarButton.onClick.AddListener(OnSmallSonarButtonClicked);

        if (SonarController.Instance != null)
        {
            SonarController.Instance.OnSonarStateChanged += UpdateSmallSonarVisual;

            UpdateSmallSonarVisual(SonarController.Instance.IsOn);
        }

        SetPanelsPosition();
    }

    private void OnDestroy()
    {
        if (SonarController.Instance != null)
        {
            SonarController.Instance.OnSonarStateChanged -= UpdateSmallSonarVisual;
        }
    }

    private void OnButtonSonarClicked(float showX, float hideX)
    {
        _isBigSonarActive = !_isBigSonarActive;

        float targetX = _isBigSonarActive ? showX : hideX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_bigSonarPanelRectTransform, targetX));
    }

    private void OnButtonVolumeClicked(float showX, float hideX)
    {
        _isVolumeActive = !_isVolumeActive;

        float targetX = _isVolumeActive ? showX : hideX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_volumePanel, targetX));
    }

    private void OnSmallSonarButtonClicked()
    {
        if (SonarController.Instance != null)
        {
            SonarController.Instance.ToggleSonar();
        }
    }

    private void UpdateSmallSonarVisual(bool isActive)
    {
        _smallSonarButton.GetComponent<Image>().color =
            isActive ? _smallSonarActiveColor : _smallSonarInactiveColor;
    }
    public IEnumerator MovePanel(RectTransform panel, float targetX, float animationSpeed = 2f)
    {
        Vector2 startPos = panel.anchoredPosition;
        Vector2 targetPos = new Vector2(targetX, startPos.y);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * (animationSpeed != 0 ? animationSpeed : _animationSpeed);
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        panel.anchoredPosition = targetPos;
    }

    private void SetPanelsPosition()
    {
        SetPanelPosition(_bigSonarPanelRectTransform, _healthPanelX);
        SetPanelPosition(_volumePanel, _hideVolumeX);
    }

    private void SetPanelPosition(RectTransform panel, float x)
    {
        Vector2 pos = panel.anchoredPosition;
        pos.x = x;
        panel.anchoredPosition = pos;
    }

    public void ShowBigSonarPanel(bool show)
    {
        _isBigSonarActive = show;

        float targetX = _isBigSonarActive ? _showSonarPanelX : _hideSonarPanelX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_bigSonarPanelRectTransform, targetX));
    }
}