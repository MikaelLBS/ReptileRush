using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour, IDataPersitiens
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool isEnteringBattle = false;
    void EnteringBattle()
    {
        isEnteringBattle=true;
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

    public void LoadData(GameData data)
    {
    }
    public void SaveData(ref GameData data)
    {
        if (isEnteringBattle)
            data.hasEnterFinalBoss = true;
    }
}
