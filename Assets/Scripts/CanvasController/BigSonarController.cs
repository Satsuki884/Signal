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
    [SerializeField] private float _showSonarPanelX = 0f;
    public float ShowSonarPanelX => _showSonarPanelX;
    [SerializeField] private float _hideSonarPanelX = 325f;
    public float HideSonarPanelX => _hideSonarPanelX;

    void Start()
    {
        SetUsingSonarPanel(false);
    }

    public void SetUsingSonarPanel(bool used)
    {
        _sonarPanel.SetActive(used);
    }
}