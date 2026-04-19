using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TilesSO", menuName = "Configs/Tiles")]
public class TilesSO : ScriptableObject
{
    [SerializeField] private List<TileData> tileDatas = new List<TileData>();
    public List<TileData> GetTileDatas() => tileDatas;
    
    public TileData GetTileData(string tileName) => tileDatas.Find(t => t.TileName == tileName);
}