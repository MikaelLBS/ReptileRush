using UnityEngine;

public class MinionBattleSpecial : MinionBattleBasic
{
    enum spacials // option of spacials attack/Abilitis
    {
        example,
        exampleTwo
    }
    [SerializeField] spacials specal;
    [Header("Area of attck")]
    [SerializeField] bool areaOfATK;
    [SerializeField] float areaSize;
    protected override void Attack()
    {
        switch (specal)
        {
            case spacials.example:
                base.Attack();
                break;
            case spacials.exampleTwo:

                break;
            default:
                base.Attack();
                break;
        }
    }
    protected override void Move()
    {
        switch (specal)
        {
            case spacials.example:
                base.Move();
                break;
            case spacials.exampleTwo:
                base.Move();
                break;
            default:
                base.Move();
                break;
        }
    }
}