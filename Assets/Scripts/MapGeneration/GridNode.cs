using UnityEngine;
using System.Collections.Generic;

public class RoomNode
{
    public Vector2Int pos;
    public LocationType type;
    public List<Vector2Int> connections = new();

    public RoomNode(Vector2Int p, LocationType t)
    {
        pos = p;
        type = t;
    }
}