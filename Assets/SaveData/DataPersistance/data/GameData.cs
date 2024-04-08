using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public bool startSpawnForMinionsWorld1;
    public MinionClass.WildMinionSave[] WildMinions;
    //public MinionClass.MinionSave[] partyMinions;
    // music
    public float[] soundsVolume;



    public GameData()
    {
        startSpawnForMinionsWorld1 = true;
        //test = new();
        //soundsVolume = null;
    }
}
