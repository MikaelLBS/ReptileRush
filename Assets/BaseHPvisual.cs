using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseHPvisual : MonoBehaviour
{
    [SerializeField] BaseBasic basse;
    [SerializeField] TextMeshPro TextMeshPro;

    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = "HP: " + basse.HP.ToString();
    }
}
