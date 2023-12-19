using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSummonButton : MonoBehaviour
{
    [NonSerialized] public GameObject minion;
    [NonReorderable] public Transform spawnPos;
    [SerializeField] Slider coolDownSlider;

    void Start()
    {
        coolDownSlider.maxValue = minion.GetComponent<BasicMinion>().Cooldown;
    }

    public void ButtonDown()
    {
        transform.GetComponent<Button>().interactable = false;
        StartCoroutine(StartCooldown(minion.GetComponent<BasicMinion>().Cooldown));

        Instantiate(minion, spawnPos.position, Quaternion.identity);

    }
    IEnumerator StartCooldown(float coolDown)
    {
        while (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            coolDownSlider.value = coolDown;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        coolDownSlider.value = 0;
        transform.GetComponent<Button>().interactable = true;
    }
}
