using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Configs/Tile")]
public class TileData : ScriptableObject
{
    [SerializeField] private string tileName;
    public string TileName => tileName;
    [SerializeField] private List<TileDefinition> definition;
    public List<TileDefinition> Definition => definition;
    [SerializeField] private LocationType locationType;
    public LocationType LocationType => locationType;

    [SerializeField] private List<TileVariant> variants;
    public List<TileVariant> Variants => variants;

    public GameObject GetPrefab(LocationType location)
    {
        foreach (var v in variants)
        {
            if (v.locationType == location)
                return v.prefab;
        }

        foreach (var v in variants)
        {
            if (v.locationType == LocationType.Any)
                return v.prefab;
        }

        return variants.Count > 0 ? variants[0].prefab : null;
    }

}