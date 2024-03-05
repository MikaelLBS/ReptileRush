using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHoveringIcon : MonoBehaviour
{
    public GameObject hoveringIcon;
    public void ShowIcon()
    {
        hoveringIcon.SetActive(true);
    }
    public void HideIcon()
    {
        hoveringIcon.SetActive(false);
    }
}
