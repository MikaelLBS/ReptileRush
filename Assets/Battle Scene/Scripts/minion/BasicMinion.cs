using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class BasicMinion : MonoBehaviour
{
    [SerializeField] protected bool isEnemy;
    [SerializeField] protected float ATK;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected float HP;
    [SerializeField] protected float Speed;
    [SerializeField] protected float Range;
    public Sprite image;
    public int Cost;
    public float Cooldown;
    protected string enamyTag;
    protected LayerMask teamLayerMask;

    public void DamgeTaken(float damge)
    {
        HP -= damge;
        if (HP <= 0) { Object.Destroy(gameObject); }
    }
    protected float attackCoolDown;
    protected bool attacked;
    protected Rigidbody2D body;
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
                    hit.collider.gameObject.GetComponent<BasicMinion>().DamgeTaken(ATK);
                    attacked = true;
                    attackCoolDown += AttackSpeed;
                }
                else if (hit.collider.gameObject.tag == "enamyBase")
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
    public virtual void Move()
    {
        if (!attacked && Mathf.Abs(body.velocity.x) < Mathf.Abs(Speed))
        {
            body.velocity += new Vector2(Speed, 0);
        }
    }
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        teamLayerMask = 0;
        if (isEnemy)
        {
            Speed *= -1;
            Range *= -1;

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
