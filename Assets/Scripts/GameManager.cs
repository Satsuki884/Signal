using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float hitSoundCooldown = 0.15f;
    private float lastHitSoundTime = -1f;

    [SerializeField] private PlayerSO _playerSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetPlayerData();
    }

    public PlayerSO PlayerData => _playerSO;

    public void TakeDamage(int damage)
    {
        if (_playerSO == null) return;

        _playerSO.CurrentHealth -= damage;
        if (_playerSO.CurrentHealth < 0) _playerSO.CurrentHealth = 0;

        if (HealthController.Instance != null)
            HealthController.Instance.UpdateHealth(_playerSO.CurrentHealth);

        if (_playerSO.CurrentHealth == 0)
        {
            if (AudioManager.Instanse != null && AudioManager.Instanse.death != null)
            {
                AudioManager.Instanse.PlaySFX(AudioManager.Instanse.death);
            }

            if (EndUIPanel.Instance != null)
                EndUIPanel.Instance.GameOver("Health has reached zero!");

            return;
        }

        if (AudioManager.Instanse != null && AudioManager.Instanse.hit != null && AudioManager.Instanse.hit.Length > 0)
        {
            if (Time.time - lastHitSoundTime >= hitSoundCooldown)
            {
                AudioClip clip = AudioManager.Instanse.hit[Random.Range(0, AudioManager.Instanse.hit.Length)];
                AudioManager.Instanse.PlaySFX(clip);
                lastHitSoundTime = Time.time;
            }
        }
    }

    public void ObtainLocator()
    {
        if (_playerSO == null) return;

        _playerSO.HasLocator = true;
        if (LocatorController.Instance != null)
            LocatorController.Instance.SetLocatorPanelPosition(true);
        TaskManager.Instance?.CompleteTaskById("TaskData_Collect_locator");
    }

    public void ObtainBigSonar()
    {
        if (_playerSO == null) return;

        _playerSO.HasBigSonar = true;
        if (UIManager.Instance != null)
            UIManager.Instance.ShowBigSonarPanel(_playerSO.HasBigSonar);
        TaskManager.Instance?.CompleteTaskById("TaskData_Collect_big_sonar");
    }

    public void ObtainFirstBattery()
    {
        if (_playerSO == null) return;

        _playerSO.HasFirstBattery = true;
        TaskManager.Instance?.CompleteTaskById("TaskData_Collect_FirstBat");
    }

    public void ObtainSecondBattery()
    {
        if (_playerSO == null) return;

        _playerSO.HasSecondBattery = true;
        TaskManager.Instance?.CompleteTaskById("TaskData_Investigate_SecondBat");
    }

    public void ResetPlayerData()
    {
        if (_playerSO == null) return;

        _playerSO.HasLocator = false;
        if (LocatorController.Instance != null)
            LocatorController.Instance.SetLocatorPanelPosition(false);

        _playerSO.HasBigSonar = false;
        if (UIManager.Instance != null)
            UIManager.Instance.ShowBigSonarPanel(_playerSO.HasBigSonar);

        _playerSO.CurrentHealth = _playerSO.MaxHealth;
        if (HealthController.Instance != null)
            HealthController.Instance.UpdateHealth(_playerSO.MaxHealth);

        _playerSO.HasFirstBattery = false;

        _playerSO.HasSecondBattery = false;
    }
}
