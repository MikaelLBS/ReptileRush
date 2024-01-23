using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class BaseBasic : MonoBehaviour
{
    [SerializeField] protected bool isEnemy;
    [SerializeField] protected float ATK;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected float HP;
    [SerializeField] protected float Range;    
    public Sprite icon; // the display icon in the UI        
    protected string enamyTag; // the tag for enemy minions
    protected LayerMask teamLayerMask; // the Layermask for allays

    Animator animator;
    float attackAnimeTime;           

    public void DamgeTaken(float damge)
    {
        HP -= damge;
        if (HP <= 0) { Object.Destroy(gameObject); }        
    }

    // --ATTACK--
    protected float attackCoolDown;
    protected bool attacked;
    protected bool startedAttackAnime;
    protected float attackAnimeSpeed;
    protected RaycastHit2D hit;
    protected virtual void Attack()
    {
        hit = Physics2D.Raycast(transform.position+new Vector3(0, -2, 0), transform.right, Range, teamLayerMask);
        Debug.DrawRay(transform.position+new Vector3(0, -2, 0), transform.right * Range, Color.green);
        if (hit.collider != null)
        {
            Debug.Log("ahhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh");
            //lägg till så tornet kan skada
        }
    }

    protected virtual void AttackMinion()
    {
        hit.collider.gameObject.GetComponent<MinionBattleBasic>().DamgeTaken(ATK);
        attackCoolDown += AttackSpeed;
    }

    private void Awake()
    {

    }
    void Update()
    {
        Attack();
    }
}
