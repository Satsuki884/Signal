using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public static RadarManager Instance;

    [Header("Ссылки")]
    public Transform player;          // Сюда игрока
    public RectTransform markerParent; // Тот самый MarkerContainer
    public GameObject markerPrefab;    // Твой префаб знака вопроса

    [Header("Настройки")]
    public float radarRange = 50f;     // Дистанция, на которой скейл минимальный
    public float radarSize = 100f;     // Радиус твоего UI круга (если радар 200, то тут 100)
    public float minScale = 0.5f;
    public float maxScale = 2.0f;

    private List<RadarData> targets = new List<RadarData>();

    void Awake() => Instance = this;

    public void Register(Transform target)
    {
        GameObject m = Instantiate(markerPrefab, markerParent);
        targets.Add(new RadarData { target = target, marker = m.GetComponent<RectTransform>() });
    }

    void Update()
    {
        if (player == null) return;

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i].target == null)
            {
                Destroy(targets[i].marker.gameObject);
                targets.RemoveAt(i);
                continue;
            }

            // 1. Считаем вектор в 2D
            Vector2 diff = (Vector2)targets[i].target.position - (Vector2)player.position;
            float dist = diff.magnitude;

            // 2. Прибиваем к краю
            Vector2 dir = dist > 0.1f ? diff.normalized : Vector2.up;
            targets[i].marker.anchoredPosition = dir * radarSize;

            // 3. Масштаб по дистанции (чем ближе, тем больше)
            float t = Mathf.Clamp01(1 - (dist / radarRange));
            targets[i].marker.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, t);
        }
    }

    private class RadarData
    {
        public Transform target;
        public RectTransform marker;
    }
}