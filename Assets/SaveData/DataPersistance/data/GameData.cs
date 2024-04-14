using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class GameData
{
    // wild minions
    public bool startSpawnForMinionsWorld1;
    public MinionClass.WildMinionSave[] WildMinions;

    // player party
    public int sceneIndex;
    public MinionClass.MinionFileSave[] PartyMinions;
    // player
    public bool playerPosWasSaved;
    public (float x, float y) worldPos;

    //Generation Handeler
    public class TileInfo
    {
        public string name;
        public (int x, int y) coords;
    }
    public class TileMapInfo
    {
        public TileInfo[] tilesInfo;
    }
    public TileMapInfo[] tileMapInfos;

    // music
    public float[] soundsVolume;



    public GameData()
    {
        startSpawnForMinionsWorld1 = true;
        playerPosWasSaved = false;
    }
}
