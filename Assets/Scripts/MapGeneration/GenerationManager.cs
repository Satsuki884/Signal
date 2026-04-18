using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    public static GenerationManager Instance;

    private int currentZoneIndex = 0;
    private LocationType[] zoneOrder = new[]
    {
        LocationType.A,
        LocationType.B,
        LocationType.C
    };

    private bool zoneStarted = false;

    private bool isClosing = false;
    private int endWorldCount = 0;
    private int maxEndWorld = 6;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanStartNewZone()
    {
        return !zoneStarted && currentZoneIndex < zoneOrder.Length;
    }

    public LocationType StartNextZone(Transform root)
    {
        zoneStarted = true;
        currentZoneRoot = root;

        return zoneOrder[currentZoneIndex];
    }

    public void FinishZone()
    {
        zoneStarted = false;
        currentZoneIndex++;
    }
    private Transform currentZoneRoot;
    public bool IsNearZoneRoot(Vector2 pos)
    {
        if (currentZoneRoot == null) return true;

        float dist = Vector2.Distance(pos, currentZoneRoot.position);

        return dist < 3f; // 👉 можна 2-4
    }

    public void RegisterWorldRoom()
    {
        endWorldCount++;

        if (endWorldCount >= maxEndWorld)
        {
            isClosing = true;
        }
    }

    public bool IsClosing()
    {
        return isClosing;
    }
}