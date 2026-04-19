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
    [SerializeField] private float _animationSpeed = 5f;
    private Coroutine _moveCoroutine;

    [Header("BigSonar Panels")]
    [SerializeField] private Button _bigSonarButton;
    [SerializeField] private BigSonarController _bigSonarController;
    private bool _isBigSonarActive = false;


    [Header("Volume Panels")]
    [SerializeField] private Button _volumeButton;
    [SerializeField] private RectTransform _volumePanel;
    [SerializeField] private float _showVolumeX = 0f;
    [SerializeField] private float _hideVolumeX = -550f;
    private bool _isVolumeActive = false;

    [Header("SmallSonar Panels")]
    [SerializeField] private Button _smallSonarButton;
    private bool _isSmallSonarActive = false;
    [SerializeField] private Color _smallSonarActiveColor = Color.green;
    [SerializeField] private Color _smallSonarInactiveColor = Color.white;

    void Start()
    {
        _bigSonarButton.onClick.RemoveAllListeners();
        _smallSonarButton.onClick.RemoveAllListeners();
        _bigSonarButton.onClick.AddListener(() => OnButtonSonarClicked(_bigSonarController.ShowSonarPanelX, _bigSonarController.HideSonarPanelX));
        _volumeButton.onClick.AddListener(() => OnButtonVolumeClicked(_showVolumeX, _hideVolumeX));
        _smallSonarButton.onClick.AddListener(OnSmallSonarButtonClicked);
        SetPanelsPosition();
    }

    private void OnButtonSonarClicked(float showX, float hideX)
    {
        Debug.Log($"Button clicked. Current state: {_isBigSonarActive}");
        _isBigSonarActive = !_isBigSonarActive;

        float targetX = _isBigSonarActive ? showX : hideX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_bigSonarController.SonarPanelTransform, targetX));
    }

    private void OnButtonVolumeClicked(float showX, float hideX)
    {
        Debug.Log($"Button clicked. Current state: {_isVolumeActive}");
        _isVolumeActive = !_isVolumeActive;

        float targetX = _isVolumeActive ? showX : hideX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(_volumePanel, targetX));
    }

    public IEnumerator MovePanel(RectTransform panel, float targetX)
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

    private void SetPanelsPosition()
    {
        SetPanelPosition(_bigSonarController.SonarPanelTransform, _bigSonarController.HideSonarPanelX);
        SetPanelPosition(_volumePanel, _hideVolumeX);
    }

    private void SetPanelPosition(RectTransform panel, float x)
    {
        Vector2 pos = panel.anchoredPosition;
        pos.x = x;
        panel.anchoredPosition = pos;
    }

    private void OnSmallSonarButtonClicked()
    {
        if (_isSmallSonarActive)
        {
            _isSmallSonarActive = false;
            _smallSonarButton.GetComponent<Image>().color = _smallSonarInactiveColor;

        }
        else
        {
            _isSmallSonarActive = true;
            _smallSonarButton.GetComponent<Image>().color = _smallSonarActiveColor;
        }
    }
}