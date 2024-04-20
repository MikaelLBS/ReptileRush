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
    public class BattleMinion : GenericBattleMinion
    {
        public GameObject minion;
        public Sprite icon;
        public RuntimeAnimatorController animator;
        public bool resetStats;
        public void LoadFromGeneric(GenericBattleMinion genericBattle)
        {
            minionName = genericBattle.minionName;
            stats = genericBattle.stats;
        }

    }
    [System.Serializable]
    public class BattleMinionSave
    {
        public GenericBattleMinion minionSave;

        public string minionPrefabPath;
        public string iconAssetPath;
        public string animatorAssetPath;

    }
    [System.Serializable]
    public class MinionSave : GenericMinionSave
    {
        public Sprite icon;
        public RuntimeAnimatorController animator;
        public GameObject minion;
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

        public void LoadFromGeneric(GenericMinionSave genericBattle)
        {
            minionName = genericBattle.minionName;
            slotIndex = genericBattle.slotIndex;
            stats = genericBattle.stats;
        }
    }
    public class MinionFileSave
    {
        public GenericMinionSave minionSave;

        public string minionPrefabPath;
        public string iconAssetPath;
        public string animatorAssetPath;
    }
    [System.Serializable]
    public class WildMinionSave
    {
        public string wildMinionPrefabPath;
        public (float x, float y) worldCoords;

        public float speed;
        public (float x, float y) randomTimer;
        public bool isBoss;
        public bool hasEnterdBattle;
        public BattleMinionSave[] battleMinions;

        public (float r, float g, float b, float a) color;
        public (float x, float y) size;
    }
    // Generic Classes
    public class GenericBattleMinion
    {
        public GenericBattleMinion()
        {

        }
        public GenericBattleMinion(GenericBattleMinion minion)
        {
            minionName = minion.minionName;
            stats = minion.stats;
        }
        public string minionName;
        public MinionStats stats;
    }
    public class GenericMinionSave
    {
        public GenericMinionSave() { }
        public GenericMinionSave(GenericMinionSave minion)
        {
            stats = minion.stats;
            minionName = minion.minionName;
            slotIndex = minion.slotIndex;
        }
        public MinionStats stats;
        public string minionName;
        public int slotIndex;
    }

}
