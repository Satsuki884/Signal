using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private KnobController musicKnob;
    [SerializeField] private KnobController sfxKnob;

    private void Start()
    {
        musicKnob.OnValueChanged += OnMusicChanged;
        sfxKnob.OnValueChanged += OnSFXChanged;
    }

    private void OnMusicChanged(float value)
    {
         AudioManager.Instanse.SetMusicVolume(value);
//        Debug.Log($"Music volume changed to: {value}");
    }

    private void OnSFXChanged(float value)
    {
         AudioManager.Instanse.SetSFXVolume(value);
  //      Debug.Log($"SFX volume changed to: {value}");
    }
}