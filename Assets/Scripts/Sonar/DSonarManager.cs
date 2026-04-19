using UnityEngine;
using System.Collections.Generic;

public class DSonarManager : MonoBehaviour
{
    public static DSonarManager Instance;

    [Header("Settings")]
    public float minDistance = 20f;
    public float maxDistance = 100f;
    public float scanDuration = 2f; // За сколько секунд линия проходит экран

    [Header("UI Links")]
    public RectTransform sonarPanel;
    public RectTransform scanningLine;
    public GameObject markerPrefab; // Простая точка/иконка
    public Transform markerContainer;

    private bool isScanning = false;
    private float timer = 0f;
    private Transform player;

    void Awake() => Instance = this;
    void Start() => player = GameObject.FindGameObjectWithTag("Player").transform;

    public void RegisterHit(Vector3 targetWorldPos)
    {
        float dist = Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, targetWorldPos);
        CreateMarker(targetWorldPos, dist);
    }
    public void StartScan()
    {
        if (isScanning) return;

        // Очищаем старые маркеры перед новым сканом
        foreach (Transform child in markerContainer) Destroy(child.gameObject);

        isScanning = true;
        timer = 0f;

        // Физический скан (разовый замер всех в радиусе)
        PerformPhysicsScan();
    }

    void Update()
    {
        // Активация по кнопке R
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartScan();
        }

        if (!isScanning) return;

        timer += Time.deltaTime;
        float progress = timer / scanDuration;

        float panelHeight = sonarPanel.rect.height;
        scanningLine.anchoredPosition = new Vector2(0, progress * panelHeight);

        if (progress >= 1f) isScanning = false;
    }

    void PerformPhysicsScan()
    {
        // Ищем всех в радиусе 100 метров
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, maxDistance);

        foreach (var hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("DSonarTarget"))
            {
                float dist = Vector2.Distance(player.position, hit.transform.position);

                // Проверяем мертвую зону
                if (dist >= minDistance)
                {
                    // Считаем позицию для UI
                    CreateMarker(hit.transform.position, dist);
                }
            }
        }
    }

    void CreateMarker(Vector3 targetWorldPos, float distance)
    {
        // 1. Нормализуем дистанцию (0 = 20м, 1 = 100м)
        float t = (distance - minDistance) / (maxDistance - minDistance);

        // 2. Считаем направление (влево-вправо относительно игрока)
        Vector3 relativePos = player.InverseTransformPoint(targetWorldPos);

        // 3. Математика пирамиды (чем дальше, тем шире может быть X)
        float xRange = Mathf.Lerp(50f, sonarPanel.rect.width / 2, t);
        float uiX = (relativePos.x / 50f) * xRange; // 50f - условный охват по бокам в мире
        float uiY = t * sonarPanel.rect.height;

        // 4. Спавним маркер
        GameObject m = Instantiate(markerPrefab, markerContainer);
        RectTransform rt = m.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(uiX, uiY);

        // Иконка сначала невидима, её «проявит» линия (можно сделать через скрипт на маркере)
        m.SetActive(false);
        StartCoroutine(ShowMarkerDelayed(m, t * scanDuration));
    }

    System.Collections.IEnumerator ShowMarkerDelayed(GameObject marker, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (marker != null) marker.SetActive(true);
        // Тут же можно запустить таймер на 20 сек для удаления
        Destroy(marker, 20f);
    }
}