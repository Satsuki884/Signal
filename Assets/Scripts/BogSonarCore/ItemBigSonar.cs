using UnityEngine;

public class ItemBigSonar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instanse != null && AudioManager.Instanse.interact != null)
                AudioManager.Instanse.PlaySFX(AudioManager.Instanse.interact);

            GameManager.Instance.ObtainBigSonar();
            Debug.Log("Big Sonar obtained!");
            Destroy(gameObject);
        }
    }
}