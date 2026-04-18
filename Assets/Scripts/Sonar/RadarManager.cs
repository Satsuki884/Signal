using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public static RadarManager Instance;

    [Header("Ссылки")]
    public Transform player;
    public RectTransform markerParent;
    public GameObject markerPrefab;

    [Header("Настройки")]
    public float radarRange = 50f;
    public float radarSize = 100f;
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


            Vector2 diff = (Vector2)targets[i].target.position - (Vector2)player.position;
            float dist = diff.magnitude;

            
            Vector2 dir = dist > 0.1f ? diff.normalized : Vector2.up;
            targets[i].marker.anchoredPosition = dir * radarSize;

            
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