using UnityEngine;

public class RadarTarget : MonoBehaviour
{
    void Start()
    {
        // Когда объект появляется, он сам лезет в радар
        if (RadarManager.Instance != null)
            RadarManager.Instance.Register(this.transform);
    }
}