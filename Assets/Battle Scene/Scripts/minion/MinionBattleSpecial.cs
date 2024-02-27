using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionBattleSpecial : MinionBattleBasic
{
    enum Spacials // option of spacials attack/Abilitis
    {
        example,
        posion
    }
    [SerializeField] Spacials[] specals;
    [Header("Area of attck")]
    [SerializeField] bool areaOfATK;
    [SerializeField] float areaSize;

    // posion
    List<MinionBattleBasic> poisonedMinions;
    float posionTimer;
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
                case Spacials.posion:

                    //base.AttackMinion();
                    if (hit.collider.gameObject.GetComponent<MinionBattleBasic>() != null && (poisonedMinions == null || !poisonedMinions.Contains(hit.collider.gameObject.GetComponent<MinionBattleBasic>())))
                        poisonedMinions.Add(hit.collider.gameObject.GetComponent<MinionBattleBasic>()); // error some how fix next time

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
        if (posionTimer > 0)
            return;
        Debug.Log("PoisonAttack");
        posionTimer += stats.AttackSpeed / 3;
        foreach (MinionBattleBasic poisendMinion in poisonedMinions)
        {
            poisendMinion.DamgeTaken(stats.ATK/5);
        }


    }
}