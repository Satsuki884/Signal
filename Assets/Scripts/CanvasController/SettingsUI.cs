using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private KnobController musicKnob;
    [SerializeField] private KnobController sfxKnob;
    [SerializeField] private KnobController engineKnob;

    private void Start()
    {
        musicKnob.OnValueChanged += OnMusicChanged;
        sfxKnob.OnValueChanged += OnSFXChanged;
        engineKnob.OnValueChanged += OnEngineChanged;
    }

    private void OnMusicChanged(float value)
    {
        AudioManager.Instanse.SetMusicVolume(value);
    }

    private void OnSFXChanged(float value)
    {
        AudioManager.Instanse.SetSFXVolume(value);
    }

    private void OnEngineChanged(float value)
    {
        AudioManager.Instanse.SetEngineVolume(value);
    }
}