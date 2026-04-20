using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damageCooldown = 5f; // Время перезарядки в секундах (можешь менять в Юнити)
    private float nextDamageTime = 0f; // Время, когда можно будет нанести следующий удар

    private void OnCollisionStay2D(Collision2D other)
    {
        // Проверяем тег И то, что текущее время игры перевалило за таймер
        if (other.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            GameManager.Instance.TakeDamage(1);

            // Заряжаем таймер на будущее: текущее время + 5 секунд
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}