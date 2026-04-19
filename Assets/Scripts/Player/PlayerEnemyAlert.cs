using UnityEngine;

public class PlayerEnemyAlert : MonoBehaviour
{
    [Header("Alert Settings")]
    [SerializeField] private float alertRadius = 8f;
    [SerializeField] private LayerMask enemyLayer; // налаштуй на шар ворог≥в
    [SerializeField] private float checkInterval = 0.2f; // €к часто перев≥р€ти (сек)
    [SerializeField] private float graceTime = 0.8f; // час без ворог≥в перед скиданн€м стану (сек)
    [SerializeField] private float replayCooldown = 10f; // м≥н≥мальний ≥нтервал перед повторним програванн€м (сек)

    private float lastEnemySeenTime = -Mathf.Infinity;
    private float lastPlayedTime = -Mathf.Infinity;
    private bool isAlerted = false;
    private float nextCheckTime = 0f;

    private void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            CheckForEnemies();
        }

        // якщо ворог≥в не бачили довше graceTime Ч скидаЇмо стан alerted (щоб звук м≥г програти знову п≥зн≥ше)
        if (isAlerted && Time.time - lastEnemySeenTime >= graceTime)
        {
            isAlerted = false;
        }
    }

    private void CheckForEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, alertRadius, enemyLayer);
        if (hits != null && hits.Length > 0)
        {
            lastEnemySeenTime = Time.time;

            // якщо ще не були в alerted стан≥ ≥ пройшов cooldown з останнього програванн€ Ч граЇмо звук один раз
            if (!isAlerted && Time.time - lastPlayedTime >= replayCooldown)
            {
                PlayAlertSoundOnce();
                isAlerted = true;
                lastPlayedTime = Time.time;
            }
        }
    }

    private void PlayAlertSoundOnce()
    {
        if (AudioManager.Instanse == null) return;

        // ¬икористовуЇмо одноразове в≥дтворенн€ через SFXSource (PlayOneShot)
        if (AudioManager.Instanse.Enemy_alarm != null)
        {
            AudioManager.Instanse.PlaySFX(AudioManager.Instanse.Enemy_alarm);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
