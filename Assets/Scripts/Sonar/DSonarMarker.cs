using UnityEngine;
using UnityEngine.UI;

public class DSonarMarker : MonoBehaviour
{
    private RectTransform dMyRT;
    private RectTransform dLineRT;
    private bool dIsActivated = false;
    private float dLifeTime;
    private CanvasGroup dCanvasGroup;

    public void DInit(RectTransform line, float life)
    {
        dMyRT = GetComponent<RectTransform>();
        dLineRT = line;
        dLifeTime = life;

        dCanvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (dCanvasGroup == null) dCanvasGroup = gameObject.AddComponent<CanvasGroup>();

        dCanvasGroup.alpha = 0; // Скрываем сразу
    }

    void Update()
    {
        if (dIsActivated) return;

        if (dLineRT != null && dLineRT.anchoredPosition.y >= dMyRT.anchoredPosition.y)
        {
            DActivate();
        }
    }

    void DActivate()
    {
        dIsActivated = true;
        dCanvasGroup.alpha = 1;
        Debug.Log("<color=green>[Marker]</color> Точка активирована линией!");


        Destroy(gameObject, dLifeTime);
    }
}