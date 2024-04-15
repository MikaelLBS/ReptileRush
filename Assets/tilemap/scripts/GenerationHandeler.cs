using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static GameData;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class GenerationHandeler : MonoBehaviour, IDataPersitiens
{
    [System.Serializable]
    public class SpawnParams : RandSpawnLocations
    {
        public SpawnParams() { }
        public SpawnParams(SpawnParams sp)
        {
            recParam = sp.recParam;
            maxPoint = sp.maxPoint;
            minPoint = sp.minPoint;
            cycleDelay = sp.cycleDelay;
            cycle = sp.cycle;
            spawnChance = sp.spawnChance;
            minions = sp.minions;
            recSize = sp.recSize;
        }
        public SpawnParams(GameData.SaveSpawners sp)
        {
            maxPoint = sp.maxPoint;
            minPoint = sp.minPoint;
            cycleDelay = sp.cycleDelay;
            cycle = sp.cycle;
            spawnChance = sp.spawnChance;
            recSize = new Vector2(sp.recSize.x, sp.recSize.y);

            minions = new GameObject[sp.WildMinions.Length];
            for (int i = 0; i < sp.WildMinions.Length; i++)
            {
                minions[i] = EntityManager.SaveToWild(ref sp.WildMinions[i], false);
            }
        }
        public Vector2 recSize;
    }
    [System.Serializable]
    public class OtherTiles
    {
        public TileBase[] upSatirs;
        public TileBase[] downSatirs;
        public TileBase[] ground;
        public TileBase[] celling;
        public TileBase[] rightCornorUp;
        public TileBase[] leftCornorUp;
        public TileBase[] rightCornorDown;
        public TileBase[] leftCornorDown;
        public TileBase[] wallRight;
        public TileBase[] wallLeft;
    }

    [SerializeField] RandomMinionSpawning minionSpawning;
    [SerializeField] SpawnParams[] spawnBoxes;
    [SerializeField] int mainPathLength;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap stairsTilemap;
    [SerializeField] TileBase[] wallTiles;
    [SerializeField] OtherTiles otherTiles;
    [SerializeField] Vector2Int stepsBetwenSpawners;
    
    [NonSerialized] public List<GameData.SaveSpawners> saveSpawners = new List<SaveSpawners>();
    [Header("Background")]
    [SerializeField] Tilemap bTilemap;
    void CreateCave()
    {
        TilePlacer.FillTiles(ref tilemap, ref wallTiles, new Vector2Int(-50, -mainPathLength / 2), new Vector2Int(mainPathLength, 80));
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();
        Vector2Int endPos = TilePlacer.GeneratePath(Vector2Int.zero, mainPathLength, ref path, new TilePlacer.GenDirChances());

        int cunter = 0;
        int spawnAtCunter = Random.Range(stepsBetwenSpawners.x, stepsBetwenSpawners.y);
        foreach (Vector3Int pos in path)
        {
            if (spawnAtCunter == cunter)
            {
                spawnAtCunter += Random.Range(stepsBetwenSpawners.x, stepsBetwenSpawners.y);
                SpawnParams spawnBox = spawnBoxes[Random.Range(0, spawnBoxes.Length)];
                spawnBox.recParam = new GameObject().AddComponent<RectTransform>();
                spawnBox.recParam.position = tilemap.CellToWorld(pos);
                spawnBox.recParam.sizeDelta = spawnBox.recSize;
                saveSpawners.Add(new(spawnBox));
                saveSpawners[saveSpawners.Count-1].pos = (spawnBox.recParam.position.x, spawnBox.recParam.position.y);
                minionSpawning.AddSpawnLoacation(spawnBox);
            }
            cunter++;
        }
        TilePlacer.RemoveTiles(ref tilemap, ref path, 4);

        path.AddRange(TilePlacer.CreateRoom(ref tilemap, 900, Vector2Int.one * -3, Vector2Int.one * 3));
        path.AddRange(TilePlacer.CreateRoom(ref tilemap, 300, endPos + Vector2Int.one * -5, endPos + Vector2Int.one * 3));

        TilePlacer.PlaceTiles(ref bTilemap, ref wallTiles, path, 0);

        PlaceOtherTiles(ref path);
    }
    void PlaceOtherTiles(ref HashSet<Vector3Int> path)
    {
        HashSet<Vector3Int> edges = new HashSet<Vector3Int>();
        foreach (Vector3Int vec in path)
        {
            if (moveDir(vec, Vector3Int.right, ref tilemap))
                edges.Add(vec + Vector3Int.right);
            if (moveDir(vec, Vector3Int.left, ref tilemap))
                edges.Add(vec + Vector3Int.left);
            if (moveDir(vec, Vector3Int.down, ref tilemap))
                edges.Add(vec + Vector3Int.down);
            if (moveDir(vec, Vector3Int.up, ref tilemap))
                edges.Add(vec + Vector3Int.up);
        }

        foreach (Vector3Int vec in edges)
        {

            if (tilemap.GetTile(vec + Vector3Int.right) == null)
                tilemap.SetTile(vec, otherTiles.wallRight[Random.Range(0, otherTiles.wallRight.Length)]);
            if (tilemap.GetTile(vec + Vector3Int.left) == null)
                tilemap.SetTile(vec, otherTiles.wallLeft[Random.Range(0, otherTiles.wallLeft.Length)]);
            if (tilemap.GetTile(vec + Vector3Int.up) == null)
                tilemap.SetTile(vec, otherTiles.ground[Random.Range(0, otherTiles.ground.Length)]);
            if (tilemap.GetTile(vec + Vector3Int.down) == null)
                tilemap.SetTile(vec, otherTiles.celling[Random.Range(0, otherTiles.celling.Length)]);
            
            if (tilemap.GetTile(vec + Vector3Int.up) == null && tilemap.GetTile(vec + Vector3Int.right) == null)
                tilemap.SetTile(vec, otherTiles.rightCornorUp[Random.Range(0, otherTiles.rightCornorUp.Length)]);
            if (tilemap.GetTile(vec + Vector3Int.up) == null && tilemap.GetTile(vec + Vector3Int.left) == null)
                tilemap.SetTile(vec, otherTiles.leftCornorUp[Random.Range(0, otherTiles.leftCornorUp.Length)]);
            if (tilemap.GetTile(vec + Vector3Int.down) == null && tilemap.GetTile(vec + Vector3Int.right) == null)
                tilemap.SetTile(vec, otherTiles.rightCornorDown[Random.Range(0, otherTiles.rightCornorDown.Length)]);
            if (tilemap.GetTile(vec + Vector3Int.down) == null && tilemap.GetTile(vec + Vector3Int.left) == null)
                tilemap.SetTile(vec, otherTiles.leftCornorDown[Random.Range(0, otherTiles.leftCornorDown.Length)]);

            if (tilemap.GetTile(vec + Vector3Int.up + Vector3Int.right) != null && tilemap.GetTile(vec + Vector3Int.up + Vector3Int.left) == null && tilemap.GetTile(vec + Vector3Int.up) == null && tilemap.GetTile(vec + Vector3Int.up*2) == null)
            {
                stairsTilemap.SetTile(vec + Vector3Int.up, otherTiles.upSatirs[Random.Range(0, otherTiles.upSatirs.Length)]);
                tilemap.SetTile(vec, wallTiles[Random.Range(0, wallTiles.Length)]);

                // debuging
                //bTilemap.SetTile(vec + Vector3Int.up * 2 + Vector3Int.right, wallTiles[Random.Range(0, wallTiles.Length)]);
                //bTilemap.SetTile(vec + Vector3Int.up * 2 + Vector3Int.right * 2, wallTiles[Random.Range(0, wallTiles.Length)]);

                if (tilemap.GetTile(vec + Vector3Int.up * 2 + Vector3Int.right) == null && tilemap.GetTile(vec + Vector3Int.up * 2 + Vector3Int.right*2) == null)
                    tilemap.SetTile(vec + Vector3Int.up + Vector3Int.right, otherTiles.ground[Random.Range(0, otherTiles.ground.Length)]);
                else
                    tilemap.SetTile(vec + Vector3Int.up + Vector3Int.right, wallTiles[Random.Range(0, wallTiles.Length)]);
            }
            if (tilemap.GetTile(vec + Vector3Int.up + Vector3Int.left) != null && tilemap.GetTile(vec + Vector3Int.up + Vector3Int.right) == null && tilemap.GetTile(vec + Vector3Int.up) == null && tilemap.GetTile(vec + Vector3Int.up * 2) == null)
            {
                stairsTilemap.SetTile(vec + Vector3Int.up, otherTiles.downSatirs[Random.Range(0, otherTiles.downSatirs.Length)]);
                tilemap.SetTile(vec, wallTiles[Random.Range(0, wallTiles.Length)]);

                if (tilemap.GetTile(vec + Vector3Int.up*2 + Vector3Int.left) == null && tilemap.GetTile(vec + Vector3Int.up * 2 + Vector3Int.left * 2) == null)
                    tilemap.SetTile(vec + Vector3Int.up + Vector3Int.left, otherTiles.ground[Random.Range(0, otherTiles.ground.Length)]);
                else
                    tilemap.SetTile(vec + Vector3Int.up + Vector3Int.left, wallTiles[Random.Range(0, wallTiles.Length)]);
            }
        }
    }
    bool moveDir(Vector3Int cell,Vector3Int move, ref Tilemap tilemap)
    {
        if (tilemap.GetTile(cell + move) != null)
            return true;

        return false;
    }

    public void LoadData(GameData data)
    {
        if (data.tileMapInfos == null)
        {
            CreateCave();
            return;
        }
        saveSpawners = data.minionSpawners;
        spawnBoxes = new SpawnParams[data.minionSpawners.Count];
        for (int i = 0; i < spawnBoxes.Length; i++)
            spawnBoxes[i] = new (data.minionSpawners[i]);
        /*for (int i = 0; i < spawnBoxes.Length; i++)
        {
            RectTransform rec = new GameObject().AddComponent<RectTransform>();
            rec.position = new Vector2(data.minionSpawners[i].pos.x, data.minionSpawners[i].pos.y);
            rec.sizeDelta = new Vector2(data.minionSpawners[i].recSize.x, data.minionSpawners[i].recSize.y);

            spawnBoxes[i] =
        }*/
        foreach (SaveSpawners saveSpawn in data.minionSpawners)
        {
            /*RectTransform rec = new GameObject().AddComponent<RectTransform>();
            rec.position = new Vector2(saveSpawn.pos.x, saveSpawn.pos.y);
            rec.sizeDelta = new Vector2(saveSpawn.recSize.x, saveSpawn.recSize.y);*/

            SpawnParams spawnBox = new(saveSpawn);
            spawnBox.recParam = new GameObject().AddComponent<RectTransform>();
            spawnBox.recParam.position = new Vector2(saveSpawn.pos.x, saveSpawn.pos.y);
            spawnBox.recParam.sizeDelta = new Vector2(saveSpawn.recSize.x, saveSpawn.recSize.y);
            //Debug.Log(spawnBox.recParam.position + " : "+ new Vector2(saveSpawn.pos.x, saveSpawn.pos.y));
            minionSpawning.AddSpawnLoacation(spawnBox);
        }

        tilemap.ClearAllTiles();
        foreach (GameData.TileInfo tileInfo in data.tileMapInfos[0].tilesInfo)
        {
            if (tileInfo == null)
                break;

            tilemap.SetTile(new Vector3Int(tileInfo.coords.x,tileInfo.coords.y),Resources.Load<Tile>("Tiles/"+tileInfo.name));
        }
        foreach (GameData.TileInfo tileInfo in data.tileMapInfos[1].tilesInfo)
        {
            if (tileInfo == null)
                break;

            bTilemap.SetTile(new Vector3Int(tileInfo.coords.x, tileInfo.coords.y), Resources.Load<Tile>("Tiles/" + tileInfo.name));
        }
        foreach (GameData.TileInfo tileInfo in data.tileMapInfos[2].tilesInfo)
        {
            if (tileInfo == null)
                break;

            stairsTilemap.SetTile(new Vector3Int(tileInfo.coords.x, tileInfo.coords.y), Resources.Load<Tile>("Tiles/" + tileInfo.name));
        }
    }
    public void SaveData(ref GameData data)
    {
        //Vector2Int(-50,-mainPathLength/2), new Vector2Int(mainPathLength,80) // tilemap.cellBounds.xMin, tilemap.cellBounds.max.x
        Vector2Int tempVector = new Vector2Int(50+mainPathLength,mainPathLength/2+80);
        //data.minionSpawners = new SaveSpawners[saveSpawners.Count];
        //for (int i = 0; i < saveSpawners.Count; i++)
        //    data.minionSpawners[i] = saveSpawners[i];
        data.minionSpawners = new List<SaveSpawners>();
        /*foreach (GameData.SaveSpawners sp in saveSpawners)
        {
            data.WildMinions = new MinionClass.WildMinionSave[spBox.minions.Length];
            for (int i = 0; i < data.WildMinions.Length; i++)
                EntityManager.WildToSave(ref spBox.minions[i], ref data.WildMinions[i]);
        }*/
        for (int j = 0; j < saveSpawners.Count; j++)
        {
            data.minionSpawners.Add(saveSpawners[j]);
            /*data.minionSpawners[j].WildMinions = new MinionClass.WildMinionSave[saveSpawners[j].WildMinions.Length];
            for (int i = 0; i < data.WildMinions.Length; i++)
                data.minionSpawners[j].WildMinions[i] = saveSpawners[j].WildMinions[i];*/
        }

        data.tileMapInfos = new GameData.TileMapInfo[3];
        SaveTileMap(ref data, ref tilemap, 0);
        SaveTileMap(ref data, ref bTilemap, 1);
        SaveTileMap(ref data, ref stairsTilemap, 2);

        void SaveTileMap(ref GameData data,ref Tilemap tilemapSave,int mapIndex)
        {
            GameData.TileInfo[] allTiles = new GameData.TileInfo[tempVector.x * tempVector.y];
            int index = 0;
            for (int x = tilemapSave.cellBounds.xMin; x < tilemapSave.cellBounds.max.x; x++)
            {
                for (int y = tilemapSave.cellBounds.yMin; y < tilemapSave.cellBounds.max.y; y++)
                {
                    if (tilemapSave.GetTile(new Vector3Int(x, y)) == null)
                        continue;
                    allTiles[index] = new GameData.TileInfo();
                    allTiles[index].coords = (x, y);
                    allTiles[index].name = tilemapSave.GetTile(new Vector3Int(x, y)).name;
                    index++;
                }

            }
            data.tileMapInfos[mapIndex] = new GameData.TileMapInfo();
            data.tileMapInfos[mapIndex].tilesInfo = allTiles;
        }
    }

}
