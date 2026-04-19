using UnityEngine;
using UnityEngine.UI;

public class DSonarMarker : MonoBehaviour
{
    public float lifeTime = 20f;
    private Image img;

    void Awake() => img = GetComponent<Image>();

    void Start()
    {
        // Плавное появление можно сделать тут или через Animator
        // А пока просто запускаем уничтожение
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Опционально: пусть маркер потихоньку тускнеет со временем
        float alpha = Mathf.Lerp(0, 1, lifeTime / 20f);
        // Но лучше просто оставить как есть, раз мы "замораживаем" снимок
    }
}