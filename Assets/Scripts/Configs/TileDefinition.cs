using System.Collections.Generic;
using UnityEngine;

public enum LocationType
{
    Any,
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
    [Header("Base Info")]
    [SerializeField] private string tileDefinitionName;
    public string TileName => tileDefinitionName;

    [Header("Connections")]
    [SerializeField] private TileDefinition connections;
    public TileDefinition Connections => connections;
}