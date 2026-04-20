using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Configs/Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private float _walkSpeed = 5f;
    public float WalkSpeed => _walkSpeed;
    [SerializeField] private int _MaxHealth = 3;
    public int MaxHealth { get => _MaxHealth; set => _MaxHealth = value; }
    [SerializeField] private int _currentHealth = 3;
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    [SerializeField] private bool _hasLocator = false;
    public bool HasLocator { get => _hasLocator; set => _hasLocator = value; }
    [SerializeField] private bool _hasBigSonar = false;
    public bool HasBigSonar { get => _hasBigSonar; set => _hasBigSonar = value; }
    [SerializeField] private bool _hasFirstBattery = false;
    public bool HasFirstBattery { get => _hasFirstBattery; set => _hasFirstBattery = value; }
    [SerializeField] private bool _hasSecondBattery = false;
    public bool HasSecondBattery { get => _hasSecondBattery; set => _hasSecondBattery = value; }
}