using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MinionBattleSpecial : MinionBattleBasic
{
    public enum Spacials // option of spacials attack/Abilitis
    {
        DontDamageMinion,
        Poison,
        Thorns,
        AreaOfAttack
    }
    public Spacials[] specals;
    // area of attack
    [Header("Area of attck")]
    [SerializeField] float offPut;
    [SerializeField] float areaSize;
    void AreaAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(hit.point + Vector2.right * offPut, Vector2.one*areaSize, 0f);
        Debug.DrawRay(hit.point + Vector2.right * offPut*1.5f, Vector2.right* areaSize, Color.red, 3f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.name == gameObject.name)
                continue;
            MinionBattleBasic minionBattkeBasicTemp = hitCollider.GetComponent<MinionBattleBasic>();

            if (minionBattkeBasicTemp == null)
            {
                if (hitCollider.GetComponent<BaseBasic>() != null)
                    hitCollider.GetComponent<BaseBasic>().DamgeTaken(stats.ATK);
                continue;
            }

            minionBattkeBasicTemp.DamgeTaken(stats.ATK);
        }
    }
    // posion
    List<MinionBattleBasic> poisonedMinions = new();
    float poisonTimer;
    // thorns
    [NonSerialized]
    public bool hasThorns;

    private void Start()
    {
        foreach (var specal in specals)
        {
            switch (specal)
            {
                case Spacials.Thorns:
                    hasThorns = true;
                    break;
                default:
                    break;
            }
        }
    }
    protected override void Attack()
    {
        foreach (var specal in specals)
        {
            switch (specal)
            {
                default:
                    base.Attack();
                    break;
            }
        }
    }
    protected override void AttackMinion()
    {
        foreach (var specal in specals)
        {
            switch (specal)
            {
                case Spacials.DontDamageMinion:
                    break;
                case Spacials.Poison:

                    base.AttackMinion();
                    if (!poisonedMinions.Contains(hit.collider.gameObject.GetComponent<MinionBattleBasic>()))
                        poisonedMinions.Add(hit.collider.gameObject.GetComponent<MinionBattleBasic>());

                    break;
                case Spacials.AreaOfAttack:
                    AreaAttack();
                    break;
                default:
                    base.AttackMinion();
                    break;
            }
        }
    }
    protected override void AttackTower()
    {
        foreach (var specal in specals)
        {
            switch (specal)
            {
                case Spacials.AreaOfAttack:
                    AreaAttack();
                    break;
                default:
                    base.AttackTower();
                    break;
            }
        }
    }
    protected override void Move()
    {
        foreach (var specal in specals)
        {
            switch (specal)
            {
                default:
                    base.Move();
                    break;
            }
        }
    }
    protected override void Extras()
    {
        if (poisonedMinions != null)
            PosionDamage();
    }
    void PosionDamage()
    {
        if (poisonedMinions.Count == 0)
            return;
        if (poisonTimer > 0)
        {
            poisonTimer -= Time.deltaTime;
            return;
        }

        poisonTimer += stats.AttackSpeed / 3;
        for (int i = 0; i < poisonedMinions.Count; i++)
        {
            if (poisonedMinions[i] == null)
                poisonedMinions.RemoveAt(i);
            else
                poisonedMinions[i].DamgeTaken(stats.ATK / 5);

        }


    }
}