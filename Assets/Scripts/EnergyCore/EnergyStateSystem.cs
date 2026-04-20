using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyStateSystem : MonoBehaviour
{
    public static EnergyStateSystem Instance;

    [SerializeField] private Slider _energyValueSlider;
    [SerializeField] private TMP_Text _energyValueText;
    [SerializeField] private TMP_Text _energyReduseSpeedText;
    [SerializeField] private Color _energyFullColor = Color.white;
    [SerializeField] private Color _energySmallColor = Color.red;
    [SerializeField] private float _lowEnergyThreshold = 25.99f;
    [SerializeField] private float _blinkSpeed = 2f;

    private Coroutine _blinkCoroutine;
    private bool _isBlinking = false;

    [Range(0f, 100f)]
    public float EnergyValue = 100f;

    [SerializeField] private float _maxValue = 100f;

    [Header("Mental Increase")]
    [SerializeField] private float _passiveReduseSpeed = 0.0f;
    [SerializeField] private float _sonarPenalty = 1.0f;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _energyValueSlider.maxValue = _maxValue;
        _energyValueSlider.value = EnergyValue;
        _energyValueText.text = EnergyValue.ToString("F1");
    }
    private void Update()
    {
        HandleEnergyIncrease();
        HandleLowEnergyBlink();
    }

    public void AddEnergy(float amount)
    {
        if (EnergyValue >= _maxValue) return;

        EnergyValue += amount;
        EnergyValue = Mathf.Clamp(EnergyValue, 0f, _maxValue);
        _energyValueSlider.value = EnergyValue;
        _energyValueText.text = EnergyValue.ToString("F1");

    }

    private void HandleLowEnergyBlink()
    {
        if (EnergyValue <= _lowEnergyThreshold)
        {
            if (!_isBlinking)
            {
                _blinkCoroutine = StartCoroutine(BlinkEnergyText());
                _isBlinking = true;
            }
        }
        else
        {
            if (_isBlinking)
            {
                StopCoroutine(_blinkCoroutine);
                _isBlinking = false;

                _energyValueText.color = _energyFullColor;
            }
        }
    }

    private IEnumerator BlinkEnergyText()
    {
        while (true)
        {
            _energyValueText.color = _energySmallColor;
            yield return new WaitForSeconds(1f / _blinkSpeed);

            _energyValueText.color = _energyFullColor;
            yield return new WaitForSeconds(1f / _blinkSpeed);
        }
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
        _energyValueText.text = EnergyValue.ToString("F1"); 
    }

    private void HandleEnergyIncrease()
    {

        float currentSpeed = _passiveReduseSpeed;

        if (SonarController.Instance != null && SonarController.Instance.IsOn)
        {
            currentSpeed += _sonarPenalty;
        }

        float stressMultiplier = Mathf.Clamp(EnergyValue / 100f, 0.2f, 1f);

        _energyReduseSpeedText.text = "-" + currentSpeed.ToString("F2") + " /s";

        ReduceEnergy(currentSpeed * stressMultiplier * Time.deltaTime);
    }

    private float RoundTo3(float value)
    {
        return Mathf.Round(value * 1000f) / 1000f;
    }

}