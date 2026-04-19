using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public static HealthController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] private List<HealthPoint> _healthPoints;
    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < _healthPoints.Count; i++)
        {
            _healthPoints[i].SetHealthPointActive(i < currentHealth);
        }
    }

    void Start()
    {
        UpdateHealth(GameManager.Instance.PlayerData.CurrentHealth);
    }
}