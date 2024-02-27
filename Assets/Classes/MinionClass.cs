using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MinionClass
{
    [System.Serializable]
    public class MinionPreciseSpawn
    {
        public GameObject minion;
        public Transform spawnPosition;
        public bool radomizeBattleSatats;
        public MinionStats minionsRndStats;
    }
    [System.Serializable]
    public class MinionStats
    {
        public MinionStats()
        {
            ATK = -1;
            AttackSpeed = -1;
            HP = -1;
            Speed = -1;
            Range = -1;
            Cost = -1;
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
        public int ATK;
        public float AttackSpeed;
        public int HP;
        public float Speed;
        public float Range;
        public int Cost;
    }
    [System.Serializable]
    public class BattleMinion
    {
        public GameObject minion;
        public string minionName;
        public Sprite icon;
        public RuntimeAnimatorController animator;
        public bool resetStats;
        public MinionStats stats;
    }
    [System.Serializable]
    public class MinionSave
    {
        public GameObject minion;
        public string minionName;
        public Sprite icon;
        public int slotIndex;
        public RuntimeAnimatorController animator;
        public MinionStats stats;
        public MinionSave()
        {
            stats = new MinionStats();

            slotIndex = 0;
        }
        public MinionSave(BattleMinion battleMinion)
        {
            minion = battleMinion.minion;
            minionName = battleMinion.minionName;
            animator = battleMinion.animator;
            stats = battleMinion.stats;

            slotIndex = 0;
        }
        public MinionSave(MinionBattleBasic battleMinion, RuntimeAnimatorController animatorController)
        {
            minion = battleMinion.basePrefab;
            minionName = battleMinion.minionName;
            icon = battleMinion.icon;
            animator = animatorController;
            stats = battleMinion.stats;
            icon = battleMinion.icon;

            slotIndex = 0;
        }
    }


}
