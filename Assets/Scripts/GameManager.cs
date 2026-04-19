using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        ResetPlayerData();
    }
    [SerializeField] PlayerSO _playerSO;

    public PlayerSO PlayerData => _playerSO;

    public void TakeDamage(int damage)
    {
        _playerSO.CurrentHealth -= damage;
        HealthController.Instance.UpdateHealth(_playerSO.CurrentHealth);
        if (_playerSO.CurrentHealth == 0)
        {
            EndUIPanel.Instance.GameOver("Health has reached zero!");
        }
        
    }

    public void ObtainLocator()
    {
        _playerSO.HasLocator = true;
        LocatorController.Instance.SetLocatorPanelPosition(true);
    }

    public void ObtainBigSonar()
    {
        _playerSO.HasBigSonar = true;
        // BigSonarController.Instance.SetBigSonarPanelPosition();
        UIManager.Instance.ShowBigSonarPanel(_playerSO.HasBigSonar);
    }

    public void ResetPlayerData()
    {
        _playerSO.HasLocator = false;
        LocatorController.Instance.SetLocatorPanelPosition(false);
        _playerSO.HasBigSonar = false;
        UIManager.Instance.ShowBigSonarPanel(_playerSO.HasBigSonar);
        _playerSO.CurrentHealth = _playerSO.MaxHealth;
        HealthController.Instance.UpdateHealth(_playerSO.MaxHealth);

    }
}