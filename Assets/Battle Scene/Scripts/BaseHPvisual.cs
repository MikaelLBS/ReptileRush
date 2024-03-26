using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseHPvisual : MonoBehaviour
{
    [SerializeField] BaseBasic BaseBasic;
    [SerializeField] TextMeshPro TextMeshPro;
    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = "HP: " + BaseBasic.HP.ToString();
    }
}
