using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUIPanel : MonoBehaviour
{
    public static EndUIPanel Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _restartButton;

    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _showY = 0f;
    [SerializeField] private float _hideY = 2000f;
    [SerializeField] private float _animationSpeed = 3f;

    private Coroutine _moveCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(RestartGame);

        SetY(_rect, _hideY);
        _panel.SetActive(false);
    }

    public void GameOver(string message)
    {
        _panel.SetActive(true);

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _text.text = message;

        _moveCoroutine = StartCoroutine(ShowPanel());
    }

    private IEnumerator ShowPanel()
    {
        yield return MovePanel(_rect, _showY);

        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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