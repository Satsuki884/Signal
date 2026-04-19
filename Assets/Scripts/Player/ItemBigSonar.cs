using UnityEngine;

public class ItemBigSonar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ObtainBigSonar();
            Debug.Log("Big Sonar obtained!");
            Destroy(gameObject);
        }
    }
}