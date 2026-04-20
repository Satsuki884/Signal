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


    public AudioClip LowBat;
    public AudioClip interact;
    public AudioClip death;
    public AudioClip[] hit;

    [Header("---Collision Clips---")]
    public AudioClip[] collisionClips; // призначити 3 кліпи в інспекторі
    public float collisionVolume = 0.8f;
    public float collisionMinRelativeVelocity = 1.0f; // мінімальна відносна швидкість для звуку

    [Header("---Enemy Clips---")]
    public AudioClip enemyBreathClip;
    public AudioClip enemyAggroClip;
    public AudioClip enemyAttackClip;
    [Header("---Enemy Volume Multipliers---")]
    public float enemyBreathVolumeMultiplier = 2f;


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
    // AudioManager.cs — додати метод
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float spatialBlend = 0f)
    {
        if (clip == null) return;

        // Якщо є SFXSource і він прив'язаний до MixerGroup, використаємо його для PlayOneShot (не позиційно)
        // Для позиційного звучання створимо тимчасовий AudioSource, але встановимо той самий outputAudioMixerGroup
        if (SFXSource != null && SFXSource.outputAudioMixerGroup != null)
        {
            // Якщо spatialBlend == 0 — просто PlayOneShot через SFXSource (не позиційно)
            if (Mathf.Approximately(spatialBlend, 0f))
            {
                SFXSource.PlayOneShot(clip, Mathf.Clamp01(volume));
                return;
            }

            // Інакше — тимчасовий AudioSource для позиційного звучання, але з тим же MixerGroup
            GameObject go = new GameObject("TempSFX");
            go.transform.position = position;
            var src = go.AddComponent<AudioSource>();
            src.clip = clip;
            src.volume = Mathf.Clamp01(volume);
            src.spatialBlend = Mathf.Clamp01(spatialBlend); // 0..1
            src.outputAudioMixerGroup = SFXSource.outputAudioMixerGroup;
            src.Play();
            Object.Destroy(go, clip.length + 0.1f);
            return;
        }

        // Фолбек: якщо SFXSource не налаштований — використовуємо PlayClipAtPoint (старий варіант)
        AudioSource.PlayClipAtPoint(clip, position, Mathf.Clamp01(volume));
    }

    [ContextMenu("Test Play Aggro")]
    private void TestPlayAggro()
    {
        if (enemyAggroClip != null) PlaySFXAtPosition(enemyAggroClip, Camera.main != null ? Camera.main.transform.position : Vector3.zero, 1f, 0f);
        else Debug.LogWarning("enemyAggroClip is null in AudioManager");
    }

    // Alarm playback (reliable PlayOneShot) + cooldown
    private float lastAlarmTime = -Mathf.Infinity;
    public float alarmCooldown = 1f; // seconds between alarms

    public void PlayEnemyAlarm(float volume = 1f)
    {
        if (Enemy_alarm == null) return;

        if (SFXSource != null)
        {
            SFXSource.PlayOneShot(Enemy_alarm, Mathf.Clamp01(volume));
            return;
        }

        AudioSource.PlayClipAtPoint(Enemy_alarm, Camera.main != null ? Camera.main.transform.position : Vector3.zero, Mathf.Clamp01(volume));
    }

    public void PlayEnemyAlarmWithCooldown(float volume = 1f)
    {
        if (Time.time - lastAlarmTime < alarmCooldown) return;
        lastAlarmTime = Time.time;
        PlayEnemyAlarm(volume);
    }

    [ContextMenu("Test Play Enemy Alarm")]
    private void TestPlayEnemyAlarm()
    {
        if (Enemy_alarm != null)
            PlayEnemyAlarm(1f);
        else
            Debug.LogWarning("TestPlayEnemyAlarm: Enemy_alarm is null in AudioManager");
    }
    public void PlayCollisionSoundRandom(Vector3 position, float volumeMultiplier = 1f)
    {
        if (collisionClips == null || collisionClips.Length == 0) return;
        if (SFXSource != null)
        {
            var clip = collisionClips[Random.Range(0, collisionClips.Length)];
            SFXSource.PlayOneShot(clip, Mathf.Clamp01(collisionVolume * volumeMultiplier));
            return;
        }

        // fallback
        var fallbackClip = collisionClips[Random.Range(0, collisionClips.Length)];
        AudioSource.PlayClipAtPoint(fallbackClip, position, Mathf.Clamp01(collisionVolume * volumeMultiplier));
    }

}
