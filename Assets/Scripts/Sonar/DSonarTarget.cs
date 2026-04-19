using UnityEngine;

public class DSonarTarget : MonoBehaviour
{
    public void OnPing()
    {
        // Обращаемся к DSonarManager через Instance и вызываем RegisterHit
        if (DSonarManager.Instance != null)
        {
            DSonarManager.Instance.RegisterHit(transform.position);
        }
    }
}