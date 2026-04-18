//using System.Collections.Generic;
//using UnityEngine;

//public class MapGenerator : MonoBehaviour
//{
//    [Header("Data")]
//    [SerializeField] private TilesSO tilesSO;

//    [Header("Spawn")]
//    [SerializeField] private Transform parent;

//    private List<(TileData tile, LocationType location)> result = new();

//    private void Start()
//    {
//        Generate();
//        Spawn();
//    }

//    // =========================
//    // 🔥 GENERATION
//    // =========================
//    private void Generate()
//    {
//        result.Clear();

//        // START
//        TileData start = GetRandomTile(LocationType.World, true);
//        result.Add((start, LocationType.World));

//        TileDefinition exit = GetExitSide(start);

//        // A
//        exit = GenerateLocation(LocationType.A, exit);

//        // WORLD
//        TileData world1 = GetNextTile(GetRequired(exit), LocationType.World, true);
//        result.Add((world1, LocationType.World));
//        exit = GetExitSide(world1);

//        // B
//        exit = GenerateLocation(LocationType.B, exit);

//        // WORLD
//        TileData world2 = GetNextTile(GetRequired(exit), LocationType.World, true);
//        result.Add((world2, LocationType.World));
//        exit = GetExitSide(world2);

//        // C
//        exit = GenerateLocation(LocationType.C, exit);

//        // END
//        TileData end = GetNextTile(GetRequired(exit), LocationType.World, true);
//        result.Add((end, LocationType.World));
//    }

//    private TileDefinition GenerateLocation(LocationType type, TileDefinition вход)
//    {
//        // ENTRY
//        TileData entry = GetNextTile(GetRequired(вход), type);
//        result.Add((entry, type));

//        TileDefinition exit = GetExitSide(entry);

//        int length = Random.Range(4, 8);

//        for (int i = 0; i < length; i++)
//        {
//            TileData next = GetNextTile(GetRequired(exit), type);

//            if (next == null)
//                break;

//            result.Add((next, type));
//            exit = GetExitSide(next);

//            if (Random.value < 0.3f)
//                GenerateBranch(exit, type);
//        }

//        // EXIT
//        TileData exitTile = GetNextTile(GetRequired(exit), type);
//        result.Add((exitTile, type));

//        return GetExitSide(exitTile);
//    }

//    private void GenerateBranch(TileDefinition exit, LocationType type)
//    {
//        int length = Random.Range(1, 4);

//        for (int i = 0; i < length; i++)
//        {
//            TileData next = GetNextTile(GetRequired(exit), type);

//            if (next == null)
//                break;

//            result.Add((next, type));
//            exit = GetExitSide(next);
//        }
//    }

//    // =========================
//    // 🧱 SPAWN (2D)
//    // =========================
//    private void Spawn()
//    {
//        if (parent != null)
//        {
//            for (int i = parent.childCount - 1; i >= 0; i--)
//                Destroy(parent.GetChild(i).gameObject);
//        }

//        Vector2 pos = Vector2.zero;
//        float prevHalf = 0f;

//        foreach (var item in result)
//        {
//            var tile = item.tile;
//            var location = item.location;

//            GameObject prefab = tile.GetPrefab(location);

//            if (prefab == null)
//            {
//                Debug.LogError("No prefab for " + tile.TileName);
//                continue;
//            }

//            float size = GetTileSize(prefab);
//            float half = size / 2f;

//            pos.x += prevHalf + half;

//            Instantiate(prefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity, parent);

//            prevHalf = half;
//        }
//    }

//    private float GetTileSize(GameObject prefab)
//    {
//        var sr = prefab.GetComponentInChildren<SpriteRenderer>();
//        if (sr != null)
//            return sr.bounds.size.x;

//        var col = prefab.GetComponentInChildren<BoxCollider2D>();
//        if (col != null)
//            return col.bounds.size.x;

//        return 10f;
//    }

//    // =========================
//    // 🔗 LOGIC
//    // =========================

//    private TileDefinition GetExitSide(TileData tile)
//    {
//        if (tile.Definition.Count == 1)
//            return tile.Definition[0];

//        return tile.Definition[Random.Range(0, tile.Definition.Count)];
//    }

//    private TileDefinition GetRequired(TileDefinition exit)
//    {
//        return exit.Connections;
//    }

//    private TileData GetNextTile(TileDefinition required, LocationType location, bool deadEndOnly = false)
//    {
//        List<TileData> candidates = new();

//        foreach (var tile in tilesSO.GetTileDatas())
//        {
//            if (!HasVariant(tile, location))
//                continue;

//            bool ok = false;

//            foreach (var def in tile.Definition)
//            {
//                if (def == required)
//                {
//                    ok = true;
//                    break;
//                }
//            }

//            if (!ok)
//                continue;

//            if (deadEndOnly && tile.Definition.Count != 1)
//                continue;

//            candidates.Add(tile);
//        }

//        if (candidates.Count == 0)
//        {
//            Debug.LogError($"NO TILE: {location} need {required.TileName}");
//            return null;
//        }

//        return candidates[Random.Range(0, candidates.Count)];
//    }

//    private TileData GetRandomTile(LocationType location, bool deadEndOnly = false)
//    {
//        List<TileData> candidates = new();

//        foreach (var tile in tilesSO.GetTileDatas())
//        {
//            if (!HasVariant(tile, location))
//                continue;

//            if (deadEndOnly && tile.Definition.Count != 1)
//                continue;

//            candidates.Add(tile);
//        }

//        return candidates[Random.Range(0, candidates.Count)];
//    }

//    private bool HasVariant(TileData tile, LocationType location)
//    {
//        foreach (var v in tile.Variants)
//        {
//            if (v.locationType == location || v.locationType == LocationType.Any)
//                return true;
//        }

//        return false;
//    }
//}