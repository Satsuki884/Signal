using UnityEngine;

public class RadarTarget : MonoBehaviour
{
    void Start()
    {
        
        if (RadarManager.Instance != null)
            RadarManager.Instance.Register(this.transform);
    }
}