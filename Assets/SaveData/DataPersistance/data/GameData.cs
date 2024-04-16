using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public class SaveSpawners
    {
        [NonSerialized] public Vector2 maxPoint;
        [NonSerialized] public Vector2 minPoint;
        public int cycleDelay; // how many cycles before spawn
        [NonSerialized] public int cycle; // amount of cycles left
        public Vector2Int spawnChance;

        public (float x, float y) recSize;
        public MinionClass.WildMinionSave[] WildMinions;
        public (float x, float y) pos;
        public SaveSpawners() { }
        public SaveSpawners(GenerationHandeler.SpawnParams sp)
        {
            maxPoint = sp.maxPoint;
            minPoint = sp.minPoint;
            cycleDelay = sp.cycleDelay;
            cycle = sp.cycle;
            spawnChance = sp.spawnChance;
            recSize = (sp.recSize.x, sp.recSize.y);

            WildMinions = new MinionClass.WildMinionSave[sp.minions.Length];
            for (int i = 0; i < sp.minions.Length; i++)
            {
                EntityManager.WildToSave(ref sp.minions[i], ref WildMinions[i]);
            }
        }
    }
    public TileMapInfo[] tileMapInfos;
    public List<SaveSpawners> minionSpawners; // Need to save minion Spawners! ! !

    // Bosses
    public MinionClass.WildMinionSave[] bosses;

    // music
    public float[] soundsVolume;

    // other
    public void NewLevelDataReset()
    {
        playerPosWasSaved = false;
        tileMapInfos = null;
        minionSpawners = null;
        WildMinions = new MinionClass.WildMinionSave[0];
    }

    public GameData()
    {
        startSpawnForMinionsWorld1 = true;
        playerPosWasSaved = false;
    }
}
