using UnityEngine;
using System.Collections.Generic;

public class DSonarUI : MonoBehaviour
{

    public struct DTargetData
    {
        public DSonarTarget.DTargetType dType;
        public float dNormX;
        public float dNormY;
    }

    [System.Serializable]
    public struct DMarkerMapping
    {
        public DSonarTarget.DTargetType dType;
        public GameObject dPrefab;
    }

    [Header("UI Настройки")]
    public RectTransform dMarkerContainer;
    public List<DMarkerMapping> dMarkerMappings;
    public float dMarkerLifetime = 20f;

    [Header("Линия сканирования")]
    public RectTransform dScanLine;
    public float dScanSpeed = 200f;

    public void DDrawSnapshot(List<DTargetData> dTargets)
    {
        foreach (Transform dChild in dMarkerContainer) Destroy(dChild.gameObject);

        StopAllCoroutines();
        StartCoroutine(DAnimateScanLine());

        float dWidth = dMarkerContainer.rect.width;
        float dHeight = dMarkerContainer.rect.height;

        foreach (var dTarget in dTargets)
        {
            GameObject dPrefabToSpawn = dMarkerMappings.Find(m => m.dType == dTarget.dType).dPrefab;
            if (dPrefabToSpawn == null) continue;

            GameObject dMarker = Instantiate(dPrefabToSpawn, dMarkerContainer);

            var dMarkerLogic = dMarker.AddComponent<DSonarMarker>();

            RectTransform dRT = dMarker.GetComponent<RectTransform>();
            dRT.anchorMin = dRT.anchorMax = dRT.pivot = new Vector2(0.5f, 0f);

            float dPosX = dTarget.dNormX * (dWidth / 2f);
            float dPosY = dTarget.dNormY * dHeight;
            dRT.anchoredPosition = new Vector2(dPosX, dPosY);

            dMarkerLogic.DInit(dScanLine, 10f);
        }
    }

    private System.Collections.IEnumerator DAnimateScanLine()
    {
        dScanLine.gameObject.SetActive(true);

        float dTargetY = dMarkerContainer.rect.height;
        dScanLine.anchoredPosition = Vector2.zero;

        while (dScanLine.anchoredPosition.y < dTargetY)
        {
            dScanLine.anchoredPosition += Vector2.up * dScanSpeed * Time.deltaTime;
            yield return null;
        }

        dScanLine.gameObject.SetActive(false);
    }
}