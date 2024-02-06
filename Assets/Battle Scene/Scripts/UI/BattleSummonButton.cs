using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSummonButton : MonoBehaviour
{
    [NonSerialized] public GameObject minion;
    [NonSerialized] public int cost;
    [NonSerialized] public int index; // index in minions array in Battle Canvas for this button
    [NonReorderable] public Transform spawnPos; // minion spawn position
    [SerializeField] public Slider coolDownSlider;
    [SerializeField] TMPro.TextMeshProUGUI costText;

    void Start()
    {
        cost = minion.GetComponent<MinionBattleBasic>().stats.Cost;
        coolDownSlider.maxValue = minion.GetComponent<MinionBattleBasic>().Cooldown;

        costText.text = cost.ToString();
    }

    public void ButtonDown()
    {
        if (cost <= GetComponentInParent<BattleCanvas>().mana)
        {
            GetComponentInParent<BattleCanvas>().ChangeMana(-cost);
            GetComponentInParent<BattleCanvas>().disableCheckCost(index);
            transform.GetComponent<Button>().interactable = false;
            StartCoroutine(StartCooldown(minion.GetComponent<MinionBattleBasic>().Cooldown));

            Instantiate(minion, spawnPos.position, Quaternion.identity).gameObject.SetActive(true);
        }

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
        GetComponentInParent<BattleCanvas>().enableCheckCost(index);
        if (GetComponentInParent<BattleCanvas>().mana >= cost)
            transform.GetComponent<Button>().interactable = true;
    }
}
