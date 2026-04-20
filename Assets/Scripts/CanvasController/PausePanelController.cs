using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    public static PausePanelController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    [Header("Settings")]
    [SerializeField] private float _animationSpeed = 3f;

    [Header("Buttons")]
    [SerializeField] private Button _openPauseButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private Button _resumeButton;

    [Header("Panels")]
    [SerializeField] private RectTransform _leftPanel;
    [SerializeField] private RectTransform _rightPanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("Positions (X)")]
    [SerializeField] private float _leftShowX = 0f;
    [SerializeField] private float _leftHideX = -2000f;

    [SerializeField] private float _rightShowX = 0f;
    [SerializeField] private float _rightHideX = 2000f;

    [Header("Task")]
    [SerializeField] private TMP_Text _taskText;
    public void ShowTask(TaskData task)
    {
        if (task == null) return;

        _taskText.text = task.Description;
    }
    private void Start()
    {
        _openPauseButton.onClick.RemoveAllListeners();
        _openPauseButton.onClick.AddListener(OpenPause);

        _resumeButton.onClick.RemoveAllListeners();
        _resumeButton.onClick.AddListener(Resume);

        _toMenuButton.onClick.RemoveAllListeners();
        _toMenuButton.onClick.AddListener(ToMenu);

        // стартовые позиции
        SetPanelX(_leftPanel, _leftHideX);
        SetPanelX(_rightPanel, _rightHideX);

        _pausePanel.SetActive(false);
    }

    // 🔥 Открыть паузу
    private void OpenPause()
    {
        _pausePanel.SetActive(true);

        StartCoroutine(MoveX(_leftPanel, _leftShowX));
        StartCoroutine(MoveX(_rightPanel, _rightShowX));

        StartCoroutine(FreezeAfterAnimation());
    }

    // 🔥 Resume
    private void Resume()
    {
        Time.timeScale = 1f;

        StartCoroutine(MoveX(_leftPanel, _leftHideX));   // ← влево
        StartCoroutine(MoveX(_rightPanel, _rightHideX)); // ← вправо

        StartCoroutine(HideAfter());
    }

    private void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🔥 Движение по X
    private IEnumerator MoveX(RectTransform panel, float targetX)
    {
        Vector2 start = panel.anchoredPosition;
        Vector2 target = new Vector2(targetX, start.y);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * _animationSpeed;
            panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        panel.anchoredPosition = target;
    }

    private IEnumerator FreezeAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 0f;
    }

    private IEnumerator HideAfter()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        _pausePanel.SetActive(false);
    }

    private void SetPanelX(RectTransform panel, float x)
    {
        Vector2 pos = panel.anchoredPosition;
        pos.x = x;
        panel.anchoredPosition = pos;
    }
}