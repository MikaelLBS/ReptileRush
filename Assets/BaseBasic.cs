using System;
using System.Collections;
using System.Net;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[System.Serializable]
public class BaseBasic : MonoBehaviour
{
    [SerializeField] protected bool isEnemy;
    [SerializeField] protected float ATK;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected float HP;
    [SerializeField] protected float Range;
    [SerializeField] protected int Attacks;
    [SerializeField] Button attackbutton;
    public Sprite icon; // the display icon in the UI        
    protected string enamyTag; // the tag for enemy minions
    protected LayerMask teamLayerMask; // the Layermask for allays
    public bool attackReady = false;

    Animator animator;
    float attackAnimeTime;

    public void DamgeTaken(float damge)
    {
        HP -= damge;
        if (HP <= 0) { Win(); }
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
        if (attackCoolDown <= 0)
        {
            //attackReady = true;
            if (!isEnemy)
                attackbutton.interactable = true;
        }
        else
        {
            attackCoolDown -= Time.deltaTime;
        }
    }

    protected virtual void AttackMinion()
    {
        Debug.Log("attack");
        hit.collider.gameObject.GetComponent<MinionBattleBasic>().DamgeTaken(ATK);
    }

    private void Awake()
    {
        if (isEnemy)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.tag = "Base";
            enamyTag = "PlayerMinion";
            gameObject.layer = LayerMask.NameToLayer("EnemyTeam");
            teamLayerMask = LayerMask.GetMask("PlayerTeam");
        }
        else
        {
            transform.tag = "Base";
            enamyTag = "EnemyMinion";
            gameObject.layer = LayerMask.NameToLayer("PlayerTeam");
            teamLayerMask = LayerMask.GetMask("EnemyTeam");
        }
    }
    void Update()
    {               
        Attack();
    }
    public void AttackButton()
    {
        //--player press button--

        //if (attackReady)
        //{ return; }

        //attackReady = false;
        attackCoolDown += AttackSpeed;
        if (hit.collider != null && hit.collider.gameObject.tag == enamyTag)
        {
            for (int i = 0; i < Attacks; i++)
                AttackMinion();
        }       
    }
    void Win()
    {

    }
}
