using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SonarController : MonoBehaviour
{
    public static SonarController Instance;

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Material sonarMat;
    [SerializeField] private Material backgroundMat;

    [SerializeField] private float maxRadius = 50f;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float aggroDuration = 15f;

    private float currentRadius;
    private bool isToggledOn;

    public event Action<bool> OnSonarStateChanged;
    public bool IsOn => isToggledOn;

    void Update()
    {
        if (sonarMat == null) return;

        if (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
        {
            ToggleSonar();
        }

        if (isToggledOn)
        {
            Vector4 playerPos = transform.position;
            sonarMat.SetVector("_PulsePos", playerPos);
            if (backgroundMat != null) backgroundMat.SetVector("_PulsePos", playerPos);

            float previousRadius = currentRadius;
            currentRadius += Time.deltaTime * speed;

            CheckEnemiesInWave(previousRadius, currentRadius);

            if (currentRadius > maxRadius)
                currentRadius = 0f;

            UpdateRadius(currentRadius);
        }
    }

    private void CheckEnemiesInWave(float minR, float maxR)
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (var enemy in enemies)
        {

            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist >= minR && dist <= maxR)
            {
                enemy.TriggerSonarAggro(aggroDuration);
            }
        }
    }


    public void ToggleSonar()
    {
        isToggledOn = !isToggledOn;

        if (!isToggledOn)
            ResetSonar();

        OnSonarStateChanged?.Invoke(isToggledOn);
    }

    public void SetSonar(bool state)
    {
        isToggledOn = state;

        if (!isToggledOn)
            ResetSonar();

        OnSonarStateChanged?.Invoke(isToggledOn);
    }

    private void UpdateRadius(float radius)
    {
        sonarMat.SetFloat("_Radius", radius);
        if (backgroundMat != null) backgroundMat.SetFloat("_Radius", radius);
    }

    private void ResetSonar()
    {
        currentRadius = 0f;
        UpdateRadius(0f);
    }
}