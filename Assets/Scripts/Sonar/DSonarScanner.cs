using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DSonarScanner : MonoBehaviour
{
    [Header("Настройки")]
    public float dMinDistance = 20f;
    public float dMaxDistance = 100f;
    [Range(0, 180)] public float dViewAngle = 90f;
    public LayerMask dTargetLayer;

    [Header("Связь")]
    public DSonarUI dSonarUI;

    [Header("Тайминги")]
    public float dScanCooldown = 15f;
    private float dLastScanTime = -100f;

    void Update()
    {
        // Проверяем нажатие R
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            // 🔥 ПРОВЕРКА: Идём в GameManager -> PlayerData -> проверяем флаг HasBigSonar
            if (GameManager.Instance == null || GameManager.Instance.PlayerData == null || !GameManager.Instance.PlayerData.HasBigSonar)
            {
                Debug.Log("<color=red>[DSonarScanner]</color> Сонар еще не установлен на корабль! Найди предмет.");
                return; // Тормозим выполнение, скан не запускается
            }

            // Проверка кулдауна
            if (Time.time >= dLastScanTime + dScanCooldown)
            {
                if (EnergyStateSystem.Instance != null)
                {
                    EnergyStateSystem.Instance.ReduceEnergy(10f);
                }

                Debug.Log("<color=yellow>[DSonarScanner]</color> Кнопка R нажата! Запуск...");
                dLastScanTime = Time.time;
                DScanEnvironment();
            }
            else
            {
                float dWait = (dLastScanTime + dScanCooldown) - Time.time;
                Debug.Log($"<color=orange>[DSonarScanner]</color> Сонар перезаряжается! Жди {dWait:F1} сек.");
            }
        }
    }

    void DScanEnvironment()
    {
        Collider2D[] dHits = Physics2D.OverlapCircleAll(transform.position, dMaxDistance, dTargetLayer);
        Debug.Log($"<color=cyan>[DSonarScanner]</color> Физика нашла {dHits.Length} коллайдеров на слое.");

        List<DSonarUI.DTargetData> dTargetsToDisplay = new List<DSonarUI.DTargetData>();

        foreach (var dHit in dHits)
        {
            DSonarTarget dTargetInfo = dHit.GetComponent<DSonarTarget>();
            if (dTargetInfo == null)
            {
                Debug.Log($"<color=gray>[DSonarScanner]</color> Объект {dHit.name} пропущен (нет скрипта DSonarTarget)");
                continue;
            }

            Vector3 dLocalPos = transform.InverseTransformPoint(dHit.transform.position);
            float dDistance = dLocalPos.magnitude;


            float dAngle = Vector3.Angle(Vector3.right, dLocalPos);

            if (dDistance >= dMinDistance && dDistance <= dMaxDistance && dAngle <= dViewAngle / 2f)
            {
                float dNormalizedY = Mathf.Clamp01((dDistance - dMinDistance) / (dMaxDistance - dMinDistance));

                float dNormalizedX = -dLocalPos.y / Mathf.Max(dLocalPos.x, 0.1f);

                dTargetsToDisplay.Add(new DSonarUI.DTargetData
                {
                    dType = dTargetInfo.dType,
                    dNormX = dNormalizedX,
                    dNormY = dNormalizedY
                });
                Debug.Log($"<color=green>[DSonarScanner]</color> Цель {dHit.name} ЗАХВАЧЕНА. Dist: {dDistance:F1}, Angle: {dAngle:F1}");
            }
            else
            {
                Debug.Log($"<color=red>[DSonarScanner]</color> Цель {dHit.name} ОТСЕЯНА. Dist: {dDistance:F1}, Angle: {dAngle:F1}");
            }
        }

        if (dSonarUI != null)
        {
            Debug.Log($"<color=yellow>[DSonarScanner]</color> Отправляю {dTargetsToDisplay.Count} целей в UI.");
            dSonarUI.DDrawSnapshot(dTargetsToDisplay);
        }
        else
        {
            Debug.LogError("<color=red>[DSonarScanner]</color> Ссылка на dSonarUI не назначена в инспекторе!");
        }
    }
}