using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MinionBattleSpecial : BasicMinion
{
    enum spacials
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
                Debug.Log("ATTCK");
                base.Attack();
                break;
            case spacials.exampleTwo:

                break;
        }
    }
    protected override void Move()
    {
        switch (specal)
        {
            case spacials.example:
                break;
            case spacials.exampleTwo:
                Debug.Log("MOVE");
                base.Move();
                break;
        }
    }
}