using UnityEngine;

public class RoomTag : MonoBehaviour
{
    public LocationType locationType;
    [Tooltip("Кількість виходів у кімнати")]
    public int exitsCount;
    [HideInInspector] public LocationType runtimeLocation;
}