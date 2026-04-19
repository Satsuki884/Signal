using System.Collections.Generic;
using UnityEngine;

public class RoomRegistry : MonoBehaviour
{
    public static RoomRegistry Instance;

    private HashSet<Vector2> occupied = new();

    private void Awake()
    {
        Instance = this;
    }

    public bool IsOccupied(Vector2 pos)
    {
        return occupied.Contains(pos);
    }

    public void Register(Vector2 pos)
    {
        occupied.Add(pos);
    }
}