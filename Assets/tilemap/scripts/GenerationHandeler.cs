using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationHandeler : MonoBehaviour
{
    [System.Serializable]
    public class SpawnParams : RandSpawnLocations
    {
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
    [Header("Background")]
    [SerializeField] Tilemap bTilemap;
    // Start is called before the first frame update
    void Start()
    {
        TilePlacer.FillTiles(ref tilemap,ref wallTiles,new Vector2Int(-50,-mainPathLength/2), new Vector2Int(mainPathLength,80));
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();
        Vector2Int endPos = TilePlacer.GeneratePath(Vector2Int.zero, mainPathLength, ref path, new TilePlacer.GenDirChances());

        int cunter = 0;
        int spawnAtCunter = Random.Range(stepsBetwenSpawners.x, stepsBetwenSpawners.y);
        foreach (Vector3Int pos in path)
        {
            if (spawnAtCunter == cunter)
            {
                spawnAtCunter += Random.Range(stepsBetwenSpawners.x, stepsBetwenSpawners.y);
                SpawnParams spawnBox = spawnBoxes[Random.Range(0,spawnBoxes.Length)];
                spawnBox.recParam = new GameObject().AddComponent<RectTransform>();
                spawnBox.recParam.position = tilemap.CellToWorld(pos);
                spawnBox.recParam.sizeDelta = spawnBox.recSize;
                minionSpawning.AddSpawnLoacation(spawnBox);
            }
            cunter++;
        }
        TilePlacer.RemoveTiles(ref tilemap,ref path,4);

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
}
