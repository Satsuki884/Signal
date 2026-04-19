using System.Collections.Generic;
using UnityEngine;

public class RoomsVariant : MonoBehaviour
{
    [SerializeField] private List<GameObject> topRooms;
    [SerializeField] private List<GameObject> bottomRooms;
    [SerializeField] private List<GameObject> leftRooms;
    [SerializeField] private List<GameObject> rightRooms;

    public List<GameObject> GetRooms(Direction dir)
    {
        return dir switch
        {
            Direction.Top => topRooms,
            Direction.Bottom => bottomRooms,
            Direction.Left => leftRooms,
            Direction.Right => rightRooms,
            _ => null
        };
    }

    public List<GameObject> GetAllRooms()
    {
        List<GameObject> all = new();

        all.AddRange(topRooms);
        all.AddRange(bottomRooms);
        all.AddRange(leftRooms);
        all.AddRange(rightRooms);

        return all;
    }
}