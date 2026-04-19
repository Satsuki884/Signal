using UnityEngine;

public class ItemLocator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Відтворюємо звук підбору
            if (AudioManager.Instanse != null && AudioManager.Instanse.interact != null)
                AudioManager.Instanse.PlaySFX(AudioManager.Instanse.interact);

            GameManager.Instance.ObtainLocator();
            Debug.Log("Locator obtained!");
            Destroy(gameObject);
        }
    }
}