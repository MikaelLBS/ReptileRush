using UnityEngine;
public class EnemyBot : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject []minions;

    int[] costs;
    float timer;
    [SerializeField] float resetTimer;
    int mana;
    // Start is called before the first frame update
    void Start()
    {
        costs = new int[minions.Length];
        for (int i = 0; i < minions.Length; i++)
        {
            costs[i] = minions[i].GetComponent<MinionBattleBasic>().Cost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            mana += 1;
            TrySummon();
            timer += resetTimer;
        }
        else
            timer -= Time.deltaTime;
    }
    void TrySummon()
    {
        int index = Random.Range(0, minions.Length);

        if (costs[index] <= mana)
        {
            mana -= costs[index];
            GameObject minion = Instantiate(minions[Random.Range(0, minions.Length)], spawnPos.position, Quaternion.identity);
        }
    }
}
