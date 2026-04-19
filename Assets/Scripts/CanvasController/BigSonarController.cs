using UnityEngine;

public class BigSonarController : MonoBehaviour
{
    public static BigSonarController Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    [Header("Big Sonar Panels")]
    public RectTransform SonarPanelTransform => _sonarPanel.GetComponent<RectTransform>();
    [SerializeField] private GameObject _sonarPanel;
    [SerializeField] private RectTransform _sonarPanelRectTransform;
    public RectTransform SonarPanel => _sonarPanelRectTransform;
    [SerializeField] private float _showSonarPanelX = 0f;
    [SerializeField] private float _healthPanelX = 475f;
    public float ShowSonarPanelX => _showSonarPanelX;
    [SerializeField] private float _hideSonarPanelX = 325f;
    public float HideSonarPanelX => _hideSonarPanelX;

    void Start()
    {
        _sonarPanelRectTransform = _sonarPanel.GetComponent<RectTransform>();
        Vector2 pos = _sonarPanelRectTransform.anchoredPosition;
        pos.x = _healthPanelX;
        _sonarPanelRectTransform.anchoredPosition = pos;
    }

    public void SetBigSonarPanelPosition()
    {
        float targetX = GameManager.Instance.PlayerData.HasBigSonar ? _hideSonarPanelX : _healthPanelX;

        StartCoroutine(UIManager.Instance.MovePanel(
            _sonarPanelRectTransform,
            targetX
        ));
    }
}