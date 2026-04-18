using UnityEngine;
using System.Linq;

public class StartRoomSpawner : MonoBehaviour
{
    [SerializeField] private RoomsVariant rooms;
    [SerializeField] private LocationType startLocation;

    private void Start()
    {
        var prefab = rooms.GetAllRooms()
            .Where(r => r.GetComponent<RoomTag>()?.locationType == startLocation)
            .OrderBy(_ => Random.value)
            .FirstOrDefault();

        if (prefab == null)
        {
            Debug.LogError("❌ Нема стартової кімнати");
            return;
        }

        var room = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        room.GetComponent<RoomTag>().runtimeLocation = startLocation;
    }
}