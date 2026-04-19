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
    [SerializeField] private bool _hasBigSonar = false;
    public bool BigSonar => _hasBigSonar;

    public void ObtainLocator()
    {
        _hasLocator = true;
        LocatorController.Instance.SetLocatorPanelPosition(true);
    }

    public void ObtainBigSonar()
    {
        _hasBigSonar = true;
        BigSonarController.Instance.SetUsingSonarPanel(true);
    }

    public void ResetPlayerData()
    {
        _hasLocator = false;
        LocatorController.Instance.SetLocatorPanelPosition(false);
        _hasBigSonar = false;
        BigSonarController.Instance.SetUsingSonarPanel(false);

    }
}