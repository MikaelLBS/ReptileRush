using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBattleSpecial : BasicMinion
{
    enum spacials
    {
        None,
        example,
        exampleTwo
    }
    [SerializeField] spacials specal;
    [Header("Area of attck")]
    [SerializeField] bool areaOfATK;
    [SerializeField] float areaSize;
    // Start is called before the first frame update
    void Start()
    {
        switch (specal)
        {
            case spacials.example:
                    
                break;
            case spacials.exampleTwo:

                break;
        }
    }
    protected override void Attack()
    {
        Debug.Log("ATTCK");
        base.Attack();
    }
}