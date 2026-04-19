using UnityEngine;
using Unity.Cinemachine;
using TMPro;

public class EnergyPoint : MonoBehaviour
{
    [SerializeField] private float _energyAmount = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnergyStateSystem.Instance.AddEnergy(_energyAmount);
            Destroy(gameObject);
        }
    }
}