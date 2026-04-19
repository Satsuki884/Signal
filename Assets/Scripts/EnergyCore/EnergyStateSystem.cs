using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyStateSystem : MonoBehaviour
{
    public static EnergyStateSystem Instance;

    [SerializeField] private Slider _energyValueSlider;
    [SerializeField] private TMP_Text _energyValueText;
    [SerializeField] private TMP_Text _energyReduseSpeedText;

    [Range(0f, 100f)]
    public float EnergyValue = 100f;

    [SerializeField] private float _maxValue = 100f;

    [Header("Mental Increase")]
    [SerializeField] private float _passiveReduseSpeed = 0.25f;
    // [SerializeField] private float _bigSonarBonusIncrease = 0.5f;

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
        _energyValueSlider.value = EnergyValue;
        _energyValueText.text = EnergyValue.ToString("F0");

    }

    public void ReduceEnergy(float amount)
    {
        if (EnergyValue <= 0f)
        {
            EndUIPanel.Instance.GameOver("You have lost all your energy!");
            return;
        }

        EnergyValue -= amount;
        EnergyValue = Mathf.Clamp(EnergyValue, 0f, _maxValue);
        _energyValueSlider.value = EnergyValue;
        _energyValueText.text = EnergyValue.ToString("F0");
    }

    private void HandleEnergyIncrease()
    {
        float speed = _passiveReduseSpeed;

        float stressMultiplier = EnergyValue / 100f;
        _energyReduseSpeedText.text = "-" + _passiveReduseSpeed.ToString("F2") + " /s";

        ReduceEnergy(speed * stressMultiplier * Time.deltaTime);
    }

}