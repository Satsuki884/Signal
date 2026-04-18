using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private Direction direction;

    private RoomsVariant rooms;
    private RoomTag parentRoom;

    private bool spawned = false;

    private void Awake()
    {
        rooms = GameObject.FindGameObjectWithTag("Rooms")?.GetComponent<RoomsVariant>();
        parentRoom = GetComponentInParent<RoomTag>();
    }

    private void Start()
    {
        Invoke(nameof(Spawn), 0.1f);
        Destroy(gameObject, 3f);
    }

    public void Spawn()
    {
        if (spawned) return;

        if (rooms == null || parentRoom == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 pos = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y)
        );

        if (RoomRegistry.Instance.IsOccupied(pos))
        {
            Destroy(gameObject);
            return;
        }

        // 🔥 РЕЗЕРВУЄМО ОДРАЗУ
        RoomRegistry.Instance.Register(pos);

        List<GameObject> pool = rooms.GetRooms(direction);
        List<GameObject> filtered;

        // 🔥 CLOSING
        if (GenerationManager.Instance.IsClosing())
        {
            filtered = pool
                .Where(r => r.GetComponent<RoomTag>()?.exitsCount == 1)
                .ToList();
        }
        else
        {
            LocationType parentType = parentRoom.runtimeLocation;
            LocationType chosenType;

            // 🔥 WORLD
            if (parentType == LocationType.World)
            {
                if (GenerationManager.Instance.CanStartNewZone())
                {
                    // 🔥 тільки якщо це той самий кластер
                    if (GenerationManager.Instance.IsNearZoneRoot(transform.position))
                    {
                        chosenType = GenerationManager.Instance.StartNextZone(transform);
                    }
                    else
                    {
                        chosenType = LocationType.World;
                    }
                }
                else
                {
                    chosenType = LocationType.World;
                }
            }
            else
            {
                // 🔥 ЗОНА РОСТЕ
                if (Random.value < 0.95f)
                {
                    chosenType = parentType;
                }
                else
                {
                    chosenType = LocationType.World;

                    // шанс завершити зону
                    if (Random.value < 0.3f)
                    {
                        GenerationManager.Instance.FinishZone();
                    }
                }
            }

            filtered = pool
                .Where(r => r.GetComponent<RoomTag>()?.locationType == chosenType)
                .ToList();

            if (chosenType == LocationType.World)
            {
                GenerationManager.Instance.RegisterWorldRoom();
            }
        }

        if (filtered.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        GameObject prefab = filtered[Random.Range(0, filtered.Count)];

        GameObject room = Instantiate(prefab, pos, Quaternion.identity);

        var tag = room.GetComponent<RoomTag>();
        tag.runtimeLocation = tag.locationType;

        spawned = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("RoomPoint") && other.GetComponent<RoomSpawner>().spawned)
        {
            Destroy(gameObject);
        }
    }
}