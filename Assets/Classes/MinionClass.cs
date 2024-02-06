using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinionClass
{
    [System.Serializable]
    public class MinionStats
    {
        public MinionStats()
        {
            ATK = 0;
            AttackSpeed = 0;
            HP = 0;
            Speed = 0;
            Range = 0;
            Cost = 0;
        }
        public MinionStats(MinionStats stats)
        {
            ATK = stats.ATK;
            AttackSpeed = stats.AttackSpeed;
            HP = stats.HP;
            Speed = stats.Speed;
            Range = stats.Range;
            Cost = stats.Cost;
        }
        public float ATK;
        public float AttackSpeed;
        public float HP;
        public float Speed;
        public float Range;
        public int Cost;
    }
    [System.Serializable]
    public class BattleMinion
    {
        public GameObject minion;
        public string minionName;
        public RuntimeAnimatorController animator;
        public bool resetStats;
        public MinionStats stats;

    }


}
