using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class MinionBattleBasic : MonoBehaviour
{
    [SerializeField] protected bool isEnemy;
    [SerializeField] protected float ATK;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected float HP;
    [SerializeField] protected float Speed;
    [SerializeField] protected float Range;
    [SerializeField] protected uint AmountOfKnockbacks;
    [SerializeField] protected float KnockbackRange;
    public Sprite icon; // the display icon in the UI
    public int Cost;
    public float Cooldown;
    protected string enamyTag; // the tag for enemy minions
    protected LayerMask teamLayerMask; // the Layermask for allays

    float knockbackAt; // the amount of HP needed to reach the next knockback stage
    uint knockbacksLeft;
    bool isInKnockbackAnimation; // is true when minion is in knockback Animation

    public void DamgeTaken(float damge)
    {
        HP -= damge;
        if (HP <= 0) { Object.Destroy(gameObject); }
        else if (HP <= knockbackAt * knockbacksLeft && !isInKnockbackAnimation)
        {
            IsKnockbacked();
            knockbacksLeft--;
        }
    }
    protected float attackCoolDown;
    protected bool attacked;
    protected Rigidbody2D body; // this minions body
    protected virtual void Attack()
    {
        if (attackCoolDown <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Range, teamLayerMask);
            Debug.DrawRay(transform.position, transform.right * Range, Color.green);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == enamyTag)
                {
                    hit.collider.gameObject.GetComponent<MinionBattleBasic>().DamgeTaken(ATK);
                    attacked = true;
                    attackCoolDown += AttackSpeed;
                }
                else if (hit.collider.gameObject.tag == "enamyBase") // INPLEMENT WHEN HAVE TOWERS
                {
                    attacked = true;
                }
                else
                    attacked = false;
            }
            else
                attacked = false;
        }
        else
        {
            attackCoolDown -= Time.deltaTime;
        }
    }
    protected virtual void Move()
    {
        if (!attacked && Mathf.Abs(body.velocity.x) < Mathf.Abs(Speed) && !isInKnockbackAnimation)
        {
            body.velocity += new Vector2(Speed, 0);
        }
    }
    protected virtual void IsKnockbacked()
    {
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
    }
    private void Awake()
    {
        knockbackAt = HP / (AmountOfKnockbacks+1);
        knockbacksLeft = AmountOfKnockbacks;
        body = GetComponent<Rigidbody2D>();
        teamLayerMask = 0;
        if (isEnemy)
        {
            Speed *= -1;
            Range *= -1;
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
