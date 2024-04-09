/*using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEditor.Animations;*/
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;

public class MinionWondering : MonoBehaviour, IDataPersitiens
{
    [SerializeField] bool isPresetSpawn; // if placing a wild minion by hand turn on this bool. This bool makes it so this GameObject is destoryed on the secound load and forth.
    [SerializeField] float timeBeforeEnableBattle;
    Rigidbody2D rbody;
    public float speed;
    public Vector2 randomTimer;
    public GameObject BasicBattleMinion;
    public MinionClass.BattleMinion[] battleMinions;
    [NonSerialized] public bool hasEnterdBattle;
    Animator animator;
    float timer;
    bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartBattleCountDown());
        animator = GetComponent<Animator>();
        timer = Random.Range(0, 3);

        rbody = GetComponent<Rigidbody2D>();

        if (!isPresetSpawn && EntityManager.instance != null)
            EntityManager.instance.AddMinion(gameObject);
    }
    IEnumerator StartBattleCountDown()
    {
        while (timeBeforeEnableBattle > 0)
        {
            timeBeforeEnableBattle -= Time.deltaTime;
            yield return null; 
        }
        timeBeforeEnableBattle = 0;
    }
    // Update is called once per frame
    float rot = 0;

    void Flip()
    {
        rot = Mathf.Abs(rot - 180);

        transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        speed *= -1;
    }
    void FixedUpdate()
    {
        // --Checks--
        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, LayerMask.NameToLayer("EnemyTeam"));
        Debug.DrawRay(transform.position, transform.right * 1, Color.red);
        if (hit.collider != null)
        {
            Flip();
            if (hit.collider.transform.name == "Player")
            {
                SceneManager.LoadScene("Battle");
            }
        }*/
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right+Vector3.down*0.5f, 5, LayerMask.NameToLayer("EnemyTeam"));
        Debug.DrawRay(transform.position, (transform.right + Vector3.down * 0.5f) * 5, Color.green);
        if (isMoving && hit2.collider == null)
        {
            Flip();
        }
        // --Timer--
        if (timer <= 0)
        {
            if (Random.Range(0,4) == 0)
            {
                isMoving = false;
                timer += 100;
            }
            else
                isMoving = true;
            Flip();
            timer += Random.Range(randomTimer.x,randomTimer.y);
        }
        else
            timer -= Time.fixedDeltaTime;
        animator.SetBool("IsAttacking",!isMoving);
        // --Movement--
        if (isMoving && Mathf.Abs(rbody.velocity.x) > Mathf.Abs(speed))
        {
           rbody.velocity = new Vector2 (speed, rbody.velocity.y);
        }
        else if (isMoving)
        {
            rbody.velocity = new Vector2(rbody.velocity.x + speed, rbody.velocity.y);
        }
        else
        {
            if (Random.Range(0,300) == 0)
                Flip();
        }
    }
    public void AddMinionDeck()
    {
        MinionDeck.Instance.basicBattleMinion = BasicBattleMinion;
        MinionDeck.Instance.minions = battleMinions;
    }
    void EnteringBattle()
    {
        if (timeBeforeEnableBattle != 0)
            return;

        AddMinionDeck();
        PlayerParty.Instance.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        hasEnterdBattle = true;
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene("Battle");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
            EnteringBattle();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
            EnteringBattle();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isMoving)
            Flip();
    }
    private void OnValidate()
    {
        foreach (MinionClass.BattleMinion minion in battleMinions)
        {
            if (minion.resetStats)
            {
                minion.resetStats = false;
                minion.stats = BasicBattleMinion.GetComponent<MinionBattleBasic>().stats;
            }
        }
    }

    public void LoadData(GameData data)
    {

        if (data.startSpawnForMinionsWorld1)
        {
            data.startSpawnForMinionsWorld1 = false;
            if (EntityManager.instance != null)
                EntityManager.instance.AddMinion(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void SaveData(ref GameData data)
    {

    }
}
