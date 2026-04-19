using UnityEngine;
using Unity.Cinemachine;
using TMPro;
using UnityEngine.UI;

public class EnergyStateSystem : MonoBehaviour
{
    public static EnergyStateSystem Instance;

    [SerializeField] private Slider _energyValueSlider;

    [Range(0f, 100f)]
    public float EnergyValue = 100f;

    [SerializeField] private float _maxValue = 100f;

    [Header("Mental Increase")]
    [SerializeField] private float _passiveIncreaseSpeed = 0.25f;
    [SerializeField] private float _stalkerBonusIncrease = 0.5f;

    [Header("UI")]
    [SerializeField] private EndUIPanel _endUIPanel;

    private bool _stalkerActive = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleEnergyIncrease();
    }

    public void AddEnergy(float amount)
    {
        if (EnergyValue >= _maxValue) return;

        EnergyValue += amount;
        EnergyValue = Mathf.Clamp(EnergyValue, 0f, _maxValue);
    }

    public void ReduceEnergy(float amount)
    {
        if (EnergyValue <= 0f)
        {
            _endUIPanel.GameOver("You have lost all your energy!");
            return;
        }

        EnergyValue -= amount;
        EnergyValue = Mathf.Clamp(EnergyValue, 0f, _maxValue);
    }

    private void HandleEnergyIncrease()
    {
        float speed = _passiveIncreaseSpeed;

        float stressMultiplier = EnergyValue / 500f;

        ReduceEnergy(speed * stressMultiplier * Time.deltaTime);
    }

}