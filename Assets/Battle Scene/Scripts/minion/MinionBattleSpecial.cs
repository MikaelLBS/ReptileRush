using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionBattleSpecial : MinionBattleBasic
{
    enum Spacials // option of spacials attack/Abilitis
    {
        DontDamageMinion,
        Poison,
        Thorns,
        AreaOfAttack
    }
    [SerializeField] Spacials[] specals;
    [Header("Area of attck")]
    [SerializeField] float areaSize;

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
                    Debug.Log("AreaOfAttack");
                    Collider[] hitColliders = Physics.OverlapBox(hit.point, Vector3.one*areaSize, Quaternion.identity); // dosent work. Fix next time
                    Debug.DrawRay(hit.point,Vector2.right,Color.red,10f);
                    foreach (Collider hitCollider in hitColliders)
                    {
                        Debug.Log(hitCollider.name);
                        MinionBattleBasic minionBattkeBasicTemp = hitCollider.GetComponent<MinionBattleBasic>();

                        if (minionBattkeBasicTemp == null)
                        {
                            hitCollider.GetComponent<BaseBasic>().DamgeTaken(stats.ATK);
                            continue;
                        }

                        minionBattkeBasicTemp.DamgeTaken(stats.ATK);
                    }
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
        foreach (MinionBattleBasic poisendMinion in poisonedMinions)
        {
            poisendMinion.DamgeTaken(stats.ATK/5);
        }


    }
}