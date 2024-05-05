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
    public bool isBoss;
    [SerializeField] bool isPresetSpawn; // if placing a wild minion by hand turn on this bool. This bool makes it so this GameObject is destoryed on the secound load and forth.
    [SerializeField] float timeBeforeEnableBattle;
    [SerializeField] float jumpDelay;
    [SerializeField] AudioClip[] walkingSound;
    [SerializeField] AudioClip[] randomSound;
    AudioSource audioSource;
    float jumpTimer;
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
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartBattleCountDown());
        animator = GetComponent<Animator>();
        if (animator == null )
            animator = GetComponentInChildren<Animator>();
        timer = Random.Range(0, 3);

        rbody = GetComponent<Rigidbody2D>();

        if (!isPresetSpawn && EntityManager.instance != null)
            EntityManager.instance.AddMinion(gameObject);
        StartCoroutine(EnableRbSim());
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
    bool isRuning = false;
    public void RunFrom(Transform target)
    {
        isRuning = true;
        StartCoroutine(Run(target));
    }
    public void StopRuning()
    {
        isRuning = false;
    }
    IEnumerator Run(Transform target)
    {
        animator.SetBool("IsAttacking", false);
        bool targetIsRight = false;
        if (transform.position.x < target.position.x)
            targetIsRight = true;

        while (isRuning)
        {
            if (targetIsRight && transform.position.x > target.position.x)
            {
                FaceRight(false);
                targetIsRight = false;
            }
            else if (transform.position.x < target.position.x)
            {
                FaceRight(true);
                targetIsRight = true;
            }

            if (Mathf.Abs(rbody.velocity.x) > Mathf.Abs(speed))
            {
                rbody.velocity = new Vector2(speed*3f, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(rbody.velocity.x + speed*3f, rbody.velocity.y);
            }
            if (walkingSound.Length != 0)
                SoundFunctions.PlaySoundDontOverrite(audioSource, walkingSound);
            yield return new WaitForFixedUpdate();
        }
    }

    float rot = 0;
    void Flip()
    {
        if (randomSound.Length != 0 && Random.Range(0,4) == 0)
            SoundFunctions.PlaySound(audioSource,randomSound);
        rot = Mathf.Abs(rot - 180);

        transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        speed *= -1;
    }
    void FaceRight(bool faceRight)
    {
        if (faceRight)
        {
            rot = 0;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            speed = MathF.Abs(speed);
        }
        else
        {
            rot = 180;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            speed = -MathF.Abs(speed);
        }
    }
    void FixedUpdate()
    {
        if (jumpTimer > 0)
            jumpTimer -= Time.fixedDeltaTime;

        if (isRuning)
            return;

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
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right+Vector3.down*0.5f, 5, 64);
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
                timer += Random.Range(randomTimer.x, randomTimer.y)*0.5f;
            }
            else
                isMoving = true;
            Flip();
            timer += Random.Range(randomTimer.x,randomTimer.y);
        }
        else
            timer -= Time.fixedDeltaTime;
        animator.SetBool("IsAttacking",!isMoving);
        if (isMoving)
        {
            if (walkingSound.Length != 0)
                SoundFunctions.PlaySoundDontOverrite(audioSource, walkingSound);
        }
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
        if (MinionDeck.Instance == null)
            MinionDeck.SetInstance();

        MinionDeck.Instance.basicBattleMinion = BasicBattleMinion;
        MinionDeck.Instance.minions = battleMinions;

        if (isBoss)
        {
            PlayerParty.Instance.isBoss = true;
            foreach (MinionClass.BattleMinion bMinion in MinionDeck.Instance.minions)
                bMinion.stats.ATK *= 2;
        }
    }
    void EnteringBattle()
    {
        if (timeBeforeEnableBattle != 0)
            return;

        hasEnterdBattle = true;
        if (isFinalBoss)
        { DataPersistenceManager.Instance.HasEnterdFinal(); }
        AddMinionDeck();
        PlayerParty.Instance.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene("Battle");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
            EnteringBattle();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
            EnteringBattle();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (jumpTimer > 0)
            return;
        //Debug.Log(collision.gameObject.layer);
        jumpTimer = jumpDelay;
        if (isMoving && collision.gameObject.layer == 6)
            rbody.AddForce(Vector2.up*200f);
    }
    private void OnValidate()
    {
        foreach (MinionClass.BattleMinion minion in battleMinions)
        {
            if (minion.resetStats)
            {
                minion.resetStats = false;

                MinionBattleBasic minBattle;
                if (minion.minion == null)
                    minBattle = BasicBattleMinion.GetComponent<MinionBattleBasic>();
                else
                    minBattle = minion.minion.GetComponent<MinionBattleBasic>();

                //minion.stats = BasicBattleMinion.GetComponent<MinionBattleBasic>().stats;

                minion.stats = minBattle.stats;
            }
        }
    }

    public void LoadData(GameData data)
    {

        if (data.startSpawnForMinionsWorld1)
        {
            if (EntityManager.instance != null)
                EntityManager.instance.AddMinion(gameObject);
        }
        else
            Destroy(gameObject);
    }
    IEnumerator EnableRbSim()
    {
        for (int i = 0; i < 20; i++)
            yield return new WaitForFixedUpdate();

        rbody.simulated = true;
    }
    public void SaveData(ref GameData data)
    {
        data.startSpawnForMinionsWorld1 = false;
    }
    // test var
    protected bool isFinalBoss;
}
