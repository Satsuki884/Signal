using System.Collections.Generic;
using UnityEngine;

public enum LocationType
{
    World,
    A,
    B,
    C
}

[System.Serializable]
public class TileVariant
{
    public LocationType locationType;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "TileDefinition", menuName = "Configs/Tile Definition")]
public class TileDefinition : ScriptableObject
{
    [SerializeField] private List<TileVariant> topRooms;
    public List<TileVariant> TopRooms => topRooms;
    [SerializeField] private List<TileVariant> bottomRooms;
    public List<TileVariant> BottomRooms => bottomRooms;
    [SerializeField] private List<TileVariant> leftRooms;
    public List<TileVariant> LeftRooms => leftRooms;
    [SerializeField] private List<TileVariant> rightRooms;
    public List<TileVariant> RightRooms => rightRooms;
}