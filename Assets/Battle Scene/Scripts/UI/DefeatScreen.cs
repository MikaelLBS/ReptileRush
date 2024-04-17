using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] Image m_Image;
    private void OnEnable()
    {
        int index = Random.Range(0, PlayerParty.Instance.minions.Count);
        m_Image.sprite = PlayerParty.Instance.minions[index].icon;
        PlayerParty.Instance.minions.RemoveAt(index);
        for (int i = index; i < PlayerParty.Instance.minions.Count-index; i++)
            PlayerParty.Instance.minions[i].slotIndex = i;
    }
}
