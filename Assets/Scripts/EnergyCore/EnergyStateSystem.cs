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
    [SerializeField] private float _passiveReduseSpeed = 0.25f;

    // Нове поле: щоб не програвати LowBat кожен кадр
    private bool _lowBatPlayed = false;

    private void Awake()
    {
        Instance = this;
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
        _energyValueText.text = EnergyValue.ToString("F0");

        // Якщо енергія піднялась вище порогу — скидаємо прапорець, щоб звук міг програти знову при наступному падінні
        if (EnergyValue > _lowEnergyThreshold)
        {
            _lowBatPlayed = false;
        }
    }

    private void HandleLowEnergyBlink()
    {
        if (EnergyValue <= _lowEnergyThreshold)
        {
            if (!_isBlinking)
            {
                _blinkCoroutine = StartCoroutine(BlinkEnergyText());
                _isBlinking = true;

                // Програємо звук LowBat один раз при вході в стан низької енергії
                if (!_lowBatPlayed && AudioManager.Instanse != null && AudioManager.Instanse.LowBat != null)
                {
                    AudioManager.Instanse.PlaySFX(AudioManager.Instanse.LowBat);
                    _lowBatPlayed = true;
                }
            }
        }
        else
        {
            if (_isBlinking)
            {
                StopCoroutine(_blinkCoroutine);
                _isBlinking = false;

                // повертаємо нормальний стан
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
