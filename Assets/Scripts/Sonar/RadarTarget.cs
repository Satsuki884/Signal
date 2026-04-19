using UnityEngine;

public enum TargetType
{
    Enemy,
    Location
}

public class RadarTarget : MonoBehaviour
{
    [SerializeField] private TargetType _type;

    void Start()
    {
        
        if (RadarManager.Instance != null)
            RadarManager.Instance.Register(this.transform, _type);
    }
}