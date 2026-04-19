using UnityEngine;

public class ItemBigSonar : MonoBehaviour
{
    [SerializeField] private PlayerSO _playerSO;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerSO.ObtainBigSonar();
            Debug.Log("Big Sonar obtained!");
            Destroy(gameObject);
        }
    }
}