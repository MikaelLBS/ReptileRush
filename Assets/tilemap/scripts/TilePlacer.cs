using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilePlacer : MonoBehaviour
{
    public static Vector2Int GeneratePath(Vector2Int startPos,int pathLength, ref HashSet<Vector3Int> path,GenDirChances genDirChances)
    {
        path.Add((Vector3Int)startPos);
        for (int i = 0; i < pathLength; i++)
        {
            startPos += GenDir(ref genDirChances);
            path.Add((Vector3Int)startPos);
        }
        return startPos;
    }
    public static void FillTiles(ref Tilemap tilemap, ref TileBase[] tiles, Vector2Int minPos,Vector2Int maxPos)
    {
        for (int x = minPos.x; x < maxPos.x; x++)
            for(int y = minPos.y; y < maxPos.y; y++)
                tilemap.SetTile(new Vector3Int(x,y), tiles[Random.Range(0, tiles.Length)]);
    }
    public static void PlaceTiles(ref Tilemap tilemap, ref TileBase[] tiles, HashSet<Vector3Int> path, int pathRadius)
    {
        if (pathRadius > 0)
            AddRadius(ref path,pathRadius);
        foreach (Vector3Int p in path)
        {
            tilemap.SetTile(p, tiles[Random.Range(0,tiles.Length)]);
        }
    }
    public static void RemoveTiles(ref Tilemap tilemap, ref HashSet<Vector3Int> path, int pathRadius)
    {
        if (pathRadius > 0)
            AddRadius(ref path, pathRadius);
        foreach (Vector3Int p in path)
        {
            tilemap.SetTile(p, null);
        }
    }
    public static HashSet<Vector3Int> CreateRoom(ref Tilemap tilemap,int moreRomoval, Vector2Int minPos, Vector2Int maxPos)
    {
        HashSet<Vector3Int> removedTiles = new HashSet<Vector3Int>();
        List<Vector2Int> searthCells = new List<Vector2Int>();
        for (int x = minPos.x; x < maxPos.x; x++)
            for (int y = minPos.y; y < maxPos.y; y++)
            {
                if (x == minPos.x || x == maxPos.x - 1)
                    searthCells.Add(new Vector2Int(x,y));
                else if (y == minPos.y || y == maxPos.y-1)
                    searthCells.Add(new Vector2Int(x, y));

                removedTiles.Add(new Vector3Int(x, y));
                tilemap.SetTile(new Vector3Int(x, y), null);
            }

        while (moreRomoval > 0 && searthCells.Count != 0)
        {

            int index = Random.Range(0,searthCells.Count);
            Vector2Int tempPos = searthCells[index];
            if (moveDir(ref tempPos, ref tilemap))
            {
                removedTiles.Add((Vector3Int)tempPos);
                tilemap.SetTile((Vector3Int)tempPos,null);
                searthCells.Add(tempPos);
                moreRomoval--;
            }
            else
                searthCells.RemoveAt(index);
        }
        return removedTiles;
    }
    static bool moveDir(ref Vector2Int moveCell,ref Tilemap tilemap)
    {
        if (tilemap.GetTile((Vector3Int)(moveCell+Vector2Int.up)) != null)
        {
            moveCell += Vector2Int.up;
            return true;
        }
        else if (tilemap.GetTile((Vector3Int)(moveCell + Vector2Int.down)) != null  && Random.Range(0,15) == 0)
        {
            moveCell += Vector2Int.down;
            return true;
        }
        else if (tilemap.GetTile((Vector3Int)(moveCell + Vector2Int.left)) != null)
        {
            moveCell += Vector2Int.left;
            return true;
        }
        else if (tilemap.GetTile((Vector3Int)(moveCell + Vector2Int.right)) != null)
        {
            moveCell += Vector2Int.right;
            return true;
        }
        return false;
    }
    static void AddRadius(ref HashSet<Vector3Int> path,int pathRadius)
    {
        HashSet<Vector3Int> tempPath = new HashSet<Vector3Int>();
        foreach (Vector3Int p in path)
        {
            for (int i = 1; i <= pathRadius; i++)
            {
                tempPath.Add(p+Vector3Int.up*i);
                tempPath.Add(p + Vector3Int.down * i);
            }
        }
        path.AddRange(tempPath);
    }
    [System.Serializable]
    public class GenDirChances
    {
        public GenDirChances() {
            up = 10;
            down = 13;
            left = 0;
            right = 40;
        }
        public GenDirChances(int up,int down,int left,int right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public int up;
        public int down;
        public int left;
        public int right;
        public int GetTotalSum()
        { return left + right + down + up; }
    }
    static Vector2Int GenDir(ref GenDirChances genDirChances)
    {
        int genNumber = Random.Range(0,genDirChances.GetTotalSum());

        if (genNumber < genDirChances.up)
            return Vector2Int.up+Vector2Int.right;
        else if (genNumber < genDirChances.up+genDirChances.down)
            return Vector2Int.down;
        else if (genNumber < genDirChances.up + genDirChances.down + genDirChances.left)
            return Vector2Int.left;
        else
            return Vector2Int.right;
    }
}
