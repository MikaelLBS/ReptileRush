/*using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEditor.Animations;*/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MinionWondering : MonoBehaviour
{

    Rigidbody2D rbody;
    [SerializeField] float speed;
    [SerializeField] Vector2 randomTimer;
    [SerializeField] GameObject BasicBattleMinion;
    public MinionClass.BattleMinion[] battleMinions;
    float timer;
    // Start is called before the first frame update
    void Start()
    {

        timer = Random.Range(0, 3);

        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    float rot = 0;

    void Flip()
    {
        rot = Mathf.Abs(rot - 180);

        transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        speed *= -1;
    }
    void Update()
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
        if (hit2.collider == null)
        {
            Flip();
        }
        // --Timer--
        if (timer <= 0)
        {
            Flip();
            timer += Random.Range(randomTimer.x,randomTimer.y);
        }
        else
            timer -= Time.deltaTime;
        // --Movement--
        if (Mathf.Abs(rbody.velocity.x) > Mathf.Abs(speed))
        {
           rbody.velocity = new Vector2 (speed, rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(rbody.velocity.x + speed, rbody.velocity.y);
        }
    }
    void AddMinionDeck()
    {
        MinionDeck.Instance.basicBattleMinion = BasicBattleMinion;
        MinionDeck.Instance.minions = battleMinions;    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            AddMinionDeck();
            PlayerParty.Instance.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("Battle");
        }
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
}
