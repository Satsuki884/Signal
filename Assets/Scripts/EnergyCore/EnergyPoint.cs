using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    [SerializeField] private float _energyAmount = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnergyStateSystem.Instance.AddEnergy(_energyAmount);
            Destroy(gameObject);
        }
    }
}