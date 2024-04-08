using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    // wild minions
    public bool startSpawnForMinionsWorld1;
    public MinionClass.WildMinionSave[] WildMinions;

    // player party
    public int sceneIndex;
    public MinionClass.MinionFileSave[] PartyMinions;

    // music
    public float[] soundsVolume;



    public GameData()
    {
        startSpawnForMinionsWorld1 = true;
        //test = new();
        //soundsVolume = null;
    }
}
