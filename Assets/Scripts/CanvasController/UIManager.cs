using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("BigSonar Panels")]
    [SerializeField] private Button _bigSonarButton;
    [SerializeField] private RectTransform _bigSonarPanel;
    [SerializeField] private float _showX = 0f;
    [SerializeField] private float _hideX = 325f;
    [SerializeField] private float _animationSpeed = 5f;
    private bool _isBigSonarActive = false;
    private Coroutine _moveCoroutine;

    [Header("SmallSonar Panels")]
    [SerializeField] private Button _smallSonarButton;
    private bool _isSmallSonarActive = false;
    [SerializeField] private Color _smallSonarActiveColor = Color.green;
    [SerializeField] private Color _smallSonarInactiveColor = Color.white;

    void Start()
    {
        _bigSonarButton.onClick.RemoveAllListeners();
        _smallSonarButton.onClick.RemoveAllListeners();
        _bigSonarButton.onClick.AddListener(OnBigSonarButtonClicked);
        _smallSonarButton.onClick.AddListener(OnSmallSonarButtonClicked);
        SetPanelPosition(_hideX);
    }

    private void OnBigSonarButtonClicked()
    {
        _isBigSonarActive = !_isBigSonarActive;

        float targetX = _isBigSonarActive ? _showX : _hideX;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MovePanel(targetX));
    }

    private IEnumerator MovePanel(float targetX)
    {
        Vector2 startPos = _bigSonarPanel.anchoredPosition;
        Vector2 targetPos = new Vector2(targetX, startPos.y);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * _animationSpeed;
            _bigSonarPanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        _bigSonarPanel.anchoredPosition = targetPos;
    }

    private void SetPanelPosition(float x)
    {
        Vector2 pos = _bigSonarPanel.anchoredPosition;
        pos.x = x;
        _bigSonarPanel.anchoredPosition = pos;
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