using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    [SerializeField] private float _energyAmount = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instanse != null && AudioManager.Instanse.interact != null)
                AudioManager.Instanse.PlaySFX(AudioManager.Instanse.interact);

            EnergyStateSystem.Instance.AddEnergy(_energyAmount);
            Destroy(gameObject);
        }
    }
}