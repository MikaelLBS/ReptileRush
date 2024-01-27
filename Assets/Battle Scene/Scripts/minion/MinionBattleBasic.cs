using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class MinionBattleBasic : MonoBehaviour
{
    public bool isEnemy;
    public MinionClass.MinionStats stats;
    //public float ATK;
    //public float AttackSpeed;
    //public float HP;
    //public float Speed;
    //public float Range;
    [SerializeField] protected uint AmountOfKnockbacks;
    [SerializeField] protected float KnockbackRange;
    public Sprite icon; // the display icon in the UI
    //public int Cost;
    public float Cooldown;
    protected string enamyTag; // the tag for enemy minions
    protected LayerMask teamLayerMask; // the Layermask for allays
    
    Animator animator;
    float attackAnimeTime;

    float knockbackAt; // the amount of HP needed to reach the next knockback stage
    uint knockbacksLeft;
    bool isInKnockbackAnimation; // is true when minion is in knockback Animation

    public void DamgeTaken(float damge)
    {
        stats.HP -= damge;
        if (stats.HP <= 0) { Object.Destroy(gameObject); }
        else if (stats.HP <= knockbackAt * knockbacksLeft && !isInKnockbackAnimation)
        {
            IsKnockbacked();
            knockbacksLeft--;
        }
    }

    // --ATTACK--
    protected float attackCoolDown;
    protected bool attacked;
    protected Rigidbody2D body; // this minions body
    protected bool startedAttackAnime;
    protected float attackAnimeSpeed;
    protected RaycastHit2D hit;
    protected virtual void Attack()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, stats.Range, teamLayerMask);
        Debug.DrawRay(transform.position, transform.right * stats.Range, Color.green);
        if (hit.collider != null)
        {
            if (!attacked)
            { body.velocity *= Vector2.up;
                attacked = true;
                animator.SetBool("IsAttacking",true);
            }

            if (attackCoolDown <= 0)
            {
                startedAttackAnime = false;
                attackCoolDown += stats.AttackSpeed;
                animator.speed = 1;

                if (hit.collider.gameObject.tag == enamyTag)
                {
                    AttackMinion();
                }
                else if (hit.collider.gameObject.tag == "Base") // INPLEMENT WHEN HAVE TOWERS
                {
                    AttackTower();
                }
                else
                {
                    attacked = false;
                    animator.SetBool("IsAttacking", false);
                }
            }
            else
            {
                if (attackAnimeTime >= attackCoolDown && !startedAttackAnime)
                {
                    animator.speed = attackAnimeSpeed;
                    animator.SetTrigger("Attack");
                    startedAttackAnime = true;
                }

                attackCoolDown -= Time.deltaTime;
            }
        }
        else
        {
            attacked = false;
            animator.SetBool("IsAttacking", false);
        }
    }
    protected virtual void AttackMinion()
    {
        hit.collider.gameObject.GetComponent<MinionBattleBasic>().DamgeTaken(stats.ATK);
        attackCoolDown += stats.AttackSpeed;
    }
    protected virtual void AttackTower()
    {
        hit.collider.gameObject.GetComponent<BaseBasic>().DamgeTaken(stats.ATK);
        attackCoolDown += stats.AttackSpeed;
    }
    // --MOVE--
    protected virtual void Move()
    {
        if (!attacked && Mathf.Abs(body.velocity.x) < Mathf.Abs(stats.Speed) && !isInKnockbackAnimation)
        {
            body.velocity += new Vector2(stats.Speed, 0);
        }
    }
    protected virtual void IsKnockbacked()
    {
        animator.SetTrigger("Damaged");
        body.velocity = new Vector2(-KnockbackRange, 4);
        StartCoroutine(StartKnockBackAnimation());
    }
    protected virtual IEnumerator StartKnockBackAnimation()
    {
        LayerMask rayMask = ~(LayerMask.GetMask("PlayerTeam") + LayerMask.GetMask("EnemyTeam"));
        float length = Physics2D.Raycast(transform.position, Vector2.down, 10, rayMask).distance+0.01f;

        isInKnockbackAnimation = true;
        yield return new WaitForSeconds(0.1f);
        if (isEnemy)
            transform.rotation = Quaternion.Euler(0, 0, -25);
        else
            transform.rotation = Quaternion.Euler(0, 0, 25);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, length, rayMask);
            Debug.DrawRay(transform.position, Vector2.down*length, Color.red);
            if (hit.collider == null) {
                yield return null;
            }
            else { break; }
        }
        isInKnockbackAnimation = false;
        animator.SetTrigger("Grounded");
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackAnimeTime = animator.runtimeAnimatorController.animationClips[0].length;
        //Debug.Log(animator.runtimeAnimatorController.animationClips[0].name);

        attackCoolDown = stats.AttackSpeed;

        if (stats.AttackSpeed < attackAnimeTime)
        {
            attackAnimeSpeed = attackAnimeTime/ attackCoolDown;
            stats.AttackSpeed /= attackAnimeSpeed;
        }
        else attackAnimeSpeed = 1;

        knockbackAt = stats.HP / (AmountOfKnockbacks+1);
        knockbacksLeft = AmountOfKnockbacks;
        body = GetComponent<Rigidbody2D>();
        teamLayerMask = 0;
        if (isEnemy)
        {
            stats.Speed *= -1;
            stats.Range *= -1;
            KnockbackRange *= -1;

            transform.tag = "EnemyMinion";
            enamyTag = "PlayerMinion";
            gameObject.layer = LayerMask.NameToLayer("EnemyTeam");
            teamLayerMask = LayerMask.GetMask("PlayerTeam");
        }
        else
        {
            transform.tag = "PlayerMinion";
            enamyTag = "EnemyMinion";
            gameObject.layer = LayerMask.NameToLayer("PlayerTeam");
            teamLayerMask = LayerMask.GetMask("EnemyTeam");
        }
    }
    void Update()
    {
        Attack();
        Move();
    }
}
