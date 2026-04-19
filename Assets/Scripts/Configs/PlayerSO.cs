using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Configs/Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private float _walkSpeed = 5f;
    public float WalkSpeed => _walkSpeed;
    [SerializeField] private int _health = 3;
    public int Health => _health;
    [SerializeField] private bool _hasLocator = false;
    public bool Locator => _hasLocator;
    [SerializeField] private LocatorController _locatorPrefab;
    public LocatorController LocatorPrefab => _locatorPrefab;

    public void ObtainLocator()
    {
        _hasLocator = true;
        _locatorPrefab.SetLocatorPanelPosition(true);
    }

    public void ResetPlayerData()
    {
        _hasLocator = false;
        _locatorPrefab.SetLocatorPanelPosition(false);
    }
}