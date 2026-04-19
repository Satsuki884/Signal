using UnityEngine;

public class ItemLocator : MonoBehaviour
{
    [SerializeField] private PlayerSO _playerSO;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerSO.ObtainLocator();
            Debug.Log("Locator obtained!");
            Destroy(gameObject);
        }
    }
}