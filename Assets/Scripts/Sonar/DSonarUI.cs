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

    public void DDrawSnapshot(List<DTargetData> dTargets)
    {

        foreach (Transform dChild in dMarkerContainer)
        {
            Destroy(dChild.gameObject);
        }

        float dWidth = dMarkerContainer.rect.width;
        float dHeight = dMarkerContainer.rect.height;


        foreach (var dTarget in dTargets)
        {

            GameObject dPrefabToSpawn = dMarkerMappings.Find(m => m.dType == dTarget.dType).dPrefab;
            if (dPrefabToSpawn == null) continue;

            GameObject dMarker = Instantiate(dPrefabToSpawn, dMarkerContainer);
            RectTransform dRT = dMarker.GetComponent<RectTransform>();

            dRT.anchorMin = new Vector2(0.5f, 0f);
            dRT.anchorMax = new Vector2(0.5f, 0f);
            dRT.pivot = new Vector2(0.5f, 0f);


            float dPosX = dTarget.dNormX * (dWidth / 2f);
            float dPosY = dTarget.dNormY * dHeight;

            dRT.anchoredPosition = new Vector2(dPosX, dPosY);

            Destroy(dMarker, dMarkerLifetime);
        }
    }
}