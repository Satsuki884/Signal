using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instanse;

    private void Awake()
    {
        Instanse = this;
    }

    [Header("---Audio Source---")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource engineSource;

    [SerializeField] private AudioMixer audioMixer;

    [Header("---Audio Clip---")]
    public AudioClip background;
    public AudioClip SubWalk;
    public AudioClip Sonar;
    public AudioClip Enemy_alarm;
    public AudioClip Enemy_scream;
    public AudioClip LowBat;
    public AudioClip interact;
    public AudioClip death;
    public AudioClip[] hit;
    // Engine sound state
    private Coroutine engineCoroutine;
    private bool isEngineRunning;

    // ================= MUSIC =================

    public void StartMusic()
    {
        if (background == null || audioSource == null) return;

        audioSource.clip = background;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (audioSource != null)
            audioSource.UnPause();
    }

    public void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    // ================= VOLUME =================

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", value);
    }

    public void SetEngineVolume(float value)
    {
        audioMixer.SetFloat("EngineVolume", value);
    }

    // ================= SFX =================

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || SFXSource == null) return;

        SFXSource.pitch = 1f;
        SFXSource.PlayOneShot(clip);
    }

    // ================= ENGINE =================

    public void SetEngineState(bool moving)
    {
        if (isEngineRunning == moving) return;

        if (moving)
        {
            isEngineRunning = true;

            // Якщо корутина вже йде — зупиняємо її і запускаємо заново, щоб уникнути дублювання
            if (engineCoroutine != null)
            {
                StopCoroutine(engineCoroutine);
                engineCoroutine = null;
            }

            engineCoroutine = StartCoroutine(EngineLoop());
        }
        else
        {
            // Встановлюємо прапорець false — корутина EngineLoop побачить це і зробить плавний fade‑out
            isEngineRunning = false;
            // НЕ викликаємо тут StopEngine(), інакше fade‑out не відбудеться
        }
    }

    private IEnumerator EngineLoop()
    {
        engineSource.clip = SubWalk;
        engineSource.loop = true;

        // Початковий низький тон
        engineSource.pitch = 0.1f;
        engineSource.Play();

        float targetPitch = 1f;
        float speed = 1.5f;

        while (isEngineRunning)
        {
            engineSource.pitch = Mathf.MoveTowards(engineSource.pitch, targetPitch, Time.deltaTime * speed);
            yield return null;
        }

        // Плавне затухання
        while (engineSource.pitch > 0.05f)
        {
            engineSource.pitch = Mathf.MoveTowards(engineSource.pitch, 0f, Time.deltaTime * speed);
            yield return null;
        }

        engineSource.Stop();
    }

    private void StopEngine()
    {
        if (engineCoroutine != null)
            StopCoroutine(engineCoroutine);

        engineCoroutine = null;
    }
}
