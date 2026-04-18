using UnityEngine;

public class SonarController : MonoBehaviour
{
    [SerializeField] private Material sonarMat;      // Стены
    [SerializeField] private Material backgroundMat; // Фон

    [SerializeField] private float maxRadius = 50f;
    [SerializeField] private float speed = 15f;

    private float currentRadius;
    private bool isToggledOn; // Состояние: работает сонар или нет

    void Update()
    {
        if (sonarMat == null) return;

        // Если нажал Z — меняем состояние на противоположное
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isToggledOn = !isToggledOn;

            // Если выключили — мгновенно гасим радиус в шейдерах
            if (!isToggledOn)
            {
                ResetSonar();
            }
        }

        if (isToggledOn)
        {
            // Качаем данные позиции (чтобы волна всегда шла от игрока)
            Vector4 playerPos = transform.position;
            sonarMat.SetVector("_PulsePos", playerPos);
            if (backgroundMat != null) backgroundMat.SetVector("_PulsePos", playerPos);

            // Увеличиваем радиус
            currentRadius += Time.deltaTime * speed;

            // ГЛАВНАЯ ФИШКА: Зацикливание
            // Если радиус превысил макс, сбрасываем его в 0, и он идет по новой
            if (currentRadius > maxRadius)
            {
                currentRadius = 0f;
            }

            // Отправляем радиус в шейдеры
            UpdateRadius(currentRadius);
        }
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