using UnityEngine;

public class ItemLocator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ObtainLocator();
            Debug.Log("Locator obtained!");
            Destroy(gameObject);
        }
    }
}